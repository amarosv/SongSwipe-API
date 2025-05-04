using DAL;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Back.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class Track : ControllerBase
    {
        // GET: api/<Track>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<Track>/5
        //[HttpGet("{id}")]
        //[SwaggerOperation(
        //    Summary = "Obtiene los datos de una canción específica",
        //    Description = "Este método obtiene todos los datos de una canción especificada por su ID.<br>" +
        //    "Si no se encuentra ninguna canción devuelve un mensaje de error."
        //)]
        //[SwaggerResponse(200, "Canción con sus datos", typeof(Entidades.Track))]
        //[SwaggerResponse(404, "No se ha encontrado ninguna canción con ese ID")]
        //[SwaggerResponse(500, "Error interno del servidor")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    IActionResult salida;

        //    Entidades.Track track = null;

        //    try
        //    {
        //        track = await MetodosTrackDAL.getTrackById(id);

        //        if (track == null)
        //        {
        //            salida = NotFound("No se ha encontrado ninguna canción con ese ID");
        //        }
        //        else
        //        {
        //            salida = Ok(track);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        salida = BadRequest(ex.Message);
        //    }

        //    return salida;
        //}

        // GET api/<Track>/5/likes
        [HttpGet("{id}/likes")]
        [SwaggerOperation(
            Summary = "Obtiene los likes de una canción",
            Description = "Este método obtiene todos los likes de una canción."
        )]
        [SwaggerResponse(200, "Likes de la canción", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetLikesTrack(int id)
        {
            IActionResult salida;

            int likes = 0;

            try
            {
                likes = MetodosTrackDAL.getLikesByTrack(id);

                salida = Ok(likes);
            }
            catch (Exception ex)
            {
                salida = BadRequest(ex.Message);
            }

            return salida;
        }

        // GET api/<Track>/5/dislikes
        [HttpGet("{id}/dislikes")]
        [SwaggerOperation(
            Summary = "Obtiene los dislikes de una canción",
            Description = "Este método obtiene todos los dislikes de una canción."
        )]
        [SwaggerResponse(200, "Dislikes de la canción", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetDislikesTrack(int id)
        {
            IActionResult salida;

            int dislikes = 0;

            try
            {
                dislikes = MetodosTrackDAL.getDislikesByTrack(id);

                salida = Ok(dislikes);
            }
            catch (Exception ex)
            {
                salida = BadRequest(ex.Message);
            }

            return salida;
        }

        //// POST api/<Track>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<Track>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<Track>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
