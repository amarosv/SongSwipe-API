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
    }
}
