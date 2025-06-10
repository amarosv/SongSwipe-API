using Azure;
using DAL.Utils;
using DTO;
using Entidades;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Windows.Input;

namespace DAL.Lists
{
    public class ListadosUserDAL
    {
        /// <summary>
        /// Esta función obtiene todos los usuarios de la base de datos y los devuelve como una lista
        /// </summary>
        /// <returns>Lista</returns>
        public static List<Usuario> getAllUsers()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM USERS WHERE UserDeleted = 0 AND UserBlocked = 0";

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string uid = (string)reader["UID"];
                            string name = (string)reader["Name"];
                            string lastName = (string)reader["LastName"];
                            string email = (string)reader["Email"];
                            string photoUrl = (string)reader["PhotoUrl"];
                            string dateJoining = ((DateTime)reader["DateJoining"]).ToString();
                            string username = (string)reader["Username"];
                            string supplier = (string)reader["Supplier"];
                            bool deleted = (bool)reader["UserDeleted"];
                            bool blocked = (bool)reader["UserBlocked"];

                            usuarios.Add(new Usuario(uid, name, lastName, email, photoUrl, dateJoining, username, supplier, deleted, blocked));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe un username y busca en la base de datos todos los usuarios que su username concuerde con el recibido
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="username">Username a buscar</param>
        /// <returns>Lista</returns>
        public static List<Usuario> getUsersByUsername(string username)
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM USERS WHERE Username LIKE @username AND UserDeleted = 0 AND UserBlocked = 0";
                    cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = "%" + username + "%";

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string uid = (string)reader["UID"];
                            string name = (string)reader["Name"];
                            string lastName = (string)reader["LastName"];
                            string email = (string)reader["Email"];
                            string photoUrl = (string)reader["PhotoUrl"];
                            string dateJoining = ((DateTime)reader["DateJoining"]).ToString();
                            string usernameUser = (string)reader["Username"];
                            string supplier = (string)reader["Supplier"];
                            bool deleted = (bool)reader["UserDeleted"];
                            bool blocked = (bool)reader["UserBlocked"];

                            usuarios.Add(new Usuario(uid, name, lastName, email, photoUrl, dateJoining, usernameUser, supplier, deleted, blocked));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, busca sus amigos y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getFriendsDAL(string uid)
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetUserFriends @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Utils.Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, busca las solicitudes de amistad que le han llegado y las devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getIncomingFriendRequestsDAL(string uid)
        {
            var usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetIncomingFriendRequests @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Utils.Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario, busca las solicitudes de amistad que ha enviado y las devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getOutgoingFriendRequestsDAL(string uid)
        {
            var usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetOutgoingFriendRequests @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Utils.Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y la id de una canción y busca los amigos del usuario que le han gustado la canción
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="trackId">ID de la canción</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getFriendsWhoLikedTrackDAL(string uid, long trackId)
        {
            List<Usuario> usuarios = new();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetFriendsWhoLikedTrack @UID, @IDTrack";
                    cmd.Parameters.AddWithValue("@UID", uid);
                    cmd.Parameters.AddWithValue("@IDTrack", trackId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Utils.Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario y obtiene de la base de datos los usuarios que este ha bloqueado
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getUsersBlockedDAL(string uid)
        {
            List<Usuario> usuarios = new();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetBlockedUsers @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Utils.Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe un UID y devuelve una lista con los usuarios a los que les ha enviado una
        /// solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getSentRequestDAL(string uid)
        {
            List<Usuario> usuarios = new();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetOutgoingFriendRequests @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Utils.Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }


        /// <summary>
        /// Esta función recibe un UID y devuelve una lista con los usuarios que le han enviado una
        /// solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getReceiveRequestDAL(string uid)
        {
            List<Usuario> usuarios = new();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC GetIncomingFriendRequests @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Utils.Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario y devuelve todos los IDs de las canciones que le han gustado
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de IDs</returns>
        public static List<long> getLikedTracksIdsDAL(string uid) {
            List<long> ids = [];

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT IDTrack FROM USERSWIPES WHERE UID = @uid AND Swipe = 1";
                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = (long)reader["IDTrack"];
                            
                            ids.Add(id);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ids;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario y devuelve todos los IDs de las canciones que no le han gustado
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de IDs</returns>
        public static List<long> getDislikedTracksIdsDAL(string uid)
        {
            List<long> ids = [];

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT IDTrack FROM USERSWIPES WHERE UID = @uid AND Swipe = 0";
                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = (long)reader["IDTrack"];

                            ids.Add(id);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ids;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario y devuelve todos los IDs de las canciones que ha swipeado
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de IDs</returns>
        public static List<long> getSwipedTracksIdsDAL(string uid)
        {
            List<long> ids = [];

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT IDTrack FROM USERSWIPES WHERE UID = @uid";
                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = (long)reader["IDTrack"];

                            ids.Add(id);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ids;
        }
        
        /// <summary>
        /// Esta función recibe el UID de un usuario y devuelve sus seguidores
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getFollowersDAL(string uid) {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT U.* FROM USERFRIENDS UF INNER JOIN USERS U ON U.UID = UF.UID WHERE UF.UIDFriend = @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Utils.Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario y devuelve sus seguidos
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de usuarios</returns>
        public static List<Usuario> getFollowigDAL(string uid)
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT U.* FROM USERFRIENDS UF INNER JOIN USERS U ON U.UID = UF.UIDFriend WHERE UF.UID = @UID";
                    cmd.Parameters.AddWithValue("@UID", uid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(Utils.Methods.mapUsuarioFromReader(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return usuarios;
        }
    }
}