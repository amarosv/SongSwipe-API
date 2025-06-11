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
    public class MetodosUserGenresDAL
    {
        /// <summary>
        /// Esta función recibe el uid de un usuario y la lista con los ids de los géneros a guardar como favoritos y los
        /// guarda en la base de datos
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="genres">Lista de ids de los géneros</param>
        /// <returns>Número de filas afectadas</returns>
        public static int addGenresToFavoritesDAL(string uid, List<long> genres)
        {
            int numFilasAfectadas = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                {
                    conn.Open();

                    foreach (long id in genres)
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO USERGENRES (UID, IDGenre) VALUES (@uid, @IDGenre)", conn))
                        {
                            cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                            cmd.Parameters.Add("@IDGenre", SqlDbType.BigInt).Value = id;
                            numFilasAfectadas += cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception) { throw; }

            return numFilasAfectadas;
        }
        
        /// <summary>
        /// Este método recibe el UID de un usuario y el ID de un género y lo elimina de la base de datos
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="idGenre">ID del género</param>
        /// <returns>Número de filas afectadas</returns>
        public static int deleteGenreFromFavoritesDAL(string uid, long idGenre)
        {
            int numFilas = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("DELETE FROM USERGENRES WHERE UID = @uid AND IDGenre = @idGenre", conn))
                {
                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    cmd.Parameters.Add("@idGenre", SqlDbType.BigInt).Value = idGenre;
                    conn.Open();

                    numFilas = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { throw; }

            return numFilas;
        }

    }
}
