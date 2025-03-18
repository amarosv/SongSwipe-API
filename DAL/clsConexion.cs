using Microsoft.Data.SqlClient;

namespace DAL
{
    public class clsConexion
    {
        /// <summary>
        /// Metodo para obtener la conexion a la base de datos de azure
        /// </summary>
        /// <returns>Devuelve la conexion a la base de datos</returns>
        public static SqlConnection GetConnection()
        {
            SqlConnection miConexion = new SqlConnection();

            try
            {
                miConexion.ConnectionString = miConexion.ConnectionString = "server=amaro.database.windows.net;database=AmaroDB;uid=usuario;pwd=LaCampana123;trustServerCertificate=true;MultipleActiveResultSets=True;";

                miConexion.Open();
            }
            catch (Exception e)
            {
                throw;
            }

            return miConexion;
        }

        /// <summary>
        /// Metodo para desconectar de la base de datos de azure
        /// </summary>
        /// <returns>Devuelve la conexion a la base de datos</returns>
        public static SqlConnection Desconectar()
        {
            SqlConnection miConexion = new SqlConnection();

            try
            {

                miConexion.ConnectionString = miConexion.ConnectionString = "server=amaro.database.windows.net;database=AmaroDB;uid=usuario;pwd=LaCampana123;trustServerCertificate=true;MultipleActiveResultSets=True;";

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                miConexion.Close();
            }

            return miConexion;
        }
    }
}