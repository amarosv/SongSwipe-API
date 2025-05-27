using Microsoft.Data.SqlClient;

namespace DAL.Utils
{
    public static class clsConexion
    {
        private static readonly string connectionString =
            "Server=amaro.database.windows.net;Database=AmaroDB;User Id=usuario;Password=LaCampana123;TrustServerCertificate=True;MultipleActiveResultSets=True;";

        /// <summary>
        /// Devuelve una conexión nueva sin abrirla (para usar con 'using')
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
