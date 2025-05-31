using DAL.Utils;
using DAL;
using DTO;
using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Lists
{
    public class ListadosUserTrackDAL
    {
        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene las canciones que le han gustado
        /// y las devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="page">Número de página</param>
        /// <param name="limit">Número de canciones por página</param>
        /// <returns>Objeto con información y lista de canciones</returns>
        public static async Task<PaginatedTracks> getLikedTracksDAL(string uid, int page, int limit, string baseUrl)
        {
            PaginatedTracks paginatedTracks = new PaginatedTracks();
            List<Track> tracks = new List<Track>();

            // Primero obtenemos los ids de las canciones de la base de datos
            var result = Utils.Methods.getListIdsDAL("EXEC GetLikedTracks @UID, @page, @limit", uid, page, limit);

            // Calcular total de canciones
            int offset = (page - 1) * limit + 1;

            if (page > 1)
            {
                paginatedTracks.LinkPreviousPage = $"{baseUrl}?page={page - 1}&limit={limit}";
            }

            if (page < result.result.totalPages)
            {
                paginatedTracks.LinkNextPage = $"{baseUrl}?page={page + 1}&limit={limit}";
            }

            // Crear una lista de tareas para manejar las peticiones de forma concurrente
            List<Task<Track>> trackTasks = new List<Task<Track>>();

            // Se añaden las tareas a la lista
            foreach (long trackId in result.result.list)
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
                DeezerCache.AddTrack(track.id, track); // Guardamos la canción en el cache
                tracks.Add(track);
            }

            // Asignar valores al objeto paginado
            paginatedTracks.Page = page;
            paginatedTracks.TotalPages = result.result.totalPages;
            paginatedTracks.TotalTracks = result.total;
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
        public static async Task<PaginatedTracks> getDisLikedTracksDAL(string uid, int page, int limit, string baseUrl)
        {
            PaginatedTracks paginatedTracks = new PaginatedTracks();
            List<Track> tracks = new List<Track>();

            // Primero obtenemos los ids de las canciones de la base de datos
            var result = Utils.Methods.getListIdsDAL("EXEC GetDisLikedTracks @UID, @page, @limit", uid, page, limit);

            // Calcular total de canciones
            int offset = (page - 1) * limit + 1;
            if (page > 1)
            {
                paginatedTracks.LinkPreviousPage = $"{baseUrl}?page={page - 1}&limit={limit}";
            }

            if (page < result.result.totalPages)
            {
                paginatedTracks.LinkNextPage = $"{baseUrl}?page={page + 1}&limit={limit}";
            }

            // Crear una lista de tareas para manejar las peticiones de forma concurrente
            List<Task<Track>> trackTasks = new List<Task<Track>>();

            // Se añaden las tareas a la lista
            foreach (long trackId in result.result.list)
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
                DeezerCache.AddTrack(track.id, track); // Guardamos la canción en el cache
                tracks.Add(track);
            }

            // Asignar valores al objeto paginado
            paginatedTracks.Page = page;
            paginatedTracks.TotalPages = result.result.totalPages;
            paginatedTracks.TotalTracks = result.total;
            paginatedTracks.Offset = offset;
            paginatedTracks.Last = offset + tracks.Count - 1;
            paginatedTracks.Limit = limit;
            paginatedTracks.Tracks = tracks;

            return paginatedTracks;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene los últimos 5 swipes
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Objeto con información y lista de canciones</returns>
        public static async Task<List<Track>> getLast5SwipesDAL(string uid)
        {
            List<(long idTrack, bool swipe)> lastSwipes = new List<(long, bool)>();
            List<Track> tracks = new List<Track>();

            // Este mapa almacena el like de las canciones para después poder ponerlo cuando la canción no está en el caché
            Dictionary<long, bool> swipeMap = new Dictionary<long, bool>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT TOP 5 IDTrack, Swipe FROM USERSWIPES WHERE UID = @uid ORDER BY DateSwipe DESC";
                    cmd.Parameters.AddWithValue("@uid", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = (long)reader["IDTrack"];
                            bool swipe = (byte)reader["Swipe"] == 1;

                            lastSwipes.Add((id, swipe));
                            swipeMap[id] = swipe;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            List<Task<Track>> trackTasks = new List<Task<Track>>();
            Dictionary<long, Task<Track>> pendingTasks = new Dictionary<long, Task<Track>>();

            foreach ((long idTrack, bool swipe) in lastSwipes)
            {
                if (DeezerCache.TryGetTrack(idTrack, out Track cachedTrack))
                {
                    cachedTrack.like = swipe;
                    tracks.Add(cachedTrack);
                }
                else
                {
                    var task = CallApiDeezer.HandleRateLimitAndGetTrack(idTrack);
                    pendingTasks[idTrack] = task;
                    trackTasks.Add(task);
                }
            }

            var trackResults = await Task.WhenAll(trackTasks);

            foreach (var kvp in pendingTasks)
            {
                var id = kvp.Key;
                var task = kvp.Value;
                if (task.Result != null)
                {
                    task.Result.like = swipeMap[id];
                    DeezerCache.AddTrack(id, task.Result);
                    tracks.Add(task.Result);
                }
            }

            return tracks;
        }

    }
}
