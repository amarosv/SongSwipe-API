using Azure;
using DAL.Utils;
using DTO;
using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Methods
{
    public class MetodosUserDAL
    {
        /// <summary>
        /// Esta función recibe un UID de usuario, lo busca en la base de datos y devuelve el usuario
        /// </summary>
        /// <param name="uid">UID del usuario a buscar</param>
        /// <returns>Usuario</returns>
        public static Usuario getUserByUIDDAL(string uid)
        {
            Usuario user = null;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM USERS WHERE UID = @UID AND UserDeleted = 0 AND UserBlocked = 0", conn))
                {
                    cmd.Parameters.Add("@UID", SqlDbType.VarChar).Value = uid;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string name = (string)reader["Name"];
                                string lastName = (string)reader["LastName"];
                                string email = (string)reader["Email"];
                                string photoUrl = (string)reader["PhotoUrl"];
                                string dateJoining = ((DateTime)reader["DateJoining"]).ToString();
                                string username = (string)reader["Username"];
                                string supplier = (string)reader["Supplier"];
                                bool deleted = (bool)reader["UserDeleted"];
                                bool blocked = (bool)reader["UserBlocked"];

                                user = new Usuario(uid, name, lastName, email, photoUrl, dateJoining, username, supplier, deleted, blocked);
                            }
                        }
                    }
                }
            }
            catch (Exception) { throw; }

            return user;
        }

        /// <summary>
        /// Esta función recibe un usuario y lo guarda en la base de datos
        /// </summary>
        /// <param name="user">Usuario</param>
        /// <returns>Número de filas afectadas</returns>
        public static int createUserDAL(Usuario user)
        {
            int affectedRows = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("INSERT INTO USERS (UID, Name, LastName, Email, PhotoUrl, Username, Supplier, UserDeleted, UserBlocked) VALUES (@uid, @name, @lastName, @email, @photoUrl, @username, @supplier, @deleted, @blocked)", conn))
                {
                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = user.UID;
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = user.Name;
                    cmd.Parameters.Add("@lastName", SqlDbType.VarChar).Value = user.LastName;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = user.Email;
                    cmd.Parameters.Add("@photoUrl", SqlDbType.VarChar).Value = user.PhotoURL;
                    cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = user.Username;
                    cmd.Parameters.Add("@supplier", SqlDbType.VarChar).Value = user.Supplier;
                    cmd.Parameters.Add("@deleted", SqlDbType.Bit).Value = user.UserDeleted;
                    cmd.Parameters.Add("@blocked", SqlDbType.Bit).Value = user.UserBlocked;

                    conn.Open();

                    affectedRows = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { throw; }

            return affectedRows;
        }

        /// <summary>
        /// Esta función recibe un UID de usuario y lo elimina de la base de datos si existe
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Número de filas afectadas</returns>
        public static int deleteUserDAL(string uid)
        {
            int numFilasAfectadas = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("DELETE FROM USERS WHERE UID = @uid", conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    numFilasAfectadas = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { throw; }

            return numFilasAfectadas;
        }

        /// <summary>
        /// Esta función recibe un usuario modificado y lo actualiza en la base de datos
        /// </summary>
        /// <param name="user">Usuario modificado</param>
        /// <returns>Número de filas afectadas</returns>
        public static int updateUserDAL(Usuario user)
        {
            int affectedRows = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("UPDATE USERS SET Name = @name, LastName = @lastName, Email = @email, PhotoUrl = @photoUrl, Username = @username WHERE UID = @uid", conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = user.UID;
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = user.Name;
                    cmd.Parameters.Add("@lastName", SqlDbType.VarChar).Value = user.LastName;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = user.Email;
                    cmd.Parameters.Add("@photoUrl", SqlDbType.VarChar).Value = user.PhotoURL;
                    cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = user.Username;
                    cmd.Parameters.Add("@deleted", SqlDbType.Bit).Value = user.UserDeleted;
                    cmd.Parameters.Add("@blocked", SqlDbType.Bit).Value = user.UserBlocked;

                    affectedRows = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { throw; }

            return affectedRows;
        }

        /// <summary>
        /// Esta función recibe un username y comprueba que no exista en la base de datos
        /// </summary>
        /// <param name="username">Username a comprobar</param>
        /// <returns>Existe o no</returns>
        public static bool checkUsernameDAL(string username)
        {
            bool exists = false;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS TOTAL FROM USERS WHERE Username = @username", conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int total = (int)reader["TOTAL"];
                                exists = total > 0;
                            }
                        }
                    }
                }
            }
            catch (Exception) { throw; }

            return exists;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y devuelve la información a mostrar en la pantalla de perfil
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Usuario con los datos a mostrar en el perfil</returns>
        public static UserProfile getUserProfileDataDAL(string uid)
        {
            UserProfile userProfile = null;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("EXEC GetUserProfileData @UID", conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@UID", uid);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string uidUser = (string)reader["UID"];
                                string name = (string)reader["Name"];
                                string lastName = (string)reader["LastName"];
                                string email = (string)reader["Email"];
                                string photoUrl = (string)reader["PhotoUrl"];
                                string dateJoining = ((DateTime)reader["DateJoining"]).ToString();
                                string username = (string)reader["Username"];
                                int savedSongs = (int)reader["SavedSongs"];
                                int followers = (int)reader["Followers"];
                                int following = (int)reader["Following"];

                                userProfile = new UserProfile(uid, username, name, lastName, photoUrl, dateJoining, email, savedSongs, followers, following);
                            }
                        }
                    }
                }
            }
            catch (Exception) { throw; }

            return userProfile;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y devuelve sus ajustes
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Ajustes del usuario</returns>
        public static Settings getUserSettingsDAL(string uid)
        {
            Settings settings = null;

            try
            {
                using (SqlConnection conexion = clsConexion.GetConnection())
                using (SqlCommand comando = new SqlCommand("EXEC GetUserSettings @UID", conexion))
                {
                    comando.Parameters.AddWithValue("@UID", uid);

                    conexion.Open();
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.HasRows)
                        {
                            while (lector.Read())
                            {
                                int mode = (int)lector["Mode"];
                                int theme = (int)lector["Theme"];
                                bool cardAnimatedCover = (bool)lector["Card_Animated_Cover"];
                                bool cardSkipSongs = (bool)lector["Card_Skip_Songs"];
                                bool cardBlurredCoverAsBackground = (bool)lector["Card_Blurred_Cover_As_Background"];
                                int privacyVisSavedSongs = (int)lector["Privacy_Vis_Saved_Songs"];
                                int privacyVisStats = (int)lector["Privacy_Vis_Stats"];
                                int privacyVisFol = (int)lector["Privacy_Vis_Fol"];
                                bool privateAccount = (bool)lector["Private_Account"];
                                string language = (string)lector["Language"];
                                bool audioLoop = (bool)lector["Audio_Loop"];
                                bool audioAutoPlay = (bool)lector["Audio_Autoplay"];
                                bool audioOnlyAudio = (bool)lector["Audio_Only_Audio"];
                                bool notifications = (bool)lector["Notifications"];
                                bool notiFriendsRequest = (bool)lector["Noti_Friends_Request"];
                                bool notiFriendsApproved = (bool)lector["Noti_Friends_Approved"];
                                bool notiAppUpdate = (bool)lector["Noti_App_Update"];
                                bool notiAppRecap = (bool)lector["Noti_App_Recap"];
                                bool notiAccountBlocked = (bool)lector["Noti_Account_Blocked"];
                                bool showTutorial = (bool)lector["Show_Tutorial"];

                                settings = new Settings(
                                    mode, theme, cardAnimatedCover, cardSkipSongs, cardBlurredCoverAsBackground,
                                    privacyVisSavedSongs, privacyVisStats, privacyVisFol, privateAccount, language,
                                    audioLoop, audioAutoPlay, audioOnlyAudio, notifications,
                                    notiFriendsRequest, notiFriendsApproved, notiAppUpdate,
                                    notiAppRecap, notiAccountBlocked, showTutorial
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return settings;
        }

        /// <summary>
        /// Esta función recibe los ajustes de un usuario modificados y los actualiza en la base de datos
        /// </summary>
        /// <param name="settings">Ajustes modificados</param>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Número de filas afectadas</returns>
        public static int updateUserSettingsDAL(Settings settings, string uid)
        {
            int numFilasAfectadas = 0;

            try
            {
                using (SqlConnection conexion = clsConexion.GetConnection())
                using (SqlCommand comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"
                UPDATE USERSETTINGS
                SET Mode = @mode,
                    Theme = @theme,
                    Card_Animated_Cover = @cardAnimatedCover,
                    Card_Skip_Songs = @cardSkipSongs,
                    Card_Blurred_Cover_As_Background = @cardBlurredCoverAsBackground,
                    Privacy_Vis_Saved_Songs = @privacyVisSavedSongs,
                    Privacy_Vis_Stats = @privacyVisStats,
                    Privacy_Vis_Fol = @privacyVisFol,
                    Private_Account = @privateAccount,
                    Language = @language,
                    Audio_Loop = @audioLoop,
                    Audio_Autoplay = @audioAutoPlay,
                    Audio_Only_Audio = @audioOnlyAudio,
                    Notifications = @notifications,
                    Noti_Friends_Request = @notiFriendRequest,
                    Noti_Friends_Approved = @notiFriendApproved,
                    Noti_App_Update = @notiAppUpdate,
                    Noti_App_Recap = @notiAppRecap,
                    Noti_Account_Blocked = @notiAccountBlocked,
                    Show_Tutorial = @showTutorial
                WHERE UID = @uid";

                    comando.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    comando.Parameters.Add("@mode", SqlDbType.Int).Value = settings.Mode;
                    comando.Parameters.Add("@theme", SqlDbType.Int).Value = settings.Theme;
                    comando.Parameters.Add("@cardAnimatedCover", SqlDbType.Bit).Value = settings.CardAnimatedCover;
                    comando.Parameters.Add("@cardSkipSongs", SqlDbType.Bit).Value = settings.CardSkipSongs;
                    comando.Parameters.Add("@cardBlurredCoverAsBackground", SqlDbType.Bit).Value = settings.CardBlurredCoverAsBackground;
                    comando.Parameters.Add("@privacyVisSavedSongs", SqlDbType.Int).Value = settings.PrivacyVisSavedSongs;
                    comando.Parameters.Add("@privacyVisStats", SqlDbType.Int).Value = settings.PrivacyVisStats;
                    comando.Parameters.Add("@privacyVisFol", SqlDbType.Int).Value = settings.PrivacyVisFol;
                    comando.Parameters.Add("@privateAccount", SqlDbType.Bit).Value = settings.PrivateAccount;
                    comando.Parameters.Add("@language", SqlDbType.VarChar).Value = settings.Language;
                    comando.Parameters.Add("@audioLoop", SqlDbType.Bit).Value = settings.AudioLoop;
                    comando.Parameters.Add("@audioAutoPlay", SqlDbType.Bit).Value = settings.AudioAutoPlay;
                    comando.Parameters.Add("@audioOnlyAudio", SqlDbType.Bit).Value = settings.AudioOnlyAudio;
                    comando.Parameters.Add("@notifications", SqlDbType.Bit).Value = settings.Notifications;
                    comando.Parameters.Add("@notiFriendRequest", SqlDbType.Bit).Value = settings.NotiFriendsRequest;
                    comando.Parameters.Add("@notiFriendApproved", SqlDbType.Bit).Value = settings.NotiFriendsApproved;
                    comando.Parameters.Add("@notiAppUpdate", SqlDbType.Bit).Value = settings.NotiAppUpdate;
                    comando.Parameters.Add("@notiAppRecap", SqlDbType.Bit).Value = settings.NotiAppRecap;
                    comando.Parameters.Add("@notiAccountBlocked", SqlDbType.Bit).Value = settings.NotiAccountBlocked;
                    comando.Parameters.Add("@showTutorial", SqlDbType.Bit).Value = settings.ShowTutorial;

                    conexion.Open();
                    numFilasAfectadas = comando.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return numFilasAfectadas;
        }

        /// <summary>
        /// Esta función recibe un email y comprueba que no exista en la base de datos
        /// </summary>
        /// <param name="email">Email a comprobar</param>
        /// <returns>Existe o no</returns>
        public static bool checkEmailDAL(string email)
        {
            bool exists = false;

            try
            {
                using (SqlConnection conexion = clsConexion.GetConnection())
                using (SqlCommand comando = conexion.CreateCommand())
                {
                    comando.CommandText = "SELECT COUNT(*) FROM USERS WHERE Email = @email";
                    comando.Parameters.Add("@email", SqlDbType.VarChar).Value = email;

                    conexion.Open();
                    int total = (int)comando.ExecuteScalar();
                    exists = total > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return exists;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario y el UID de otro usuario y comprueba si son amigos
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="friend">UID del amigo</param>
        /// <returns>Son amigos o no</returns>
        public static bool isMyFriendDAL(string uid, string friend)
        {
            bool isMyFriend = false;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                WITH FriendsCheck AS (
                    SELECT 1 AS FRIEND FROM USERFRIENDS
                    WHERE (UID = @uid AND UIDFriend = @friend)
                       OR (UID = @friend AND UIDFriend = @uid)
                )
                SELECT * FROM FriendsCheck WHERE (SELECT COUNT(*) FROM FriendsCheck) = 2";

                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    cmd.Parameters.Add("@friend", SqlDbType.VarChar).Value = friend;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        isMyFriend = reader.HasRows;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return isMyFriend;
        }


        /// <summary>
        /// Esta función recibe el UID de un usuario y el UID de otro usuario y comprueba si lo sigue
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="friend">UID del seguido</param>
        /// <returns>Lo sigue o no</returns>
        public static bool followingDAL(string uid, string uidUser)
        {
            bool following = false;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT 1 FROM USERFRIENDS WHERE UID = @uid AND UIDFriend = @friend";

                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    cmd.Parameters.Add("@friend", SqlDbType.VarChar).Value = uidUser;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        following = reader.HasRows;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return following;
        }

        /// <summary>
        /// Esta función recibe dos UIDs y envía una solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del emisor</param>
        /// <param name="friend">UID del receptor</param>
        /// <returns>Número de filas afectadas</returns>
        public static int sendRequestDAL(string uid, string friend)
        {
            int numFilasAfectadas = 0;

            try
            {
                Settings settings = getUserSettingsDAL(friend);

                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    cmd.Parameters.Add("@friend", SqlDbType.VarChar).Value = friend;

                    cmd.CommandText = settings != null && !settings.PrivateAccount
                        ? "INSERT INTO USERFRIENDS (UID, UIDFriend) VALUES (@uid, @friend)"
                        : "INSERT INTO USERFRIENDREQUEST (UIDSender, UIDReceiver) VALUES (@uid, @friend)";

                    conn.Open();
                    numFilasAfectadas = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return numFilasAfectadas;
        }

        /// <summary>
        /// Esta función recibe dos UIDs y elimina una solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del emisor</param>
        /// <param name="friend">UID del receptor</param>
        /// <returns>Número de filas afectadas</returns>
        public static int deleteRequestDAL(string uid, string friend)
        {
            int numFilasAfectadas = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM USERFRIENDREQUEST WHERE UIDSender = @uid AND UIDReceiver = @friend";
                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    cmd.Parameters.Add("@friend", SqlDbType.VarChar).Value = friend;

                    conn.Open();
                    numFilasAfectadas = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return numFilasAfectadas;
        }

        /// <summary>
        /// Esta función recibe dos UIDs y acepta una solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del emisor</param>
        /// <param name="friend">UID del receptor</param>
        /// <returns>Número de filas afectadas</returns>
        public static int acceptRequestDAL(string uid, string friend)
        {
            int numFilasAfectadas = 0;

            using (SqlConnection conn = clsConexion.GetConnection())
            {
                conn.Open();
                SqlTransaction transaccion = conn.BeginTransaction();

                try
                {
                    using (SqlCommand insertarAmigo = new SqlCommand(
                        "INSERT INTO USERFRIENDS (UID, UIDFriend) VALUES (@uid, @friend)",
                        conn, transaccion))
                    {
                        insertarAmigo.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                        insertarAmigo.Parameters.Add("@friend", SqlDbType.VarChar).Value = friend;
                        numFilasAfectadas += insertarAmigo.ExecuteNonQuery();
                    }

                    using (SqlCommand eliminarSolicitud = new SqlCommand(
                        "DELETE FROM USERFRIENDREQUEST WHERE UIDSender = @uid AND UIDReceiver = @friend",
                        conn, transaccion))
                    {
                        eliminarSolicitud.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                        eliminarSolicitud.Parameters.Add("@friend", SqlDbType.VarChar).Value = friend;
                        numFilasAfectadas += eliminarSolicitud.ExecuteNonQuery();
                    }

                    transaccion.Commit();
                }
                catch (Exception)
                {
                    transaccion.Rollback();
                    throw;
                }
            }

            return numFilasAfectadas;
        }

        /// <summary>
        /// Esta función recibe dos UIDs y rechaza una solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del emisor</param>
        /// <param name="friend">UID del receptor</param>
        /// <returns>Número de filas afectadas</returns>
        public static int declineRequestDAL(string uid, string friend)
        {
            int numFilasAfectadas = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM USERFRIENDREQUEST WHERE UIDSender = @friend AND UIDReceiver = @uid";
                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    cmd.Parameters.Add("@friend", SqlDbType.VarChar).Value = friend;

                    conn.Open();
                    numFilasAfectadas = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return numFilasAfectadas;
        }

        /// <summary>
        /// Esta función recibe el UID y aceota todas sus solicitudes de amistad recibidas
        /// </summary>
        /// <param name="uid"></param>
        public static void acceptAllRequestsDAL(string uid)
        {
            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("EXEC AcceptAllFriendRequests @UID", conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@UID", uid);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Esta función recibe dos UIDs y comprueba si ya se le ha enviado una solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del emisor</param>
        /// <param name="friend">UID del receptor</param>
        /// <returns>Solicitud enviada o no</returns>
        public static bool requestSentDAL(string uid, string friend)
        {
            bool sent = false;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT 1 
                FROM USERFRIENDREQUEST 
                WHERE UIDSender = @uid AND UIDReceiver = @friend";

                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    cmd.Parameters.Add("@friend", SqlDbType.VarChar).Value = friend;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        sent = reader.HasRows;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return sent;
        }

        /// <summary>
        /// Esta función recibe dos UIDs y elimina al amigo
        /// </summary>
        /// <param name="uid">UID del emisor</param>
        /// <param name="friend">UID del receptor</param>
        /// <returns>Número de filas afectadas</returns>
        public static int deleteFriendDAL(string uid, string friend)
        {
            int numFilasAfectadas = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM USERFRIENDS WHERE UID = @uid AND UIDFriend = @friend";
                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    cmd.Parameters.Add("@friend", SqlDbType.VarChar).Value = friend;

                    conn.Open();
                    numFilasAfectadas = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return numFilasAfectadas;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario y reactiva su cuenta eliminada
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Número de filas afectadas</returns>
        public static int reactivateAccountDAL(string uid) {
            int numFilasAfectadas = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("EXEC ReactivateUserAccount @uid", conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@uid", uid);
                    numFilasAfectadas = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { throw; }

            return numFilasAfectadas;
        }
    }
}
