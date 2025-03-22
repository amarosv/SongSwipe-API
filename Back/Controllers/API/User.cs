using DAL;
using DTO;
using Entidades;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Back.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class User : ControllerBase
    {
        // GET: api/<User>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<User>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(String id, int page, int limit)
        {
            IActionResult salida;

            PaginatedTracks paginatedTracks;

            if (page == 0) {
                page = 1;
            }

            if (limit == 0)
            {
                limit = 10;
            }

            String baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            try
            {
                paginatedTracks = await Listados.getLikedTracks(id, page, limit, baseUrl);
                if (paginatedTracks == null)
                {
                    salida = NotFound("No se ha encontrado ninguna canción");
                }
                else
                {
                    salida = Ok(paginatedTracks);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // POST api/<User>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<User>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<User>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
