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
    public class MetodosUserTrackDAL
    {

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
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Crear parámetros dinámicos para la cláusula IN
                    List<string> paramNames = new List<string>();
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

                    conn.Open();

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
                throw;
            }

            return tracksNotSaved;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario y una lista de Swipes y los inserta en la base de datos
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="swipes">Lista de Swipes</param>
        /// <returns>Número de lineas afectadas</returns>
        public static int saveSwipesDAL(string uid, List<Swipe> swipes)
        {
            int numFilasAfectadas = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    List<string> valuesClauses = new List<string>();

                    for (int i = 0; i < swipes.Count; i++)
                    {
                        string idTrackParam = $"@idTrack{i}";
                        string idAlbumParam = $"@idAlbum{i}";
                        string idArtistParam = $"@idArtist{i}";
                        string swipeParam = $"@swipe{i}";
                        string dateParam = $"@date{i}";

                        valuesClauses.Add($"(@uid, {idTrackParam}, {idAlbumParam}, {idArtistParam}, {swipeParam}, {dateParam})");

                        cmd.Parameters.AddWithValue(idTrackParam, swipes[i].Id);
                        cmd.Parameters.AddWithValue(idAlbumParam, swipes[i].IdAlbum);
                        cmd.Parameters.AddWithValue(idArtistParam, swipes[i].IdArtist);
                        cmd.Parameters.AddWithValue(swipeParam, swipes[i].Like);

                        TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                        DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);
                        cmd.Parameters.AddWithValue(dateParam, cetTime);
                    }

                    cmd.Parameters.AddWithValue("@uid", uid);

                    cmd.CommandText = $@"
                INSERT INTO USERSWIPES (UID, IDTrack, IDAlbum, IDArtist, Swipe, DateSwipe)
                VALUES {string.Join(", ", valuesClauses)}
            ";

                    conn.Open();
                    numFilasAfectadas = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return numFilasAfectadas;
        }
        
        /// <summary>
        /// Esta función recibe el UID de un usuario y el id de una canción y devuelve si el usuario la ha marcado como me gusta
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="idTrack">ID de la canción</param>
        /// <returns>Booleano que indica si se ha marcado como me gusta</returns>
        public static bool isTrackLikedDAL(String uid, long idTrack)
        {
            bool exists = false;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("SELECT 1 FROM USERSWIPES WHERE IDTrack = @IDTrack AND Swipe = 1 AND UID = @uid", conn))
                {
                    cmd.Parameters.Add("@IDTrack", SqlDbType.BigInt).Value = idTrack;
                    cmd.Parameters.Add("@UID", SqlDbType.VarChar).Value = uid;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        exists = reader.HasRows;
                    }
                }
            }
            catch (Exception) { throw; }

            return exists;
        }

        /// <summary>
        /// Esta función recibe el UID de un usuario, un objeto con el ID de la canción y el nuevo valor del like y lo actualiza en la base de datos
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="simpleSwipe">Objeto con el ID de la canción y el nuevo valor del like</param>
        /// <returns>Número de líneas modificadas</returns>
        public static int updateSwipeDAL(string uid, SimpleSwipe simpleSwipe)
        {
            int numFilasAfectadas = 0;

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = new SqlCommand("UPDATE USERSWIPES SET Swipe = @like, DateSwipe = @date WHERE UID = @uid AND IDTrack = @idTrack", conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                    TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                    DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);
                    cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = cetTime;
                    cmd.Parameters.Add("@like", SqlDbType.Int).Value = simpleSwipe.Like;
                    cmd.Parameters.Add("@idTrack", SqlDbType.BigInt).Value = simpleSwipe.Id;

                    numFilasAfectadas = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return numFilasAfectadas;
        }

    }
}
