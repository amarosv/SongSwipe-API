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
    public class ListadosAlbumsDAL
    {
        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene el top 10 de albumes con más me gusta global
        /// y los devuelve como una lista
        /// </summary>
        /// <returns>Objeto con información y lista de albumes</returns>
        public static async Task<List<Album>> getTop10LikedAlbumsGlobalDAL()
        {
            List<(long idAlbum, int likes)> albumsDetails = new List<(long, int)>();
            List<Album> albums = new List<Album>();

            // Este mapa almacena los likes del album para después poder ponerlo cuando el artista no está en el caché
            Dictionary<long, int> likesMap = new Dictionary<long, int>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC Get10MostLikedAlbumsGlobal";

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long idAlbum = (long)reader["IDAlbum"];
                            int likes = (int)reader["Likes"];

                            albumsDetails.Add((idAlbum, likes));
                            likesMap[idAlbum] = likes;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            List<Task<Album>> albumsTasks = new List<Task<Album>>();
            Dictionary<long, Task<Album>> pendingTasks = new Dictionary<long, Task<Album>>();

            foreach ((long idAlbum, int likes) in albumsDetails)
            {
                if (DeezerCache.TryGetAlbum(idAlbum, out Album cachedAlbum))
                {
                    cachedAlbum.likes = likes;
                    albums.Add(cachedAlbum);
                }
                else
                {
                    var task = CallApiDeezer.HandleRateLimitAndGetAlbum(idAlbum);
                    pendingTasks[idAlbum] = task;
                    albumsTasks.Add(task);
                }
            }

            var albumsResults = await Task.WhenAll(albumsTasks);

            foreach (var kvp in pendingTasks)
            {
                var id = kvp.Key;
                var task = kvp.Value;
                if (task.Result != null)
                {
                    task.Result.likes = likesMap[id];
                    DeezerCache.AddAlbum(id, task.Result);
                    albums.Add(task.Result);
                }
            }

            return albums;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene el top 10 de albumes con más no me gusta global
        /// y los devuelve como una lista
        /// </summary>
        /// <returns>Objeto con información y lista de albumes</returns>
        public static async Task<List<Album>> getTop10DislikedAlbumsGlobalDAL()
        {
            List<(long idAlbum, int likes)> albumsDetails = new List<(long, int)>();
            List<Album> albums = new List<Album>();

            // Este mapa almacena los likes del album para después poder ponerlo cuando el artista no está en el caché
            Dictionary<long, int> likesMap = new Dictionary<long, int>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC Get10MostDislikedAlbumsGlobal";

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long idAlbum = (long)reader["IDAlbum"];
                            int likes = (int)reader["Likes"];

                            albumsDetails.Add((idAlbum, likes));
                            likesMap[idAlbum] = likes;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            List<Task<Album>> albumsTasks = new List<Task<Album>>();
            Dictionary<long, Task<Album>> pendingTasks = new Dictionary<long, Task<Album>>();

            foreach ((long idAlbum, int likes) in albumsDetails)
            {
                if (DeezerCache.TryGetAlbum(idAlbum, out Album cachedAlbum))
                {
                    cachedAlbum.likes = likes;
                    albums.Add(cachedAlbum);
                }
                else
                {
                    var task = CallApiDeezer.HandleRateLimitAndGetAlbum(idAlbum);
                    pendingTasks[idAlbum] = task;
                    albumsTasks.Add(task);
                }
            }

            var albumsResults = await Task.WhenAll(albumsTasks);

            foreach (var kvp in pendingTasks)
            {
                var id = kvp.Key;
                var task = kvp.Value;
                if (task.Result != null)
                {
                    task.Result.likes = likesMap[id];
                    DeezerCache.AddAlbum(id, task.Result);
                    albums.Add(task.Result);
                }
            }

            return albums;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene el top 10 de albumes con más swipes global
        /// y los devuelve como una lista
        /// </summary>
        /// <returns>Objeto con información y lista de albumes</returns>
        public static async Task<List<Album>> getTop10SwipesAlbumsGlobalDAL()
        {
            List<(long idAlbum, int likes)> albumsDetails = new List<(long, int)>();
            List<Album> albums = new List<Album>();

            // Este mapa almacena los likes del album para después poder ponerlo cuando el artista no está en el caché
            Dictionary<long, int> likesMap = new Dictionary<long, int>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC Get10MostSwipeAlbumsGlobal";

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long idAlbum = (long)reader["IDAlbum"];
                            int likes = (int)reader["Swipes"];

                            albumsDetails.Add((idAlbum, likes));
                            likesMap[idAlbum] = likes;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            List<Task<Album>> albumsTasks = new List<Task<Album>>();
            Dictionary<long, Task<Album>> pendingTasks = new Dictionary<long, Task<Album>>();

            foreach ((long idAlbum, int likes) in albumsDetails)
            {
                if (DeezerCache.TryGetAlbum(idAlbum, out Album cachedAlbum))
                {
                    cachedAlbum.likes = likes;
                    albums.Add(cachedAlbum);
                }
                else
                {
                    var task = CallApiDeezer.HandleRateLimitAndGetAlbum(idAlbum);
                    pendingTasks[idAlbum] = task;
                    albumsTasks.Add(task);
                }
            }

            var albumsResults = await Task.WhenAll(albumsTasks);

            foreach (var kvp in pendingTasks)
            {
                var id = kvp.Key;
                var task = kvp.Value;
                if (task.Result != null)
                {
                    task.Result.likes = likesMap[id];
                    DeezerCache.AddAlbum(id, task.Result);
                    albums.Add(task.Result);
                }
            }

            return albums;
        }
    }
}
