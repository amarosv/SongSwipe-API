using DAL.Utils;
using DTO;
using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Lists
{
    public class ListadosUserGenresDAL
    {

        /// <summary>
        /// Esta función recibe el uid de un usuario, obtiene los géneros que ha marcado como favorito
        /// y los devuelve como una lista
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <param name="page">Número de página</param>
        /// <param name="limit">Número de artistas por página</param>
        /// <returns>Objeto con información y lista de géneros que ha marcado como favorito</returns>
        public static async Task<PaginatedGenres> getFavGenresDAL(string uid, int page, int limit, string baseUrl)
        {
            PaginatedGenres paginatedGenres = new PaginatedGenres();
            List<Genre> genres = new List<Genre>();

            // Primero obtenemos los ids de los géneros de la base de datos
            var result = Utils.Methods.getListIdsDAL("EXEC GetFavoriteGenres @UID, @page, @limit", uid, page, limit);

            // Calcular total de géneros
            int offset = (page - 1) * limit + 1;

            if (page > 1)
            {
                paginatedGenres.LinkPreviousPage = $"{baseUrl}?page={page - 1}&limit={limit}";
            }

            if (page < result.result.totalPages)
            {
                paginatedGenres.LinkNextPage = $"{baseUrl}?page={page + 1}&limit={limit}";
            }

            // Crear una lista de tareas para manejar las peticiones de forma concurrente
            List<Task<Genre>> genreTasks = new List<Task<Genre>>();

            // Se añaden las tareas a la lista
            foreach (long genreId in result.result.list)
            {
                // Verificamos si el género ya está en el cache
                if (DeezerCache.TryGetGenre(genreId, out Genre cachedGenre))
                {
                    genres.Add(cachedGenre); // Si está en el cache, lo agregamos directamente
                }
                else
                {
                    genreTasks.Add(CallApiDeezer.HandleRateLimitAndGetGenre(genreId)); // Si no está, la solicitamos
                }
            }

            // Esperamos que todas las tareas se completen
            var genreResults = await Task.WhenAll(genreTasks);

            // Agregamos los resultados a la lista final y los almacenamos en el cache
            foreach (var genre in genreResults.Where(t => t != null))
            {
                DeezerCache.AddGenre(genre.Id, genre); // Guardamos el género en el cache
                genres.Add(genre);
            }

            // Asignar valores al objeto paginado
            paginatedGenres.Page = page;
            paginatedGenres.TotalPages = result.result.totalPages;
            paginatedGenres.TotalGenres = result.total;
            paginatedGenres.Offset = offset;
            paginatedGenres.Last = offset + genres.Count - 1;
            paginatedGenres.Limit = limit;
            paginatedGenres.Genres = genres;

            return paginatedGenres;
        }

        /// <summary>
        /// Esta función recibe el uid de un usuario y devuelve una lista con los ids de los géneros que sigue el usuario
        /// </summary>
        /// <param name="uid">UID del usuario</param>
        /// <returns>Lista de ids de los artistas</returns>
        public static List<long> getUserFavoriteGenresIdsDAL(string uid)
        {
            List<long> genresIds = new();

            try
            {
                using (SqlConnection conn = clsConexion.GetConnection())
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT IDGenre FROM USERGENRES WHERE UID = @UID";
                    cmd.Parameters.Add("@UID", SqlDbType.VarChar).Value = uid;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = (long)reader["IDGenre"];
                            if (id > 0)
                                genresIds.Add(id);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return genresIds;
        }

    }
}
