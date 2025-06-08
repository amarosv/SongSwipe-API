using Azure;
using DAL.Utils;
using DTO;
using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

            var fetchedArtistsDict = new Dictionary<long, Artist>();

            foreach (var kvp in pendingTasks)
            {
                var id = kvp.Key;
                var task = kvp.Value;
                if (task.Result != null)
                {
                    task.Result.likes = likesMap[id];
                    DeezerCache.AddArtist(id, task.Result);
                    fetchedArtistsDict[id] = task.Result;
                }
            }

            // Reconstruimos la lista en el orden original
            artists = artistsDetails
                .Where(x => DeezerCache.TryGetArtist(x.idArtist, out _) || fetchedArtistsDict.ContainsKey(x.idArtist))
                .Select(x =>
                    DeezerCache.TryGetArtist(x.idArtist, out var cached) ? cached : fetchedArtistsDict[x.idArtist])
                .ToList();

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

            var fetchedArtistsDict = new Dictionary<long, Artist>();

            foreach (var kvp in pendingTasks)
            {
                var id = kvp.Key;
                var task = kvp.Value;
                if (task.Result != null)
                {
                    task.Result.likes = likesMap[id];
                    DeezerCache.AddArtist(id, task.Result);
                    fetchedArtistsDict[id] = task.Result;
                }
            }

            // Reconstruimos la lista en el orden original
            artists = artistsDetails
                .Where(x => DeezerCache.TryGetArtist(x.idArtist, out _) || fetchedArtistsDict.ContainsKey(x.idArtist))
                .Select(x =>
                    DeezerCache.TryGetArtist(x.idArtist, out var cached) ? cached : fetchedArtistsDict[x.idArtist])
                .ToList();

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

            var fetchedArtistsDict = new Dictionary<long, Artist>();

            foreach (var kvp in pendingTasks)
            {
                var id = kvp.Key;
                var task = kvp.Value;
                if (task.Result != null)
                {
                    task.Result.likes = likesMap[id];
                    DeezerCache.AddArtist(id, task.Result);
                    fetchedArtistsDict[id] = task.Result;
                }
            }

            // Reconstruimos la lista en el orden original
            artists = artistsDetails
                .Where(x => DeezerCache.TryGetArtist(x.idArtist, out _) || fetchedArtistsDict.ContainsKey(x.idArtist))
                .Select(x =>
                    DeezerCache.TryGetArtist(x.idArtist, out var cached) ? cached : fetchedArtistsDict[x.idArtist])
                .ToList();

            return artists;
        }

        /// <summary>
        /// Esta función obtiene el top 3 canciones con más me gustas del artista
        /// </summary>
        /// <param name="idArtist">ID del artista</param>
        /// <returns>Lista de canciones</returns>
        public static async Task<List<Track>> getTopTracksDAL(long idArtist)
        {
            List<(long idTrack, int likes)> trackDetails = new List<(long, int)>();
            List<Track> tracks = new List<Track>();

            // Este mapa almacena los likes de la canción para después poder ponerlo cuando la canción no está en el caché
            Dictionary<long, int> likesMap = new Dictionary<long, int>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetTopTracksByArtist @id";
                    cmd.Parameters.AddWithValue("@id", idArtist);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long idTrack = (long)reader["ID"];
                            int likes = (int)reader["LIKES"];

                            trackDetails.Add((idTrack, likes));
                            likesMap[idTrack] = likes;
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

            foreach ((long idTrack, int likes) in trackDetails)
            {
                if (DeezerCache.TryGetTrack(idTrack, out Track cachedTrack))
                {
                    cachedTrack.likes = likes;
                    tracks.Add(cachedTrack);
                }
                else
                {
                    var task = CallApiDeezer.HandleRateLimitAndGetTrack(idTrack);
                    pendingTasks[idTrack] = task;
                    trackTasks.Add(task);
                }
            }

            var fetchedTracksDict = new Dictionary<long, Track>();

            foreach (var kvp in pendingTasks)
            {
                var id = kvp.Key;
                var task = kvp.Value;
                if (task.Result != null)
                {
                    task.Result.likes = likesMap[id];
                    DeezerCache.AddTrack(id, task.Result);
                    fetchedTracksDict[id] = task.Result;
                }
            }

            // Reconstruimos la lista en el orden original
            tracks = trackDetails
                .Where(x => DeezerCache.TryGetTrack(x.idTrack, out _) || fetchedTracksDict.ContainsKey(x.idTrack))
                .Select(x =>
                    DeezerCache.TryGetTrack(x.idTrack, out var cached) ? cached : fetchedTracksDict[x.idTrack])
                .ToList();

            return tracks;
        }

        /// <summary>
        /// Esta función obtiene el top 3 albumes con más me gustas del artista
        /// </summary>
        /// <param name="idArtist">ID del artista</param>
        /// <returns>Lista de albumes</returns>
        public static async Task<List<Album>> getTopAlbumsDAL(long idArtist)
        {
            List<(long idAlbum, int likes)> albumDetails = new List<(long, int)>();
            List<Album> albums = new List<Album>();

            // Este mapa almacena los likes del album para después poder ponerlo cuando el abum no está en el caché
            Dictionary<long, int> likesMap = new Dictionary<long, int>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetTopAlbumsByArtist @id";
                    cmd.Parameters.AddWithValue("@id", idArtist);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long idTrack = (long)reader["ID"];
                            int likes = (int)reader["LIKES"];

                            albumDetails.Add((idTrack, likes));
                            likesMap[idTrack] = likes;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            List<Task<Album>> albumTasks = new List<Task<Album>>();
            Dictionary<long, Task<Album>> pendingTasks = new Dictionary<long, Task<Album>>();

            foreach ((long idAlbum, int likes) in albumDetails)
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
                    albumTasks.Add(task);
                }
            }

            var fetchedAlbumsDict = new Dictionary<long, Album>();

            foreach (var kvp in pendingTasks)
            {
                var id = kvp.Key;
                var task = kvp.Value;
                if (task.Result != null)
                {
                    task.Result.likes = likesMap[id];
                    DeezerCache.AddAlbum(id, task.Result);
                    fetchedAlbumsDict[id] = task.Result;
                }
            }

            // Reconstruimos la lista en el orden original
            albums = albumDetails
                .Where(x => DeezerCache.TryGetAlbum(x.idAlbum, out _) || fetchedAlbumsDict.ContainsKey(x.idAlbum))
                .Select(x =>
                    DeezerCache.TryGetAlbum(x.idAlbum, out var cached) ? cached : fetchedAlbumsDict[x.idAlbum])
                .ToList();

            return albums;
        }
    }
}
