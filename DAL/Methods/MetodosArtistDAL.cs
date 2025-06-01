using DAL.Utils;
using DTO;
using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Methods
{
    public class MetodosArtistDAL
    {
        /// <summary>
        /// Esta función recibe el id de un artista y devuelve sus datos
        /// </summary>
        /// <param name="id">Id del artista</param>
        /// <returns>Datos del artista</returns>
        //public static async Task<Artist> getArtistaById(int id)
        //{
        //    if (DeezerCache.TryGetArtist(id, out Artist cachedartista))
        //    {
        //        return cachedartista;
        //    }

        //    return await CallApiDeezer.HandleRateLimitAndGetArtist(id);
        //}


        /// <summary>
        /// Esta función recibe el id de un artista y devuelve el número de likes de este
        /// </summary>
        /// <param name="idArtist">ID del artista</param>
        /// <returns>Número de likes</returns>
        public static int getLikesByArtist(long idArtist)
        {
            int likes = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS LIKES FROM USERSWIPES WHERE IDArtist = @IDArtist AND Swipe = 1", conn))
                {
                    cmd.Parameters.Add("@IDArtist", SqlDbType.BigInt).Value = idArtist;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                likes = (int)reader["LIKES"]; 
                            }
                        }
                    }
                }
            }
            catch (Exception) { throw; }

            return likes;
        }


        /// <summary>
        /// Esta función recibe el id de un artista y devuelve el número de dislikes de este
        /// </summary>
        /// <param name="idartista">ID del artista</param>
        /// <returns>Número de dislikes</returns>
        public static int getDislikesByArtist(long idArtist)
        {
            int likes = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS DISLIKES FROM USERSWIPES WHERE IDArtist = @IDArtist AND Swipe = 0", conn))
                {
                    cmd.Parameters.Add("@IDArtist", SqlDbType.BigInt).Value = idArtist;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                likes = (int)reader["DISLIKES"];
                            }
                        }
                    }
                }
            }
            catch (Exception) { throw; }

            return likes;
        }

        /// <summary>
        /// Esta función recibe el ID de un artista y devuelve sus stats
        /// </summary>
        /// <param name="idArtist">ID del artista</param>
        /// <returns>Stats</returns>
        public static Stats getStatsByArtistDAL(long idArtist)
        {
            Stats stats = new Stats();

            int likes = getLikesByArtist(idArtist);
            int dislikes = getDislikesByArtist(idArtist);

            int swipes = likes + dislikes;

            stats.Likes = likes;
            stats.Dislikes = dislikes;
            stats.Swipes = swipes;

            return stats;
        }
    }
}
