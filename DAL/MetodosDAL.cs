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
    public class MetodosDAL
    {
        /// <summary>
        /// Esta función recibe un UID de usuario, lo busca en la base de datos y devuelve el usuario
        /// </summary>
        /// <param name="uid">UID del usuario a buscar</param>
        /// <returns>Usuario</returns>
        public static Usuario getUserByUIDDAL(String uid)
        {
            Usuario user = null;
            String name = "";
            String lastName = "";
            String email = "";
            String photoUrl = "";
            String dateJoining = "";
            String username = "";
            bool blocked = false;
            bool deleted = false;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@UID", System.Data.SqlDbType.VarChar).Value = uid;
                miComando.CommandText = "SELECT * FROM USERS WHERE UID = @UID AND UserDeleted = 0 AND UserBlocked = 0";

                miLector = miComando.ExecuteReader();

                if (miLector.HasRows) {
                    while (miLector.Read()) {
                        name = (String)miLector["Name"];
                        lastName = (String)miLector["LastName"];
                        email = (String)miLector["Email"];
                        photoUrl = (String)miLector["PhotoUrl"];
                        dateJoining = ((DateTime)miLector["DateJoining"]).ToString();
                        username = (String)miLector["Username"];
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uid, name, lastName, email, photoUrl, dateJoining, username, deleted, blocked);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return user;
        }

        /// <summary>
        /// Esta función recibe un usuario y lo guarda en la base de datos
        /// </summary>
        /// <param name="user">Usuario</param>
        /// <returns>Número de filas afectadas</returns>
        public static int createUser(Usuario user) {
            int numFilasAfectadas = 0;

            SqlCommand miComando = new SqlCommand();

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@uid", System.Data.SqlDbType.VarChar).Value = user.UID;
                miComando.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = user.Name;
                miComando.Parameters.Add("@lastName", System.Data.SqlDbType.VarChar).Value = user.LastName;
                miComando.Parameters.Add("@email", System.Data.SqlDbType.VarChar).Value = user.Email;
                miComando.Parameters.Add("@photoUrl", System.Data.SqlDbType.VarChar).Value = user.PhotoURL;
                miComando.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = user.Username;
                miComando.Parameters.Add("@deleted", System.Data.SqlDbType.Bit).Value = user.UserDeleted;
                miComando.Parameters.Add("@blocked", System.Data.SqlDbType.Bit).Value = user.UserBlocked;

                miComando.CommandText = "INSERT INTO USERS (UID, Name, LastName, Email, PhotoUrl, Username, UserDeleted, UserBlocked)" +
                    "VALUES (@uid, @name, @lastName, @email, @photoUrl, @username, @deleted, @blocked)";

                numFilasAfectadas = miComando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            } finally
            {
                clsConexion.Desconectar();
            }

            return numFilasAfectadas;
        }
        
        /// <summary>
        /// Esta función recibe un UID de usuario y lo elimina de la base de datos si existe
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Número de filas afectadas</returns>
        public static int deleteUser(String uid)
        {
            int numFilasAfectadas = 0;

            SqlCommand miComando = new SqlCommand();

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@uid", System.Data.SqlDbType.VarChar).Value = uid;

                miComando.CommandText = "DELETE FROM USERS WHERE UID = @uid";

                numFilasAfectadas = miComando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return numFilasAfectadas;
        }
    }
}
