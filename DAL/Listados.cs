using Entidades;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Windows.Input;

namespace DAL
{
    public class Listados
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string DeezerApiUrl = "https://api.deezer.com/";
        private const int MaxRequestsPerWindow = 50;
        private static DateTime _lastRequestTime = DateTime.MinValue;
        private static int _requestCount = 0;

        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene las canciones que le han gustado
        /// y las devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de canciones</returns>
        public static async Task<List<Track>> getLikedTracks(String uid)
        {
            List<Track> tracks = new List<Track>();

            // Primero obtenemos los ids de las canciones de la base de datos
            List<long> tracksId = getTracksIds("EXEC GetLikedTracks @UID", uid);

            // Crear una lista de tareas para manejar las peticiones de forma concurrente
            List<Task<Track>> trackTasks = new List<Task<Track>>();

            // Se añaden las tareas a la lista
            foreach (long trackId in tracksId)
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

            return tracks;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene las canciones que no le han gustado
        /// y las devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de canciones</returns>
        public static async Task<List<Track>> getDisLikedTracks(String uid)
        {
            List<Track> tracks = new List<Track>();

            // Primero obtenemos los ids de las canciones de la base de datos
            List<long> tracksId = getTracksIds("EXEC GetDislikedTracks @UID", uid);

            // Crear una lista de tareas para manejar las peticiones de forma concurrente
            List<Task<Track>> trackTasks = new List<Task<Track>>();

            // Se añaden las tareas a la lista
            foreach (long trackId in tracksId)
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

            return tracks;
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
        private static List<long> getTracksIds(String procedure, String uid)
        {
            // Obtenemos de la DB los ids de las canciones que le han gustado
            List<long> tracksId = new List<long>();
            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.CommandText = procedure;
                miComando.Parameters.AddWithValue("@UID", uid);
                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        long idTrack = (long)miLector["IDTrack"];

                        tracksId.Add(idTrack);
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

            return tracksId;
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