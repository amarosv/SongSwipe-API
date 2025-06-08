using DAL.Utils;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Methods
{
    public class MetodosUserArtistDAL
    {
        /// <summary>
        /// Esta función recibe el uid de un usuario y la lista con los ids de los artistas a guardar como favoritos y los
        /// guarda en la base de datos
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="artists">Lista de ids de los artistas</param>
        /// <returns>Número de filas afectadas</returns>
        public static int addArtistsToFavoritesDAL(string uid, List<long> artists)
        {
            int numFilasAfectadas = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                {
                    conn.Open();

                    foreach (long id in artists)
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO USERARTISTS (UID, IDArtist) VALUES (@uid, @IDArtist)", conn))
                        {
                            cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                            cmd.Parameters.Add("@IDArtist", SqlDbType.BigInt).Value = id;
                            numFilasAfectadas += cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception) { throw; }

            return numFilasAfectadas;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y el id de un artista y devuelve si el usuario lo ha marcado como favorito
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="idArtist">ID del artista</param>
        /// <returns>Lo ha marcado como favorito o no</returns>
        public static bool isArtistFavorite(string uid, long idArtist)
        {
            bool isFavorite = false;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("SELECT 1 FROM USERARTISTS WHERE UID = @uid AND IDArtist = @IDArtist", conn))
                {
                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    cmd.Parameters.Add("@IDArtist", SqlDbType.BigInt).Value = idArtist;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        isFavorite = reader.HasRows;
                    }
                }
            }
            catch (Exception) { throw; }

            return isFavorite;
        }
    
        /// <summary>
        /// Esta función recibe el uid de un usuario y el id de un artista y lo elimina de sus favoritos
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="idArtist">ID del artista</param>
        /// <returns>Número de filas afectadas</returns>
        public static int deleteArtistFromFavoritesDAL(string uid, long idArtist)
        {
            int numFilas = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("DELETE FROM USERARTISTS WHERE UID = @uid AND IDArtist = @idArtist", conn))
                {
                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    cmd.Parameters.Add("@idArtist", SqlDbType.BigInt).Value = idArtist;
                    conn.Open();

                    numFilas = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { throw; }

            return numFilas;
        } 
    }
}
