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
    public class Album : ControllerBase
    {
        //// GET: api/<Album>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<Album>/5
        //[HttpGet("{id}")]
        //[SwaggerOperation(
        //    Summary = "Obtiene los datos de un álbum específico",
        //    Description = "Este método obtiene todos los datos de un álbum especificado por su ID.<br>" +
        //    "Si no se encuentra ningún álbum devuelve un mensaje de error."
        //)]
        //[SwaggerResponse(200, "Álbum con sus datos", typeof(Entidades.Album))]
        //[SwaggerResponse(404, "No se ha encontrado ningún álbum con ese ID")]
        //[SwaggerResponse(500, "Error interno del servidor")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    IActionResult salida;

        //    Entidades.Album album = null;

        //    try
        //    {
        //        album = await MetodosAlbumDAL.getAlbumById(id);

        //        if (album == null)
        //        {
        //            salida = NotFound("No se ha encontrado ningún álbum con ese ID");
        //        }
        //        else
        //        {
        //            salida = Ok(album);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        salida = BadRequest(ex.Message);
        //    }

        //    return salida;
        //}

        // GET api/<Album>/5/likes
        [HttpGet("{id}/likes")]
        [SwaggerOperation(
            Summary = "Obtiene los likes de un álbum",
            Description = "Este método obtiene todos los likes de un álbum."
        )]
        [SwaggerResponse(200, "Likes del álbum", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetLikesAlbum(int id)
        {
            IActionResult salida;

            int likes = 0;

            try
            {
                likes = MetodosAlbumDAL.getLikesByAlbum(id);

                salida = Ok(likes);
            }
            catch (Exception ex)
            {
                salida = BadRequest(ex.Message);
            }

            return salida;
        }

        // GET api/<Album>/5/dislikes
        [HttpGet("{id}/dislikes")]
        [SwaggerOperation(
            Summary = "Obtiene los dislikes de un álbum",
            Description = "Este método obtiene todos los dislikes de un álbum."
        )]
        [SwaggerResponse(200, "Dislikes del álbum", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetDislikesAlbum(int id)
        {
            IActionResult salida;

            int dislikes = 0;

            try
            {
                dislikes = MetodosAlbumDAL.getDislikesByAlbum(id);

                salida = Ok(dislikes);
            }
            catch (Exception ex)
            {
                salida = BadRequest(ex.Message);
            }

            return salida;
        }

        // GET api/<Album>/1/stats
        [HttpGet("{id}/stats")]
        [SwaggerOperation(
            Summary = "Obtiene las stats de un album",
            Description = "Este método recibe el ID de un album y obtiene sus stats<br>" +
            "Si no se encuentra ningún album devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Stats obtenidas correctamente", typeof(Stats))]
        [SwaggerResponse(404, "No se ha encontrado ningun album")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetStatsByAlbum(
            [SwaggerParameter(Description = "ID del album")]
            long id
        )
        {
            IActionResult salida;
            Stats stats;

            try
            {
                stats = MetodosAlbumDAL.getStatsByAlbumDAL(id);
                if (stats == null)
                {
                    salida = NotFound("No se ha encontrado ningún álbum");
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

        // GET api/<Album>/top_liked_albums
        [HttpGet("top_liked_albums")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con top 10 albumes con más me gustas globales",
            Description = "Este método obtiene el top 10 albumes con más me gustas globales y las devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de artistas obtenida correctamente", typeof(List<Entidades.Album>))]
        [SwaggerResponse(404, "No se ha encontrado ningun álbum")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetTop10LikedArtists()
        {
            IActionResult salida;
            List<Entidades.Album> albums;

            try
            {
                albums = await ListadosAlbumsDAL.getTop10LikedAlbumsGlobalDAL();
                if (albums.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún álbum");
                }
                else
                {
                    salida = Ok(albums);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<Album>/top_disliked_albums
        [HttpGet("top_disliked_albums")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con top 10 albumes con más no me gustas globales",
            Description = "Este método obtiene el top 10 albumes con más no me gustas globales y las devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de albumes obtenida correctamente", typeof(List<Entidades.Album>))]
        [SwaggerResponse(404, "No se ha encontrado ningun álbum")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetTop10DislikedAlbumsByUser()
        {
            IActionResult salida;
            List<Entidades.Album> albums;

            try
            {
                albums = await ListadosAlbumsDAL.getTop10DislikedAlbumsGlobalDAL();
                if (albums.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún álbum");
                }
                else
                {
                    salida = Ok(albums);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<Album>/top_swipes_albums
        [HttpGet("top_swipes_albums")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con top 10 albumes con más swipes globales",
            Description = "Este método obtiene el top 10 albumes con más swipes globales y las devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de albumes obtenida correctamente", typeof(List<Entidades.Album>))]
        [SwaggerResponse(404, "No se ha encontrado ninguna álbum")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetTop10SwipesAlbumsByUser()
        {
            IActionResult salida;
            List<Entidades.Album> albums;

            try
            {
                albums = await ListadosAlbumsDAL.getTop10SwipesAlbumsGlobalDAL();
                if (albums.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún álbum");
                }
                else
                {
                    salida = Ok(albums);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        //// POST api/<Album>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<Album>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<Album>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
