using DAL.Lists;
using DAL.Methods;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Back.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class Artist : ControllerBase
    {
        // GET api/<Artist>/top_liked_artists
        [HttpGet("top_liked_artists")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con top 10 artistas con más me gustas globales",
            Description = "Este método obtiene el top 10 artistas con más me gustas globales y las devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de artistas obtenida correctamente", typeof(List<Entidades.Artist>))]
        [SwaggerResponse(404, "No se ha encontrado ningun artista")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetTop10LikedArtists()
        {
            IActionResult salida;
            List<Entidades.Artist> artists;

            try
            {
                artists = await ListadosArtistsDAL.getTop10LikedArtistsGlobalDAL();
                if (artists.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún artista");
                }
                else
                {
                    salida = Ok(artists);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<Artist>/top_disliked_artists
        [HttpGet("top_disliked_artists")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con top 10 artistas con más no me gustas globales",
            Description = "Este método obtiene el top 10 artistas con más no me gustas globales y las devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de artistas obtenida correctamente", typeof(List<Entidades.Artist>))]
        [SwaggerResponse(404, "No se ha encontrado ningun artista")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetTop10DislikedArtistsByUser()
        {
            IActionResult salida;
            List<Entidades.Artist> artists;

            try
            {
                artists = await ListadosArtistsDAL.getTop10DislikedArtistsGlobalDAL();
                if (artists.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún artista");
                }
                else
                {
                    salida = Ok(artists);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<Artist>/top_swipes_artists
        [HttpGet("top_swipes_artists")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con top 10 artistas con más swipes globales",
            Description = "Este método obtiene el top 10 artistas con más swipes globales y las devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de artistas obtenida correctamente", typeof(List<Entidades.Artist>))]
        [SwaggerResponse(404, "No se ha encontrado ningun artista")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetTop10SwipesArtistsByUser()
        {
            IActionResult salida;
            List<Entidades.Artist> artists;

            try
            {
                artists = await ListadosArtistsDAL.getTop10SwipesArtistsGlobalDAL();
                if (artists.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún artista");
                }
                else
                {
                    salida = Ok(artists);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<Artist>/1/stats
        [HttpGet("{id}/stats")]
        [SwaggerOperation(
            Summary = "Obtiene las stats de un artista",
            Description = "Este método recibe el ID de un artista y obtiene sus stats<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Stats obtenidas correctamente", typeof(Stats))]
        [SwaggerResponse(404, "No se ha encontrado ningun artista")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetStatsByArtist(
            [SwaggerParameter(Description = "ID del artista")]
            long id
        )
        {
            IActionResult salida;
            Stats stats;

            try
            {
                stats = MetodosArtistDAL.getStatsByArtistDAL(id);
                if (stats == null)
                {
                    salida = NotFound("No se ha encontrado ningún artista");
                }
                else
                {
                    salida = Ok(stats);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<Artist>/1/saved_tracks
        [HttpGet("{id}/saved_tracks")]
        [SwaggerOperation(
            Summary = "Obtiene el número de canciones guardadas de un artista",
            Description = "Este método recibe el ID de un artista y obtiene el número de canciones guardadas (tanto como me gusta como no me gusta)<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Número de canciones guardadas obtenido correctamente", typeof(int))]
        [SwaggerResponse(404, "No se ha encontrado ningun artista")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetSavedTracksByArtist(
            [SwaggerParameter(Description = "ID del artista")]
            long id
        )
        {
            IActionResult salida;
            int total;

            try
            {
                total = MetodosArtistDAL.getSavedSongsByArtistDAL(id);
                salida = Ok(total);
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

    }
}
