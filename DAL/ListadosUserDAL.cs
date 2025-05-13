using Azure;
using DAL.Utils;
using DTO;
using Entidades;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Input;

namespace DAL
{
    public class ListadosUserDAL
    {
        /// <summary>
        /// Esta función obtiene todos los usuarios de la base de datos y los devuelve como una lista
        /// </summary>
        /// <returns>Lista</returns>
        public static List<Usuario> getAllUsers()
        {
            List<Usuario> usuarios = new List<Usuario>();

            Usuario user = null;
            String uid = "";
            String name = "";
            String lastName = "";
            String email = "";
            String photoUrl = "";
            String dateJoining = "";
            String username = "";
            String supplier = "";
            bool blocked = false;
            bool deleted = false;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.CommandText = "SELECT * FROM USERS WHERE UserDeleted = 0 AND UserBlocked = 0";

                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        uid = (String)miLector["UID"];
                        name = (String)miLector["Name"];
                        lastName = (String)miLector["LastName"];
                        email = (String)miLector["Email"];
                        photoUrl = (String)miLector["PhotoUrl"];
                        dateJoining = ((DateTime)miLector["DateJoining"]).ToString();
                        username = (String)miLector["Username"];
                        supplier = (String)miLector["Supplier"];
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uid, name, lastName, email, photoUrl, dateJoining, username, supplier, deleted, blocked);
                        usuarios.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe un username y busca en la base de datos todos los usuarios que su username concuerde con el recibido
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="username">Username a buscar</param>
        /// <returns>Lista</returns>
        public static List<Usuario> getUsersByUsername(String username)
        {
            List<Usuario> usuarios = new List<Usuario>();

            Usuario user = null;
            String uid = "";
            String name = "";
            String lastName = "";
            String email = "";
            String photoUrl = "";
            String dateJoining = "";
            String usernameUser = "";
            String supplier = "";
            bool blocked = false;
            bool deleted = false;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = "%" + username + "%";
                miComando.CommandText = "SELECT * FROM USERS WHERE Username LIKE @username AND UserDeleted = 0 AND UserBlocked = 0";

                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        uid = (String)miLector["UID"];
                        name = (String)miLector["Name"];
                        lastName = (String)miLector["LastName"];
                        email = (String)miLector["Email"];
                        photoUrl = (String)miLector["PhotoUrl"];
                        dateJoining = ((DateTime)miLector["DateJoining"]).ToString();
                        usernameUser = (String)miLector["Username"];
                        supplier = (String)miLector["Supplier"];
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uid, name, lastName, email, photoUrl, dateJoining, usernameUser, supplier, deleted, blocked);
                        usuarios.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene las canciones que le han gustado
        /// y las devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="page">Número de página</param>
        /// <param name="limit">Número de canciones por página</param>
        /// <returns>Objeto con información y lista de canciones</returns>
        public static async Task<PaginatedTracks> getLikedTracksDAL(String uid, int page, int limit, String baseUrl)
        {
            PaginatedTracks paginatedTracks = new PaginatedTracks();
            List<Track> tracks = new List<Track>();

            // Primero obtenemos los ids de las canciones de la base de datos
            var result = getListIds("EXEC GetLikedTracks @UID, @page, @limit", uid, page, limit);

            // Calcular total de canciones
            int offset = (page - 1) * limit + 1;

            if (page > 1)
            {
                paginatedTracks.LinkPreviousPage = $"{baseUrl}?page={page - 1}&limit={limit}";
            }

            if (page < result.totalPages) {
                paginatedTracks.LinkNextPage = $"{baseUrl}?page={page + 1}&limit={limit}";
            }

            // Crear una lista de tareas para manejar las peticiones de forma concurrente
            List<Task<Track>> trackTasks = new List<Task<Track>>();

            // Se añaden las tareas a la lista
            foreach (long trackId in result.list)
            {
                // Verificamos si la canción ya está en el cache
                if (DeezerCache.TryGetTrack(trackId, out Track cachedTrack))
                {
                    tracks.Add(cachedTrack); // Si está en el cache, lo agregamos directamente
                }
                else
                {
                    trackTasks.Add(CallApiDeezer.HandleRateLimitAndGetTrack(trackId)); // Si no está, la solicitamos
                }
            }

            // Esperamos que todas las tareas se completen
            var trackResults = await Task.WhenAll(trackTasks);

            // Agregamos los resultados a la lista final y los almacenamos en el cache
            foreach (var track in trackResults.Where(t => t != null))
            {
                DeezerCache.AddTrack(track.Id, track); // Guardamos la canción en el cache
                tracks.Add(track);
            }

            // Asignar valores al objeto paginado
            paginatedTracks.Page = page;
            paginatedTracks.TotalPages = result.totalPages;
            paginatedTracks.Offset = offset;
            paginatedTracks.Last = offset + tracks.Count - 1;
            paginatedTracks.Limit = limit;
            paginatedTracks.Tracks = tracks;

            return paginatedTracks;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene las canciones que no le han gustado
        /// y las devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="page">Número de página</param>
        /// <param name="limit">Número de canciones por página</param>
        /// <returns>Objeto con información y lista de canciones</returns>
        public static async Task<PaginatedTracks> getDisLikedTracksDAL(String uid, int page, int limit, String baseUrl)
        {
            PaginatedTracks paginatedTracks = new PaginatedTracks();
            List<Track> tracks = new List<Track>();

            // Primero obtenemos los ids de las canciones de la base de datos
            var result = getListIds("EXEC GetDisLikedTracks @UID, @page, @limit", uid, page, limit);

            // Calcular total de canciones
            int offset = (page - 1) * limit + 1;
            if (page > 1)
            {
                paginatedTracks.LinkPreviousPage = $"{baseUrl}?page={page - 1}&limit={limit}";
            }

            if (page < result.totalPages)
            {
                paginatedTracks.LinkNextPage = $"{baseUrl}?page={page + 1}&limit={limit}";
            }

            // Crear una lista de tareas para manejar las peticiones de forma concurrente
            List<Task<Track>> trackTasks = new List<Task<Track>>();

            // Se añaden las tareas a la lista
            foreach (long trackId in result.list)
            {
                // Verificamos si la canción ya está en el cache
                if (DeezerCache.TryGetTrack(trackId, out Track cachedTrack))
                {
                    tracks.Add(cachedTrack); // Si está en el cache, lo agregamos directamente
                }
                else
                {
                    trackTasks.Add(CallApiDeezer.HandleRateLimitAndGetTrack(trackId)); // Si no está, la solicitamos
                }
            }

            // Esperamos que todas las tareas se completen
            var trackResults = await Task.WhenAll(trackTasks);

            // Agregamos los resultados a la lista final y los almacenamos en el cache
            foreach (var track in trackResults.Where(t => t != null))
            {
                DeezerCache.AddTrack(track.Id, track); // Guardamos la canción en el cache
                tracks.Add(track);
            }

            // Asignar valores al objeto paginado
            paginatedTracks.Page = page;
            paginatedTracks.TotalPages = result.totalPages;
            paginatedTracks.Offset = offset;
            paginatedTracks.Last = offset + tracks.Count - 1;
            paginatedTracks.Limit = limit;
            paginatedTracks.Tracks = tracks;

            return paginatedTracks;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, busca sus amigos y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getFriendsDAL(String uid)
        {
            List<Usuario> usuarios = new List<Usuario>();
            Usuario user = null;
            String uidUser = "";
            String name = "";
            String lastName = "";
            String email = "";
            String photoUrl = "";
            String dateJoining = "";
            String username = "";
            String supplier = "";
            bool blocked = false;
            bool deleted = false;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.CommandText = "EXEC GetUserFriends @UID";
                miComando.Parameters.AddWithValue("@UID", uid);
                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        uidUser = (String)miLector["UID"];
                        name = (String)miLector["Name"];
                        lastName = (String)miLector["LastName"];
                        email = (String)miLector["Email"];
                        photoUrl = (String)miLector["PhotoUrl"];
                        dateJoining = ((DateTime)miLector["DateJoining"]).ToString();
                        username = (String)miLector["Username"];
                        supplier = (String)miLector["Supplier"];
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uidUser, name, lastName, email, photoUrl, dateJoining, username, supplier, deleted, blocked);
                        usuarios.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return usuarios;
        }
        
        /// <summary>
        /// Esta función recibe el uid de un usuario, busca las solicitudes de amistad que le han llegado y las devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getIncomingFriendRequestsDAL(String uid)
        {
            List<Usuario> usuarios = new List<Usuario>();
            Usuario user = null;
            String uidUser = "";
            String name = "";
            String lastName = "";
            String email = "";
            String photoUrl = "";
            String dateJoining = "";
            String username = "";
            String supplier = "";
            bool blocked = false;
            bool deleted = false;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.CommandText = "EXEC GetIncomingFriendRequests @UID";
                miComando.Parameters.AddWithValue("@UID", uid);
                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        uidUser = (String)miLector["UID"];
                        name = (String)miLector["Name"];
                        lastName = (String)miLector["LastName"];
                        email = (String)miLector["Email"];
                        photoUrl = (String)miLector["PhotoUrl"];
                        dateJoining = ((DateTime)miLector["DateJoining"]).ToString();
                        username = (String)miLector["Username"];
                        supplier = (String)miLector["Supplier"];
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uidUser, name, lastName, email, photoUrl, dateJoining, username, supplier, deleted, blocked);
                        usuarios.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, busca las solicitudes de amistad que ha enviado y las devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getOutgoingFriendRequestsDAL(String uid)
        {
            List<Usuario> usuarios = new List<Usuario>();
            Usuario user = null;
            String uidUser = "";
            String name = "";
            String lastName = "";
            String email = "";
            String photoUrl = "";
            String dateJoining = "";
            String username = "";
            String supplier = "";
            bool blocked = false;
            bool deleted = false;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.CommandText = "EXEC GetOutgoingFriendRequests @UID";
                miComando.Parameters.AddWithValue("@UID", uid);
                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        uidUser = (String)miLector["UID"];
                        name = (String)miLector["Name"];
                        lastName = (String)miLector["LastName"];
                        email = (String)miLector["Email"];
                        photoUrl = (String)miLector["PhotoUrl"];
                        dateJoining = ((DateTime)miLector["DateJoining"]).ToString();
                        username = (String)miLector["Username"];
                        supplier = (String)miLector["Supplier"];
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uidUser, name, lastName, email, photoUrl, dateJoining, username, supplier, deleted, blocked);
                        usuarios.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y la id de una canción y busca los amigos del usuario que le han gustado la canción
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="trackId">ID de la canción</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getFriendsWhoLikedTrackDAL(String uid, long trackId)
        {
            List<Usuario> usuarios = new List<Usuario>();
            Usuario user = null;
            String uidUser = "";
            String name = "";
            String lastName = "";
            String email = "";
            String photoUrl = "";
            String dateJoining = "";
            String username = "";
            String supplier = "";
            bool blocked = false;
            bool deleted = false;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.CommandText = "EXEC GetFriendsWhoLikedTrack @UID, @IDTrack";
                miComando.Parameters.AddWithValue("@UID", uid);
                miComando.Parameters.AddWithValue("@IDTrack", trackId);
                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        uidUser = (String)miLector["UID"];
                        name = (String)miLector["Name"];
                        lastName = (String)miLector["LastName"];
                        email = (String)miLector["Email"];
                        photoUrl = (String)miLector["PhotoUrl"];
                        dateJoining = ((DateTime)miLector["DateJoining"]).ToString();
                        username = (String)miLector["Username"];
                        supplier = (String)miLector["Supplier"];
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uidUser, name, lastName, email, photoUrl, dateJoining, username, supplier, deleted, blocked);
                        usuarios.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario y obtiene de la base de datos los usuarios que este ha bloqueado
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getUsersBlockedDAL(String uid)
        {
            List<Usuario> usuarios = new List<Usuario>();
            Usuario user = null;
            String uidUser = "";
            String name = "";
            String lastName = "";
            String email = "";
            String photoUrl = "";
            String dateJoining = "";
            String username = "";
            String supplier = "";
            bool blocked = false;
            bool deleted = false;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.CommandText = "EXEC GetBlockedUsers @UID";
                miComando.Parameters.AddWithValue("@UID", uid);
                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        uidUser = (String)miLector["UID"];
                        name = (String)miLector["Name"];
                        lastName = (String)miLector["LastName"];
                        email = (String)miLector["Email"];
                        photoUrl = (String)miLector["PhotoUrl"];
                        dateJoining = ((DateTime)miLector["DateJoining"]).ToString();
                        username = (String)miLector["Username"];
                        supplier = (String)miLector["Supplier"];
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uidUser, name, lastName, email, photoUrl, dateJoining, username, supplier, deleted, blocked);
                        usuarios.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función obtiene los ids de las canciones/generos/artistas de la base de datos
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="procedure">Procedure a ejecutar</param>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de ids de canciones/generos/artistas</returns>
        private static (int totalPages, List<long> list) getListIds(String procedure, String uid, int page, int limit)
        {
            // Obtenemos de la DB los ids de las canciones/generos/artistas que le han gustado
            List<long> ids = new List<long>();
            int totalPages = 0;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.CommandText = procedure;
                miComando.Parameters.AddWithValue("@UID", uid);
                miComando.Parameters.AddWithValue("@page", page);
                miComando.Parameters.AddWithValue("@limit", limit);
                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    if (miLector.Read())
                    {
                        totalPages = miLector.GetInt32(0);
                    }

                    if (miLector.NextResult())
                    {
                        while (miLector.Read())
                        {
                            long idTrack = (long)miLector["ID"];

                            ids.Add(idTrack);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return (totalPages, ids);
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene los artistas que sigue
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="page">Número de página</param>
        /// <param name="limit">Número de artistas por página</param>
        /// <returns>Objeto con información y lista de artistas</returns>
        public static async Task<PaginatedArtists> getFavArtistsDAL(String uid, int page, int limit, String baseUrl)
        {
            PaginatedArtists paginatedArtists = new PaginatedArtists();
            List<Artist> artists = new List<Artist>();

            // Primero obtenemos los ids de los artistas de la base de datos
            var result = getListIds("EXEC GetFavoriteArtists @UID, @page, @limit", uid, page, limit);

            // Calcular total de artistas
            int offset = (page - 1) * limit + 1;

            if (page > 1)
            {
                paginatedArtists.LinkPreviousPage = $"{baseUrl}?page={page - 1}&limit={limit}";
            }

            if (page < result.totalPages)
            {
                paginatedArtists.LinkNextPage = $"{baseUrl}?page={page + 1}&limit={limit}";
            }

            // Crear una lista de tareas para manejar las peticiones de forma concurrente
            List<Task<Artist>> artistTasks = new List<Task<Artist>>();

            // Se añaden las tareas a la lista
            foreach (long artistId in result.list)
            {
                // Verificamos si el artista ya está en el cache
                if (DeezerCache.TryGetArtist(artistId, out Artist cachedArtist))
                {
                    artists.Add(cachedArtist); // Si está en el cache, lo agregamos directamente
                }
                else
                {
                    artistTasks.Add(CallApiDeezer.HandleRateLimitAndGetArtist(artistId)); // Si no está, la solicitamos
                }
            }

            // Esperamos que todas las tareas se completen
            var artistResults = await Task.WhenAll(artistTasks);

            // Agregamos los resultados a la lista final y los almacenamos en el cache
            foreach (var artist in artistResults.Where(t => t != null))
            {
                DeezerCache.AddArtist(artist.Id, artist); // Guardamos el artista en el cache
                artists.Add(artist);
            }

            // Asignar valores al objeto paginado
            paginatedArtists.Page = page;
            paginatedArtists.TotalPages = result.totalPages;
            paginatedArtists.Offset = offset;
            paginatedArtists.Last = offset + artists.Count - 1;
            paginatedArtists.Limit = limit;
            paginatedArtists.Artists = artists;

            return paginatedArtists;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene los géneros que ha marcado como favorito
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="page">Número de página</param>
        /// <param name="limit">Número de artistas por página</param>
        /// <returns>Objeto con información y lista de géneros que ha marcado como favorito</returns>
        public static async Task<PaginatedGenres> getFavGenresDAL(String uid, int page, int limit, String baseUrl)
        {
            PaginatedGenres paginatedGenres = new PaginatedGenres();
            List<Genre> genres = new List<Genre>();

            // Primero obtenemos los ids de los géneros de la base de datos
            var result = getListIds("EXEC GetFavoriteGenres @UID, @page, @limit", uid, page, limit);

            // Calcular total de géneros
            int offset = (page - 1) * limit + 1;

            if (page > 1)
            {
                paginatedGenres.LinkPreviousPage = $"{baseUrl}?page={page - 1}&limit={limit}";
            }

            if (page < result.totalPages)
            {
                paginatedGenres.LinkNextPage = $"{baseUrl}?page={page + 1}&limit={limit}";
            }

            // Crear una lista de tareas para manejar las peticiones de forma concurrente
            List<Task<Genre>> genreTasks = new List<Task<Genre>>();

            // Se añaden las tareas a la lista
            foreach (long genreId in result.list)
            {
                // Verificamos si el género ya está en el cache
                if (DeezerCache.TryGetGenre(genreId, out Genre cachedGenre))
                {
                    genres.Add(cachedGenre); // Si está en el cache, lo agregamos directamente
                }
                else
                {
                    genreTasks.Add(CallApiDeezer.HandleRateLimitAndGetGenre(genreId)); // Si no está, la solicitamos
                }
            }

            // Esperamos que todas las tareas se completen
            var genreResults = await Task.WhenAll(genreTasks);

            // Agregamos los resultados a la lista final y los almacenamos en el cache
            foreach (var genre in genreResults.Where(t => t != null))
            {
                DeezerCache.AddGenre(genre.Id, genre); // Guardamos el género en el cache
                genres.Add(genre);
            }

            // Asignar valores al objeto paginado
            paginatedGenres.Page = page;
            paginatedGenres.TotalPages = result.totalPages;
            paginatedGenres.Offset = offset;
            paginatedGenres.Last = offset + genres.Count - 1;
            paginatedGenres.Limit = limit;
            paginatedGenres.Genres = genres;

            return paginatedGenres;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y devuelve una lista con los ids de los artistas que sigue al usuario
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de ids de los artistas</returns>
        public static List<long> getUserFavoriteArtistsIdsDAL(String uid)
        {
            List<long> artistsIds = new List<long>();
            
            long id = 0;
            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@UID", System.Data.SqlDbType.VarChar).Value = uid;
                miComando.CommandText = "SELECT * FROM USERARTISTS WHERE UID = @UID";

                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                       id = (long)miLector["IDArtist"];

                        if (id > 0)
                        {
                            artistsIds.Add(id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return artistsIds;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y devuelve una lista con los ids de los géneros que sigue el usuario
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de ids de los artistas</returns>
        public static List<long> getUserFavoriteGenresIdsDAL(String uid)
        {
            List<long> genresIds = new List<long>();

            long id = 0;
            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@UID", System.Data.SqlDbType.VarChar).Value = uid;
                miComando.CommandText = "SELECT * FROM USERGENRES WHERE UID = @UID";

                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        id = (long)miLector["IDGenre"];

                        if (id > 0)
                        {
                            genresIds.Add(id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return genresIds;
        }
    }
}