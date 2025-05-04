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
            Album album = null;

            // Verificamos si el album ya está en el cache
            if (DeezerCache.TryGetAlbum(id, out Album cachedAlbum))
            {
                album = cachedAlbum;
            }
            else
            {
                album = await CallApiDeezer.HandleRateLimitAndGetAlbum(id);
            }

            return album;
        }

        /// <summary>
        /// Esta función recibe el id de un álbum y devuelve el número de likes de este
        /// </summary>
        /// <param name="idAlbum">ID del album</param>
        /// <returns>Número de likes</returns>
        public static int getLikesByAlbum(long idAlbum)
        {
            int likes = 0;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.CommandText = "EXEC GetLikesAlbum @IDAlbum";
                miComando.Parameters.AddWithValue("@IDAlbum", idAlbum);
                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        likes = (int)miLector["LIKES"];
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

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.CommandText = "EXEC GetDislikesAlbum @IDAlbum";
                miComando.Parameters.AddWithValue("@IDAlbum", idAlbum);
                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        dislikes = (int)miLector["DISLIKES"];
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

            return dislikes;
        }
    }
}
