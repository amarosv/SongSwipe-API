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


    }
}
