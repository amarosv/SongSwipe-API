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

namespace DAL
{
    public class MetodosUserDAL
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
            String supplier = "";
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

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        name = (String)miLector["Name"];
                        lastName = (String)miLector["LastName"];
                        email = (String)miLector["Email"];
                        photoUrl = (String)miLector["PhotoUrl"];
                        dateJoining = ((DateTime)miLector["DateJoining"]).ToString();
                        username = (String)miLector["Username"];
                        supplier = (String)miLector["Supplier"];
                        deleted = (bool)miLector["UserDeleted"];
                        blocked = (bool)miLector["UserBlocked"];

                        user = new Usuario(uid, name, lastName, email, photoUrl, dateJoining, username, supplier, deleted, blocked);
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
        public static int createUserDAL(Usuario user)
        {
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
                miComando.Parameters.Add("@supplier", System.Data.SqlDbType.VarChar).Value = user.Supplier;
                miComando.Parameters.Add("@deleted", System.Data.SqlDbType.Bit).Value = user.UserDeleted;
                miComando.Parameters.Add("@blocked", System.Data.SqlDbType.Bit).Value = user.UserBlocked;

                miComando.CommandText = "INSERT INTO USERS (UID, Name, LastName, Email, PhotoUrl, Username, Supplier, UserDeleted, UserBlocked)" +
                    "VALUES (@uid, @name, @lastName, @email, @photoUrl, @username, @supplier, @deleted, @blocked)";

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

        /// <summary>
        /// Esta función recibe un UID de usuario y lo elimina de la base de datos si existe
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Número de filas afectadas</returns>
        public static int deleteUserDAL(String uid)
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

        /// <summary>
        /// Esta función recibe un usuario modificado y lo actualiza en la base de datos
        /// </summary>
        /// <param name="user">Usuario modificado</param>
        /// <returns>Número de filas afectadas</returns>
        public static int updateUserDAL(Usuario user)
        {
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

                miComando.CommandText = "UPDATE USERS " +
                    "SET Name = @name, LastName = @lastName, Email = @email, PhotoUrl = @photoUrl, Username = @username " +
                    "WHERE UID = @uid";

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

        /// <summary>
        /// Esta función recibe un username y comprueba que no exista en la base de datos
        /// </summary>
        /// <param name="username">Username a comprobar</param>
        /// <returns>Existe o no</returns>
        public static bool checkUsernameDAL(String username)
        {
            bool exists = false;
            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = username;
                miComando.CommandText = "SELECT COUNT(*) AS TOTAL FROM USERS WHERE Username = @username";

                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        int total = (int)miLector["TOTAL"];

                        exists = total > 0;
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

            return exists;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y la lista con los ids de los artistas a guardar como favoritos y los
        /// guarda en la base de datos
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="artists">Lista de ids de los artistas</param>
        /// <returns>Número de filas afectadas</returns>
        public static int addArtistsToFavoritesDAL(String uid, List<long> artists)
        {
            int numFilasAfectadas = 0;

            SqlCommand miComando = new SqlCommand();

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                foreach (long id in artists)
                {
                    // Crear un nuevo parámetro para cada iteración
                    SqlParameter uidUser = miComando.Parameters.Add("@uid", System.Data.SqlDbType.VarChar);
                    uidUser.Value = uid;
                    SqlParameter idArtistParam = new SqlParameter("@IDArtist", System.Data.SqlDbType.BigInt);
                    idArtistParam.Value = id;
                    miComando.Parameters.Clear();  // Limpiar los parámetros previos
                    miComando.Parameters.Add(uidUser);
                    miComando.Parameters.Add(idArtistParam);  // Añadir el nuevo parámetro

                    miComando.CommandText = "INSERT INTO USERARTISTS (UID, IDArtist) VALUES (@uid, @IDArtist)";
                    numFilasAfectadas += miComando.ExecuteNonQuery();
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

            return numFilasAfectadas;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y la lista con los ids de los géneros a guardar como favoritos y los
        /// guarda en la base de datos
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="genres">Lista de ids de los géneros</param>
        /// <returns>Número de filas afectadas</returns>
        public static int addGenresToFavoritesDAL(String uid, List<long> genres)
        {
            int numFilasAfectadas = 0;

            SqlCommand miComando = new SqlCommand();

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                foreach (long id in genres)
                {
                    // Crear un nuevo parámetro para cada iteración
                    SqlParameter uidUser = miComando.Parameters.Add("@uid", System.Data.SqlDbType.VarChar);
                    uidUser.Value = uid;
                    SqlParameter idGenreParam = new SqlParameter("@IDGenre", System.Data.SqlDbType.BigInt);
                    idGenreParam.Value = id;
                    miComando.Parameters.Clear();  // Limpiar los parámetros previos
                    miComando.Parameters.Add(uidUser);
                    miComando.Parameters.Add(idGenreParam);  // Añadir el nuevo parámetro

                    miComando.CommandText = "INSERT INTO USERGENRES (UID, IDGenre) VALUES (@uid, @IDGenre)";
                    numFilasAfectadas += miComando.ExecuteNonQuery();
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

            return numFilasAfectadas;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y devuelve la información a mostrar en la pantalla de perfil
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Usuario con los datos a mostrar en el perfil</returns>
        public static UserProfile getUserProfileDataDAL(String uid)
        {
            UserProfile userProfile = null;
            String uidUser = "";
            String name = "";
            String lastName = "";
            String email = "";
            String photoUrl = "";
            String dateJoining = "";
            String username = "";
            int savedSongs;
            int followers;
            int following;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.CommandText = "EXEC GetUserProfileData @UID";
                miComando.Parameters.AddWithValue("@UID", uid);
                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        uidUser = (String)miLector["UID"];
                        name = (String)miLector["Name"];
                        lastName = (String)miLector["LastName"];
                        email = (String)miLector["Email"];
                        photoUrl = (String)miLector["PhotoUrl"];
                        dateJoining = ((DateTime)miLector["DateJoining"]).ToString();
                        username = (String)miLector["Username"];
                        savedSongs = (int)miLector["SavedSongs"];
                        followers = (int)miLector["Followers"];
                        following = (int)miLector["Following"];

                        userProfile = new UserProfile(uid, username, name, lastName, photoUrl, dateJoining, email, savedSongs, followers, following);
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

            return userProfile;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y devuelve sus ajustes
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Ajustes del usuario</returns>
        public static Settings getUserSettingsDAL(String uid)
        {
            Settings settings = null;
            int mode;
            int theme;
            bool cardAnimatedCover;
            bool cardSkipSongs;
            bool cardBlurredCoverAsBackground;
            int privacyVisSavedSongs;
            int privacyVisStats;
            int privacyVisFol;
            bool privateAccount;
            string language;
            bool audioLoop;
            bool audioAutoPlay;
            bool audioOnlyAudio;
            bool notifications;
            bool notiFriendsRequest;
            bool notiFriendsApproved;
            bool notiAppUpdate;
            bool notiAppRecap;
            bool notiAccountBlocked;
            bool showTutorial;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.CommandText = "EXEC GetUserSettings @UID";
                miComando.Parameters.AddWithValue("@UID", uid);
                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        mode = (int)miLector["Mode"];
                        theme = (int)miLector["Theme"];
                        cardAnimatedCover = (bool)miLector["Card_Animated_Cover"];
                        cardSkipSongs = (bool)miLector["Card_Skip_Songs"];
                        cardBlurredCoverAsBackground = (bool)miLector["Card_Blurred_Cover_As_Background"];
                        privacyVisSavedSongs = (int)miLector["Privacy_Vis_Saved_Songs"];
                        privacyVisStats = (int)miLector["Privacy_Vis_Stats"];
                        privacyVisFol = (int)miLector["Privacy_Vis_Fol"];
                        privateAccount = (bool)miLector["Private_Account"];
                        language = (string)miLector["Language"];
                        audioLoop = (bool)miLector["Audio_Loop"];
                        audioAutoPlay = (bool)miLector["Audio_Autoplay"];
                        audioOnlyAudio = (bool)miLector["Audio_Only_Audio"];
                        notifications = (bool)miLector["Notifications"];
                        notiFriendsRequest = (bool)miLector["Noti_Friends_Request"];
                        notiFriendsApproved = (bool)miLector["Noti_Friends_Approved"];
                        notiAppUpdate = (bool)miLector["Noti_App_Update"];
                        notiAppRecap = (bool)miLector["Noti_App_Recap"];
                        notiAccountBlocked = (bool)miLector["Noti_Account_Blocked"];
                        showTutorial = (bool)miLector["Show_Tutorial"];

                        settings = new Settings(mode, theme, cardAnimatedCover, cardSkipSongs, cardBlurredCoverAsBackground, privacyVisSavedSongs, privacyVisStats, privacyVisFol, privateAccount, language, audioLoop, audioAutoPlay, audioOnlyAudio, notifications, notiFriendsRequest, notiFriendsApproved, notiAppUpdate, notiAppRecap, notiAccountBlocked, showTutorial);
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

            return settings;
        }

        /// <summary>
        /// Esta función recibe los ajustes de un usuario modificados y los actualiza en la base de datos
        /// </summary>
        /// <param name="settings">Ajustes modificados</param>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Número de filas afectadas</returns>
        public static int updateUserSettingsDAL(Settings settings, String uid)
        {
            int numFilasAfectadas = 0;

            SqlCommand miComando = new SqlCommand();

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@uid", System.Data.SqlDbType.VarChar).Value = uid;
                miComando.Parameters.Add("@mode", System.Data.SqlDbType.Int).Value = settings.Mode;
                miComando.Parameters.Add("@theme", System.Data.SqlDbType.Int).Value = settings.Theme;
                miComando.Parameters.Add("@cardAnimatedCover", System.Data.SqlDbType.Bit).Value = settings.CardAnimatedCover;
                miComando.Parameters.Add("@cardSkipSongs", System.Data.SqlDbType.Bit).Value = settings.CardSkipSongs;
                miComando.Parameters.Add("@cardBlurredCoverAsBackground", System.Data.SqlDbType.Bit).Value = settings.CardBlurredCoverAsBackground;
                miComando.Parameters.Add("@privacyVisSavedSongs", System.Data.SqlDbType.Int).Value = settings.PrivacyVisSavedSongs;
                miComando.Parameters.Add("@privacyVisStats", System.Data.SqlDbType.Int).Value = settings.PrivacyVisStats;
                miComando.Parameters.Add("@privacyVisFol", System.Data.SqlDbType.Int).Value = settings.PrivacyVisFol;
                miComando.Parameters.Add("@privateAccount", System.Data.SqlDbType.Bit).Value = settings.PrivateAccount;
                miComando.Parameters.Add("@language", System.Data.SqlDbType.VarChar).Value = settings.Language;
                miComando.Parameters.Add("@audioLoop", System.Data.SqlDbType.Bit).Value = settings.AudioLoop;
                miComando.Parameters.Add("@audioAutoPlay", System.Data.SqlDbType.Bit).Value = settings.AudioAutoPlay;
                miComando.Parameters.Add("@audioOnlyAudio", System.Data.SqlDbType.Bit).Value = settings.AudioOnlyAudio;
                miComando.Parameters.Add("@notifications", System.Data.SqlDbType.Bit).Value = settings.Notifications;
                miComando.Parameters.Add("@notiFriendRequest", System.Data.SqlDbType.Bit).Value = settings.NotiFriendsRequest;
                miComando.Parameters.Add("@notiFriendApproved", System.Data.SqlDbType.Bit).Value = settings.NotiFriendsApproved;
                miComando.Parameters.Add("@notiAppUpdate", System.Data.SqlDbType.Bit).Value = settings.NotiAppUpdate;
                miComando.Parameters.Add("@notiAppRecap", System.Data.SqlDbType.Bit).Value = settings.NotiAppRecap;
                miComando.Parameters.Add("@notiAccountBlocked", System.Data.SqlDbType.Bit).Value = settings.NotiAccountBlocked;
                miComando.Parameters.Add("@showTutorial", System.Data.SqlDbType.Bit).Value = settings.ShowTutorial;

                miComando.CommandText = "UPDATE USERSETTINGS " +
                    "SET Mode = @mode, Theme = @theme, Card_Animated_Cover = @cardAnimatedCover, Card_Skip_Songs = @cardSkipSongs," +
                    "Card_Blurred_Cover_As_Background = @cardBlurredCoverAsBackground, Privacy_Vis_Saved_Songs = @privacyVisSavedSongs," +
                    "Privacy_Vis_Stats = @privacyVisStats, Privacy_Vis_Fol = @privacyVisFol, Private_Account = @privateAccount, Language = @language," +
                    "Audio_Loop = @audioLoop, Audio_Autoplay = @audioAutoPlay, Audio_Only_Audio = @audioOnlyAudio, Notifications = @notifications," +
                    "Noti_Friends_Request = @notiFriendRequest, Noti_Friends_Approved = @notiFriendApproved, Noti_App_Update = @notiAppUpdate," +
                    "Noti_App_Recap = @notiAppRecap, Noti_Account_Blocked = @notiAccountBlocked, Show_Tutorial = @showTutorial " +
                    "WHERE UID = @uid";

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

        /// <summary>
        /// Esta función recibe un email y comprueba que no exista en la base de datos
        /// </summary>
        /// <param name="email">Email a comprobar</param>
        /// <returns>Existe o no</returns>
        public static bool checkEmailDAL(String email)
        {
            bool exists = false;
            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@email", System.Data.SqlDbType.VarChar).Value = email;
                miComando.CommandText = "SELECT COUNT(*) AS TOTAL FROM USERS WHERE Email = @email";

                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        int total = (int)miLector["TOTAL"];

                        exists = total > 0;
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

            return exists;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y una lista de id de canciones, comprueba cuales el usuario ha guardado
        /// y devuelve las que no estan guardadas aún
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="idTrack">ID de la canción</param>
        /// <returns>Listas de ids de canciones que no están guardadas</returns>
        public static List<long> hasUserSavedTrackDAL(string uid, List<long> idsTracks)
        {
            List<long> tracksNotSaved = new List<long>();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                {
                    // Crear parámetros dinámicos
                    List<string> paramNames = new List<string>();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    for (int i = 0; i < idsTracks.Count; i++)
                    {
                        string paramName = "@id" + i;
                        cmd.Parameters.AddWithValue(paramName, idsTracks[i]);
                        paramNames.Add(paramName);
                    }

                    cmd.Parameters.AddWithValue("@uid", uid);

                    cmd.CommandText = $@"
                        SELECT IDTrack 
                        FROM USERSWIPES 
                        WHERE UID = @uid AND IDTrack IN ({string.Join(", ", paramNames)})
                    ";

                    HashSet<long> savedTrackIds = new HashSet<long>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            savedTrackIds.Add(reader.GetInt64(0));
                        }
                    }

                    // Comparar con la lista original
                    foreach (long id in idsTracks)
                    {
                        if (!savedTrackIds.Contains(id))
                        {
                            tracksNotSaved.Add(id);
                        }
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

            return tracksNotSaved;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario y una lista de Swipes y los inserta en la base de datos
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="swipes">Lista de Swipes</param>
        /// <returns>Bool que indica si se ha guardado</returns>
        public static int saveSwipesDAL(string uid, List<Swipe> swipes)
        {
            int numFilasAfectadas = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                {
                    // Construimos la parte VALUES del SQL
                    List<string> valuesClauses = new List<string>();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = $"INSERT INTO USERSWIPES (UID, IDTrack, IDAlbum, IDArtist, Swipe) VALUES ";

                    for (int i = 0; i < swipes.Count; i++)
                    {
                        cmd.CommandText += $"('{uid}', {swipes[i].Id}, {swipes[i].IdAlbum}, {swipes[i].IdArtist}, {swipes[i].Like})";
                        if (i < swipes.Count - 1)
                        {
                            cmd.CommandText += ", ";
                        }

                        //string uidParam = $"@uid{i}";
                        //string idTrackParam = $"@idTrack{i}";
                        //string idAlbumParam = $"@idTrack{i}";
                        //string swipeParam = $"@swipe{i}";

                        //valuesClauses.Add($"({uidParam}, {idTrackParam}, {idAlbumParam}, {swipeParam})");

                        //cmd.Parameters.AddWithValue(uidParam, uid);
                        //cmd.Parameters.AddWithValue(idTrackParam, swipes[i].Id);
                        //cmd.Parameters.AddWithValue(idAlbumParam, swipes[i].IdAlbum);
                        //cmd.Parameters.AddWithValue(swipeParam, swipes[i].Like);
                    }

                    //cmd.CommandText = $"INSERT INTO USERSWIPES (UID, IDTrack, IDAlbum, Swipe) VALUES {string.Join(", ", valuesClauses)}";

                    numFilasAfectadas = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return numFilasAfectadas;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario y el UID de otro usuario y comprueba si son amigos
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="friend">UID del amigo</param>
        /// <returns>Son amigos o no</returns>
        public static bool isMyFriendDAL(String uid, String friend) {
            bool isMyFriend = false;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@uid", System.Data.SqlDbType.VarChar).Value = uid;
                miComando.Parameters.Add("@friend", System.Data.SqlDbType.VarChar).Value = friend;
                miComando.CommandText = "WITH FriendsCheck AS (" +
                    "SELECT 1 AS FRIEND FROM USERFRIENDS WHERE" +
                    "(UID = @uid AND UIDFriend = @friend)" +
                    "OR" +
                    "(UID = @friend AND UIDFriend = @uid)" +
                    ")" +
                    "SELECT * FROM FriendsCheck WHERE (SELECT COUNT(*) FROM FriendsCheck) = 2";


                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        int total = (int)miLector["FRIEND"];

                        isMyFriend = total == 1;
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

            return isMyFriend;
        }
            
        /// <summary>
        /// Esta función recibe dos UIDs y envía una solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del emisor</param>
        /// <param name="friend">UID del receptor</param>
        /// <returns>Número de filas afectadas</returns>
        public static int sendRequestDAL(String uid, String friend) {
            int numFilasAfectadas = 0;

            // Primero obtenemos si la cuenta a enviar la solicitud es pública
            Settings settings = getUserSettingsDAL(friend);

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@uid", System.Data.SqlDbType.VarChar).Value = uid;
                miComando.Parameters.Add("@friend", System.Data.SqlDbType.VarChar).Value = friend;

                if (settings != null && !settings.PrivateAccount) {
                    miComando.CommandText = "INSERT INTO USERFRIENDS (UID, UIDFriend) VALUES (@uid, @friend)";
                } else {
                    miComando.CommandText = "INSERT INTO USERFRIENDREQUEST (UIDSender, UIDReceiver) VALUES (@uid, @friend)";
                }
                
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

        /// <summary>
        /// Esta función recibe dos UIDs y elimina una solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del emisor</param>
        /// <param name="friend">UID del receptor</param>
        /// <returns>Número de filas afectadas</returns>
        public static int deleteRequestDAL(String uid, String friend)
        {
            int numFilasAfectadas = 0;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@uid", System.Data.SqlDbType.VarChar).Value = uid;
                miComando.Parameters.Add("@friend", System.Data.SqlDbType.VarChar).Value = friend;
                miComando.CommandText = "DELETE FROM USERFRIENDREQUEST WHERE UIDSender = @uid AND UIDReceiver = @friend";

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

        /// <summary>
        /// Esta función recibe dos UIDs y acepta una solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del emisor</param>
        /// <param name="friend">UID del receptor</param>
        /// <returns>Número de filas afectadas</returns>
        public static int acceptRequestDAL(string uid, string friend)
        {
            int numFilasAfectadas = 0;

            SqlConnection conexion = clsConexion.GetConnection();
            SqlTransaction transaccion = null;

            try
            {
                // Iniciamos la transacción
                transaccion = conexion.BeginTransaction();

                SqlCommand insertarAmigo = new SqlCommand(
                    "INSERT INTO USERFRIENDS (UID, UIDFriend) VALUES (@uid, @friend)",
                    conexion, transaccion);

                insertarAmigo.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                insertarAmigo.Parameters.Add("@friend", SqlDbType.VarChar).Value = friend;

                numFilasAfectadas += insertarAmigo.ExecuteNonQuery();

                SqlCommand eliminarSolicitud = new SqlCommand(
                    "DELETE FROM USERFRIENDREQUEST WHERE UIDSender = @uid AND UIDReceiver = @friend",
                    conexion, transaccion);

                eliminarSolicitud.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                eliminarSolicitud.Parameters.Add("@friend", SqlDbType.VarChar).Value = friend;

                numFilasAfectadas += eliminarSolicitud.ExecuteNonQuery();

                // Confirmamos la transacción si todo fue bien
                transaccion.Commit();
            }
            catch (Exception)
            {
                // Si hay un error, deshacemos la transacción
                if (transaccion != null)
                {
                    transaccion.Rollback();
                }
                throw;
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return numFilasAfectadas;
        }

        /// <summary>
        /// Esta función recibe dos UIDs y rechaza una solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del emisor</param>
        /// <param name="friend">UID del receptor</param>
        /// <returns>Número de filas afectadas</returns>
        public static int declineRequestDAL(String uid, String friend)
        {
            int numFilasAfectadas = 0;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@uid", System.Data.SqlDbType.VarChar).Value = uid;
                miComando.Parameters.Add("@friend", System.Data.SqlDbType.VarChar).Value = friend;
                miComando.CommandText = "DELETE FROM USERFRIENDREQUEST WHERE UIDSender = @friend AND UIDReceiver = @uid";

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
    
        /// <summary>
        /// Esta función recibe dos UIDs y comprueba si ya se le ha enviado una solicitud de amistad
        /// </summary>
        /// <param name="uid">UID del emisor</param>
        /// <param name="friend">UID del receptor</param>
        /// <returns>Solicitud enviada o no</returns>
        public static bool requestSentDAL(String uid, String friend)
        {
            bool sent = false;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@uid", System.Data.SqlDbType.VarChar).Value = uid;
                miComando.Parameters.Add("@friend", System.Data.SqlDbType.VarChar).Value = friend;
                miComando.CommandText = "SELECT 1 AS 'SENT' FROM USERFRIENDREQUEST WHERE UIDSender = @uid AND UIDReceiver = @friend";

                miLector = miComando.ExecuteReader();

                if (miLector.HasRows)
                {
                    while (miLector.Read())
                    {
                        int total = (int)miLector["SENT"];

                        sent = total == 1;
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

            return sent;
        }

        /// <summary>
        /// Esta función recibe dos UIDs y elimina al amigo
        /// </summary>
        /// <param name="uid">UID del emisor</param>
        /// <param name="friend">UID del receptor</param>
        /// <returns>Número de filas afectadas</returns>
        public static int deleteFriendDAL(String uid, String friend) {
            int numFilasAfectadas = 0;

            SqlCommand miComando = new SqlCommand();
            SqlDataReader miLector;

            try
            {
                miComando.Connection = clsConexion.GetConnection();

                miComando.Parameters.Add("@uid", System.Data.SqlDbType.VarChar).Value = uid;
                miComando.Parameters.Add("@friend", System.Data.SqlDbType.VarChar).Value = friend;
                miComando.CommandText = "DELETE FROM USERFRIENDS WHERE UID = @uid AND UIDFriend = @friend";

                numFilasAfectadas = miComando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                clsConexion.Desconectar();
            }

            return numFilasAfectadas;
        }
    }
}
