using DAL.Utils;
using DTO;
using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Lists
{
    public class ListadosUserArtistDAL
    {
        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene los artistas que sigue
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="page">Número de página</param>
        /// <param name="limit">Número de artistas por página</param>
        /// <returns>Objeto con información y lista de artistas</returns>
        public static async Task<PaginatedArtists> getFavArtistsDAL(string uid, int page, int limit, string baseUrl)
        {
            PaginatedArtists paginatedArtists = new PaginatedArtists();
            List<Artist> artists = new List<Artist>();

            // Primero obtenemos los ids de los artistas de la base de datos
            var result = Utils.Methods.getListIdsDAL("EXEC GetFavoriteArtists @UID, @page, @limit", uid, page, limit);

            // Calcular total de artistas
            int offset = (page - 1) * limit + 1;

            if (page > 1)
            {
                paginatedArtists.LinkPreviousPage = $"{baseUrl}?page={page - 1}&limit={limit}";
            }

            if (page < result.result.totalPages)
            {
                paginatedArtists.LinkNextPage = $"{baseUrl}?page={page + 1}&limit={limit}";
            }

            // Crear una lista de tareas para manejar las peticiones de forma concurrente
            List<Task<Artist>> artistTasks = new List<Task<Artist>>();

            // Se añaden las tareas a la lista
            foreach (long artistId in result.result.list)
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
                DeezerCache.AddArtist(artist.id, artist); // Guardamos el artista en el cache
                artists.Add(artist);
            }

            // Asignar valores al objeto paginado
            paginatedArtists.Page = page;
            paginatedArtists.TotalPages = result.result.totalPages;
            paginatedArtists.TotalArtists = result.total;
            paginatedArtists.Offset = offset;
            paginatedArtists.Last = offset + artists.Count - 1;
            paginatedArtists.Limit = limit;
            paginatedArtists.Artists = artists;

            return paginatedArtists;
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
        /// Esta función recibe el uid de un usuario, obtiene el top 10 de artistas con más me gusta del usuario
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Objeto con información y lista de artistas</returns>
        public static async Task<List<Artist>> getTop10LikedArtistsByUserDAL(string uid)
        {
            List<(long idArtist, int likes)> artistsDetails = new List<(long, int)>();
            List<Artist> artists = new List<Artist>();

            // Este mapa almacena los likes del artista para después poder ponerlo cuando el artista no está en el caché
            Dictionary<long, int> likesMap = new Dictionary<long, int>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC Get10MostLikedArtistsByUser @uid";
                    cmd.Parameters.AddWithValue("@uid", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long idArtist = (long)reader["IDArtist"];
                            int likes = (int)reader["Likes"];

                            artistsDetails.Add((idArtist, likes));
                            likesMap[idArtist] = likes;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            List<Task<Artist>> artistsTasks = new List<Task<Artist>>();
            Dictionary<long, Task<Artist>> pendingTasks = new Dictionary<long, Task<Artist>>();

            foreach ((long idArtist, int likes) in artistsDetails)
            {
                if (DeezerCache.TryGetArtist(idArtist, out Artist cachedArtist))
                {
                    cachedArtist.likes = likes;
                    artists.Add(cachedArtist);
                }
                else
                {
                    var task = CallApiDeezer.HandleRateLimitAndGetArtist(idArtist);
                    pendingTasks[idArtist] = task;
                    artistsTasks.Add(task);
                }
            }

            var artistsResults = await Task.WhenAll(artistsTasks);

            foreach (var kvp in pendingTasks)
            {
                var id = kvp.Key;
                var task = kvp.Value;
                if (task.Result != null)
                {
                    task.Result.likes = likesMap[id];
                    DeezerCache.AddArtist(id, task.Result);
                    artists.Add(task.Result);
                }
            }

            return artists;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene el top 10 de artistas con más no me gusta del usuario
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Objeto con información y lista de artistas</returns>
        public static async Task<List<Artist>> getTop10DislikedArtistsByUserDAL(string uid)
        {
            List<(long idArtist, int likes)> artistsDetails = new List<(long, int)>();
            List<Artist> artists = new List<Artist>();

            // Este mapa almacena los likes del artista para después poder ponerlo cuando el artista no está en el caché
            Dictionary<long, int> likesMap = new Dictionary<long, int>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC Get10MostDislikedArtistsByUser @uid";
                    cmd.Parameters.AddWithValue("@uid", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long idArtist = (long)reader["IDArtist"];
                            int likes = (int)reader["Likes"];

                            artistsDetails.Add((idArtist, likes));
                            likesMap[idArtist] = likes;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            List<Task<Artist>> artistsTasks = new List<Task<Artist>>();
            Dictionary<long, Task<Artist>> pendingTasks = new Dictionary<long, Task<Artist>>();

            foreach ((long idArtist, int likes) in artistsDetails)
            {
                if (DeezerCache.TryGetArtist(idArtist, out Artist cachedArtist))
                {
                    cachedArtist.likes = likes;
                    artists.Add(cachedArtist);
                }
                else
                {
                    var task = CallApiDeezer.HandleRateLimitAndGetArtist(idArtist);
                    pendingTasks[idArtist] = task;
                    artistsTasks.Add(task);
                }
            }

            var artistsResults = await Task.WhenAll(artistsTasks);

            foreach (var kvp in pendingTasks)
            {
                var id = kvp.Key;
                var task = kvp.Value;
                if (task.Result != null)
                {
                    task.Result.likes = likesMap[id];
                    DeezerCache.AddArtist(id, task.Result);
                    artists.Add(task.Result);
                }
            }

            return artists;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene el top 10 de artistas con más swipes del usuario
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Objeto con información y lista de artistas</returns>
        public static async Task<List<Artist>> getTop10SwipesArtistsByUserDAL(string uid)
        {
            List<(long idArtist, int likes)> artistsDetails = new List<(long, int)>();
            List<Artist> artists = new List<Artist>();

            // Este mapa almacena los likes del artista para después poder ponerlo cuando el artista no está en el caché
            Dictionary<long, int> likesMap = new Dictionary<long, int>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC Get10MostSwipeArtistsByUser @uid";
                    cmd.Parameters.AddWithValue("@uid", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long idArtist = (long)reader["IDArtist"];
                            int likes = (int)reader["Swipes"];

                            artistsDetails.Add((idArtist, likes));
                            likesMap[idArtist] = likes;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            List<Task<Artist>> artistsTasks = new List<Task<Artist>>();
            Dictionary<long, Task<Artist>> pendingTasks = new Dictionary<long, Task<Artist>>();

            foreach ((long idArtist, int likes) in artistsDetails)
            {
                if (DeezerCache.TryGetArtist(idArtist, out Artist cachedArtist))
                {
                    cachedArtist.likes = likes;
                    artists.Add(cachedArtist);
                }
                else
                {
                    var task = CallApiDeezer.HandleRateLimitAndGetArtist(idArtist);
                    pendingTasks[idArtist] = task;
                    artistsTasks.Add(task);
                }
            }

            var artistsResults = await Task.WhenAll(artistsTasks);

            foreach (var kvp in pendingTasks)
            {
                var id = kvp.Key;
                var task = kvp.Value;
                if (task.Result != null)
                {
                    task.Result.likes = likesMap[id];
                    DeezerCache.AddArtist(id, task.Result);
                    artists.Add(task.Result);
                }
            }

            return artists;
        }

    }
}
