using Azure;
using DAL.Utils;
using DTO;
using Entidades;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
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

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM USERS WHERE UserDeleted = 0 AND UserBlocked = 0";

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string uid = (string)reader["UID"];
                            string name = (string)reader["Name"];
                            string lastName = (string)reader["LastName"];
                            string email = (string)reader["Email"];
                            string photoUrl = (string)reader["PhotoUrl"];
                            string dateJoining = ((DateTime)reader["DateJoining"]).ToString();
                            string username = (string)reader["Username"];
                            string supplier = (string)reader["Supplier"];
                            bool deleted = (bool)reader["UserDeleted"];
                            bool blocked = (bool)reader["UserBlocked"];

                            usuarios.Add(new Usuario(uid, name, lastName, email, photoUrl, dateJoining, username, supplier, deleted, blocked));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe un username y busca en la base de datos todos los usuarios que su username concuerde con el recibido
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="username">Username a buscar</param>
        /// <returns>Lista</returns>
        public static List<Usuario> getUsersByUsername(string username)
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM USERS WHERE Username LIKE @username AND UserDeleted = 0 AND UserBlocked = 0";
                    cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = "%" + username + "%";

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string uid = (string)reader["UID"];
                            string name = (string)reader["Name"];
                            string lastName = (string)reader["LastName"];
                            string email = (string)reader["Email"];
                            string photoUrl = (string)reader["PhotoUrl"];
                            string dateJoining = ((DateTime)reader["DateJoining"]).ToString();
                            string usernameUser = (string)reader["Username"];
                            string supplier = (string)reader["Supplier"];
                            bool deleted = (bool)reader["UserDeleted"];
                            bool blocked = (bool)reader["UserBlocked"];

                            usuarios.Add(new Usuario(uid, name, lastName, email, photoUrl, dateJoining, usernameUser, supplier, deleted, blocked));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
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
        public static List<Usuario> getFriendsDAL(string uid)
        {
            var usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetUserFriends @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, busca las solicitudes de amistad que le han llegado y las devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getIncomingFriendRequestsDAL(string uid)
        {
            var usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetIncomingFriendRequests @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, busca las solicitudes de amistad que ha enviado y las devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getOutgoingFriendRequestsDAL(string uid)
        {
            var usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetOutgoingFriendRequests @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
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
        public static List<Usuario> getFriendsWhoLikedTrackDAL(string uid, long trackId)
        {
            List<Usuario> usuarios = new();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetFriendsWhoLikedTrack @UID, @IDTrack";
                    cmd.Parameters.AddWithValue("@UID", uid);
                    cmd.Parameters.AddWithValue("@IDTrack", trackId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario y obtiene de la base de datos los usuarios que este ha bloqueado
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getUsersBlockedDAL(string uid)
        {
            List<Usuario> usuarios = new();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetBlockedUsers @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
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
        private static (int totalPages, List<long> list) getListIds(string procedure, string uid, int page, int limit)
        {
            List<long> ids = new();
            int totalPages = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = procedure;
                    cmd.Parameters.AddWithValue("@UID", uid);
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@limit", limit);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalPages = reader.GetInt32(0);
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                ids.Add((long)reader["ID"]);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
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
        public static List<long> getUserFavoriteArtistsIdsDAL(string uid)
        {
            List<long> artistsIds = new();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT IDArtist FROM USERARTISTS WHERE UID = @UID";
                    cmd.Parameters.Add("@UID", SqlDbType.VarChar).Value = uid;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = (long)reader["IDArtist"];
                            if (id > 0)
                            {
                                artistsIds.Add(id);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return artistsIds;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y devuelve una lista con los ids de los géneros que sigue el usuario
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de ids de los artistas</returns>
        public static List<long> getUserFavoriteGenresIdsDAL(string uid)
        {
            List<long> genresIds = new();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT IDGenre FROM USERGENRES WHERE UID = @UID";
                    cmd.Parameters.Add("@UID", SqlDbType.VarChar).Value = uid;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = (long)reader["IDGenre"];
                            if (id > 0)
                                genresIds.Add(id);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return genresIds;
        }

        /// <summary>
        /// Esta función recibe un UID y devuelve una lista con los usuarios a los que les ha enviado una
        /// solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getSentRequestDAL(string uid)
        {
            List<Usuario> usuarios = new();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetOutgoingFriendRequests @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }


        /// <summary>
        /// Esta función recibe un UID y devuelve una lista con los usuarios que le han enviado una
        /// solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getReceiveRequestDAL(string uid)
        {
            List<Usuario> usuarios = new();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetIncomingFriendRequests @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

    }
}