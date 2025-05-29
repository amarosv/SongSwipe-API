using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Utils
{
    public class Methods
    {
        /// <summary>
        /// Esta función recibe los datos de un reader y los mapea en un Usuario
        /// </summary>
        /// <param name="reader">Datos del reader</param>
        /// <returns>Usuario</returns>
        public static Usuario mapUsuarioFromReader(SqlDataReader reader)
        {
            return new Usuario(
                uid: (string)reader["UID"],
                name: (string)reader["Name"],
                lastName: (string)reader["LastName"],
                email: (string)reader["Email"],
                photoURL: (string)reader["PhotoUrl"],
                dateJoining: ((DateTime)reader["DateJoining"]).ToString(),
                username: (string)reader["Username"],
                supplier: (string)reader["Supplier"],
                userDeleted: (bool)reader["UserDeleted"],
                userBlocked: (bool)reader["UserBlocked"]
            );
        }

        /// <summary>
        /// Esta función obtiene los ids de las canciones/generos/artistas de la base de datos
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="procedure">Procedure a ejecutar</param>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de ids de canciones/generos/artistas</returns>
        public static (int total, (int totalPages, List<long> list) result) getListIdsDAL(string procedure, string uid, int page, int limit)
        {
            List<long> ids = new();
            int totalPages = 0;
            int totalTracks = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = procedure;
                    cmd.Parameters.AddWithValue("@UID", uid);
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@limit", limit);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalPages = reader.GetInt32(0);
                        }

                        if (reader.NextResult()) {
                            if (reader.Read())
                            {
                                totalTracks = reader.GetInt32(0);
                            }
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                ids.Add((long)reader["ID"]);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return (totalTracks, (totalPages, ids));
        }

    }
}
