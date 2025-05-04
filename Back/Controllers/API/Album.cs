using DAL;
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
