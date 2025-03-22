using DAL;
using DTO;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Back.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class User : ControllerBase
    {
        // GET: api/<User>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Obtiene un listado con todos los usuarios activos",
            Description = "Este método obtiene todas los usuarios activos (desbloqueados y no eliminados) y los devuelve como un listado.<br>" +
            "Si no se encuentra ningun usuario devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de usarios obtenida correctamente", typeof(List<Usuario>))]
        [SwaggerResponse(404, "No se encontraron usuarios")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult Get()
        {
            IActionResult salida;

            List<Usuario> lista = null;

            try
            {
                lista = ListadosDAL.getAllUsers();

                if (lista == null || lista.Count == 0) {
                    salida = NotFound("No se ha encontrado ningún usuario");
                } else
                {
                    salida = Ok(lista);
                }
            }
            catch (Exception ex) { 
                salida = BadRequest(ex.Message);
            }

            return salida;
        }

        // GET api/<User>/5
        [HttpGet("{uid}")]
        [SwaggerOperation(
            Summary = "Obtiene los datos de un usuario específico",
            Description = "Este método obtiene todos los datos de un usuario especificado por su UID.<br>" +
            "Si no se encuentra ningun usuario devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Usuario con sus datos", typeof(Usuario))]
        [SwaggerResponse(404, "No se ha encontrado ningún usuario con ese UID")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult Get(
            [SwaggerParameter(Description = "UID del usuario a buscar")]
            String uid
        )
        {
            IActionResult salida;

            Usuario user;

            try
            {
                user = MetodosDAL.getUserByUIDDAL(uid);

                if (user == null) {
                    salida = NotFound("No se ha encontrado ningún usuario con ese UID");
                } else
                {
                    salida = Ok(user);
                }
            }
            catch (Exception ex) {
                salida = BadRequest(ex.Message);
            }

            return salida;
        }

        // GET api/<User>/5/liked
        [HttpGet("{uid}/liked")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con todas las canciones marcadas como \"me gusta\" por el usuario",
            Description = "Este método obtiene todas las canciones que el usuario ha marcado como \"me gusta\" y las devuelve como un listado.<br>" +
            "Si no se encuentra ninguna canción devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de canciones obtenida correctamente", typeof(List<PaginatedTracks>))]
        [SwaggerResponse(404, "No se ha encontrado ninguna canción")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetLikedTracks(
            [SwaggerParameter(Description = "UID del usuario que ha marcado las canciones como me gusta.")]
            String uid,
            [SwaggerParameter(Description = "Número de página de la que obtener las canciones")]
            int page,
            [SwaggerParameter(Description = "Número de canciones que se muestran por página")]
            int limit)
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
                paginatedTracks = await ListadosDAL.getLikedTracksDAL(uid, page, limit, baseUrl);
                if (paginatedTracks.Tracks.Count == 0)
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

        // GET api/<User>/5/disliked
        [HttpGet("{uid}/disliked")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con todas las canciones marcadas como \"no me gusta\" por el usuario",
            Description = "Este método obtiene todas las canciones que el usuario ha marcado como \"no me gusta\" y las devuelve como un listado.<br>" +
            "Si no se encuentra ninguna canción devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de canciones obtenida correctamente", typeof(List<PaginatedTracks>))]
        [SwaggerResponse(404, "No se ha encontrado ninguna canción")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetDisLikedTracks(
            [SwaggerParameter(Description = "UID del usuario que ha marcado las canciones como NO me gusta.")]
            String uid,
            [SwaggerParameter(Description = "Número de página de la que obtener las canciones")]
            int page,
            [SwaggerParameter(Description = "Número de canciones que se muestran por página")]
            int limit)
        {
            IActionResult salida;

            PaginatedTracks paginatedTracks;

            if (page == 0)
            {
                page = 1;
            }

            if (limit == 0)
            {
                limit = 10;
            }

            String baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            try
            {
                paginatedTracks = await ListadosDAL.getDisLikedTracksDAL(uid, page, limit, baseUrl);
                if (paginatedTracks.Tracks.Count == 0)
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
        [SwaggerOperation(
            Summary = "Obtiene los datos de un usuario y lo almacena en la base de datos",
            Description = "Este método obtiene todos los datos de un usuario y lo guarda en la base de datos.<br>" +
            "Devuelve el número de filas afectadads"
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult Post([FromBody] Usuario user)
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosDAL.createUser(user);
                if (numFilasAfectadas == 0)
                {
                    salida = NotFound("No se ha podido crear el usuario");
                }
                else
                {
                    salida = Ok("Se ha creado el usuario correctamente");
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // PUT api/<User>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<User>/5
        [HttpDelete("{uid}")]
        [SwaggerOperation(
            Summary = "Obtiene un UID de usuario y lo elimina de la base de datos",
            Description = "Este método obtiene un UID de usuario y lo borra de la base de datos.<br>" +
            "Si no se encuentra ningún usuario devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(404, "No se ha podido eliminar al usuario")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult Delete(
            [SwaggerParameter(Description = "UID del usuario a eliminar")] 
            String uid)
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosDAL.deleteUser(uid);
                if (numFilasAfectadas == 0)
                {
                    salida = NotFound("No se ha podido eliminar al usuario");
                }
                else
                {
                    salida = Ok("Se ha eliminado al usuario correctamente");
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }
    }
}
