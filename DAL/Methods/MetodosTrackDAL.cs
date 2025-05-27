using DAL.Utils;
using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Methods
{
    public class MetodosTrackDAL
    {
        /// <summary>
        /// Esta función recibe el id de una canción y devuelve sus datos
        /// </summary>
        /// <param name="id">Id de la canción</param>
        /// <returns>Datos de la canción</returns>
        public static async Task<Track> getTrackById(int id)
        {
            Track track = null;

            // Verificamos si la canción ya está en el cache
            if (DeezerCache.TryGetTrack(id, out Track cachedTrack))
            {
                track = cachedTrack;
            }
            else
            {
                track = await CallApiDeezer.HandleRateLimitAndGetTrack(id);
            }

            return track;
        }

        /// <summary>
        /// Esta función recibe el id de una canción y devuelve el número de likes de esta
        /// </summary>
        /// <param name="idTrack">ID de la canción</param>
        /// <returns>Número de likes</returns>
        public static int getLikesByTrack(long idTrack)
        {
            int likes = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetLikesTrack @IDTrack";
                    cmd.Parameters.AddWithValue("@IDTrack", idTrack);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            likes = (int)reader["LIKES"];
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return likes;
        }

        /// <summary>
        /// Esta función recibe el id de una canción y devuelve el número de dislikes de esta
        /// </summary>
        /// <param name="idTrack">ID de la canción</param>
        /// <returns>Número de dislikes</returns>
        public static int getDislikesByTrack(long idTrack)
        {
            int dislikes = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetDislikesTrack @IDTrack";
                    cmd.Parameters.AddWithValue("@IDTrack", idTrack);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dislikes = (int)reader["DISLIKES"];
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dislikes;
        }

    }
}
