using DAL.Utils;
using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Lists
{
    public class ListadosArtistsDAL
    {

        /// <summary>
        /// Esta función obtiene el top 10 de artistas con más me gusta globales
        /// y los devuelve como una lista
        /// </summary>
        /// <returns>Objeto con información y lista de artistas</returns>
        public static async Task<List<Artist>> getTop10LikedArtistsGlobalDAL()
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
                    cmd.CommandText = "EXEC Get10MostLikedArtistsGlobal";

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
        /// Esta función obtiene el top 10 de artistas con más no me gusta globales
        /// y los devuelve como una lista
        /// </summary>
        /// <returns>Objeto con información y lista de artistas</returns>
        public static async Task<List<Artist>> getTop10DislikedArtistsGlobalDAL()
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
                    cmd.CommandText = "EXEC Get10MostDislikedArtistsGlobal";

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
        /// Esta función obtiene el top 10 de artistas con más swipes globales
        /// y los devuelve como una lista
        /// </summary>
        /// <returns>Objeto con información y lista de artistas</returns>
        public static async Task<List<Artist>> getTop10SwipesArtistsGlobalDAL()
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
                    cmd.CommandText = "EXEC Get10MostSwipeArtistsGlobal";

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
