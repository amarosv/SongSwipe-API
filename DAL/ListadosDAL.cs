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
    public class ListadosDAL
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string DeezerApiUrl = "https://api.deezer.com/";
        private const int MaxRequestsPerWindow = 50;
        private static DateTime _lastRequestTime = DateTime.MinValue;
        private static int _requestCount = 0;

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
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uid, name, lastName, email, photoUrl, dateJoining, username, deleted, blocked);
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
            var result = getTracksIds("EXEC GetLikedTracks @UID, @page, @limit", uid, page, limit);

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
            foreach (long trackId in result.tracks)
            {
                // Verificamos si la canción ya está en el cache
                if (TrackCache.TryGetTrack(trackId, out Track cachedTrack))
                {
                    tracks.Add(cachedTrack); // Si está en el cache, lo agregamos directamente
                }
                else
                {
                    trackTasks.Add(HandleRateLimitAndGetTrack(trackId)); // Si no está, la solicitamos
                }
            }

            // Esperamos que todas las tareas se completen
            var trackResults = await Task.WhenAll(trackTasks);

            // Agregamos los resultados a la lista final y los almacenamos en el cache
            foreach (var track in trackResults.Where(t => t != null))
            {
                TrackCache.AddTrack(track.Id, track); // Guardamos la canción en el cache
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
            var result = getTracksIds("EXEC GetDisLikedTracks @UID, @page, @limit", uid, page, limit);

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
            foreach (long trackId in result.tracks)
            {
                // Verificamos si la canción ya está en el cache
                if (TrackCache.TryGetTrack(trackId, out Track cachedTrack))
                {
                    tracks.Add(cachedTrack); // Si está en el cache, lo agregamos directamente
                }
                else
                {
                    trackTasks.Add(HandleRateLimitAndGetTrack(trackId)); // Si no está, la solicitamos
                }
            }

            // Esperamos que todas las tareas se completen
            var trackResults = await Task.WhenAll(trackTasks);

            // Agregamos los resultados a la lista final y los almacenamos en el cache
            foreach (var track in trackResults.Where(t => t != null))
            {
                TrackCache.AddTrack(track.Id, track); // Guardamos la canción en el cache
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
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uidUser, name, lastName, email, photoUrl, dateJoining, username, deleted, blocked);
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
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uidUser, name, lastName, email, photoUrl, dateJoining, username, deleted, blocked);
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
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uidUser, name, lastName, email, photoUrl, dateJoining, username, deleted, blocked);
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
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uidUser, name, lastName, email, photoUrl, dateJoining, username, deleted, blocked);
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
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uidUser, name, lastName, email, photoUrl, dateJoining, username, deleted, blocked);
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

        private static async Task<Track> HandleRateLimitAndGetTrack(long trackId)
        {
            await HandleRateLimit();

            var response = await _httpClient.GetAsync($"{DeezerApiUrl}/track/{trackId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Track trackData = JsonConvert.DeserializeObject<Track>(content);
                return trackData;
            }
            return null;
        }

        /// <summary>
        /// Esta función obtiene los ids de las canciones de la base de datos
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="procedure">Procedure a ejecutar</param>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de ids de canciones</returns>
        private static (int totalPages, List<long> tracks) getTracksIds(String procedure, String uid, int page, int limit)
        {
            // Obtenemos de la DB los ids de las canciones que le han gustado
            List<long> tracksId = new List<long>();
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
                            long idTrack = (long)miLector["IDTrack"];

                            tracksId.Add(idTrack);
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

            return (totalPages, tracksId);
        }

        private static async Task HandleRateLimit()
        {
            if (_requestCount >= MaxRequestsPerWindow)
            {
                TimeSpan elapsed = DateTime.UtcNow - _lastRequestTime;
                if (elapsed.TotalSeconds < 5) // Espera si se superó el límite
                {
                    await Task.Delay(TimeSpan.FromSeconds(5 - elapsed.TotalSeconds));
                }
                _requestCount = 0;
            }

            _lastRequestTime = DateTime.UtcNow;
            _requestCount++;
        }
    }
}