using DAL.Utils;
using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MetodosAlbumDAL
    {
        /// <summary>
        /// Esta función recibe el id de un álbum y devuelve sus datos
        /// </summary>
        /// <param name="id">Id del album</param>
        /// <returns>Datos del album</returns>
        public static async Task<Album> getAlbumById(int id)
        {
            if (DeezerCache.TryGetAlbum(id, out Album cachedAlbum))
            {
                return cachedAlbum;
            }

            return await CallApiDeezer.HandleRateLimitAndGetAlbum(id);
        }


        /// <summary>
        /// Esta función recibe el id de un álbum y devuelve el número de likes de este
        /// </summary>
        /// <param name="idAlbum">ID del album</param>
        /// <returns>Número de likes</returns>
        public static int getLikesByAlbum(long idAlbum)
        {
            int likes = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetLikesAlbum @IDAlbum";
                    cmd.Parameters.AddWithValue("@IDAlbum", idAlbum);

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
        /// Esta función recibe el id de un álbum y devuelve el número de dislikes de este
        /// </summary>
        /// <param name="idAlbum">ID del album</param>
        /// <returns>Número de dislikes</returns>
        public static int getDislikesByAlbum(long idAlbum)
        {
            int dislikes = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetDislikesAlbum @IDAlbum";
                    cmd.Parameters.AddWithValue("@IDAlbum", idAlbum);

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
