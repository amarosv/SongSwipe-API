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
        [SwaggerResponse(404, "No se ha encontrado ningún usuario")]
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

        // GET: api/<User>/username/username
        [HttpGet("username/{username}")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con todos los usuarios activos cuyo username concuerde con el especificado",
            Description = "Este método obtiene todas los usuarios activos (desbloqueados y no eliminados) cuyo username concuerde con el especificado y los devuelve como un listado.<br>" +
            "Si no se encuentra ningun usuario devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de usarios obtenida correctamente", typeof(List<Usuario>))]
        [SwaggerResponse(404, "No se ha encontrado ningún usuario con ese username")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetByUsername(String username)
        {
            IActionResult salida;

            List<Usuario> lista = null;

            try
            {
                lista = ListadosDAL.getUsersByUsername(username);

                if (lista == null || lista.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún usuario con ese username");
                }
                else
                {
                    salida = Ok(lista);
                }
            }
            catch (Exception ex)
            {
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

        // GET api/<User>/5/friends
        [HttpGet("{uid}/friends")]
        [SwaggerOperation(
            Summary = "Obtiene los amigos de un usuario",
            Description = "Este método obtiene todos los amigos de un usuario especificado por su UID.<br>" +
            "Si no se encuentra ningun amigo se devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de amigos", typeof(List<Usuario>))]
        [SwaggerResponse(404, "No se ha encontrado ningún amigo para ese usuario")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetFriends(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid
        )
        {
            IActionResult salida;

            List<Usuario> usuarios;

            try
            {
                usuarios = ListadosDAL.getFriendsDAL(uid);

                if (usuarios == null || usuarios.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún amigo para ese usuario");
                }
                else
                {
                    salida = Ok(usuarios);
                }
            }
            catch (Exception ex)
            {
                salida = BadRequest(ex.Message);
            }

            return salida;
        }

        // GET api/<User>/5/inrequests
        [HttpGet("{uid}/inrequests")]
        [SwaggerOperation(
            Summary = "Obtiene las solicitudes de amistad entrantes",
            Description = "Este método obtiene todas las solicitudes de amistad entrantes de un usuario y las devuelve como una lista.<br>" +
            "Si no se encuentra ninguna solicitud entrante se devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de solicitudes de amistad entrante", typeof(List<Usuario>))]
        [SwaggerResponse(404, "No se ha encontrado ninguna solicitud de amistad entrante para ese usuario")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetIncomingFriendRequests(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid
        )
        {
            IActionResult salida;

            List<Usuario> usuarios;

            try
            {
                usuarios = ListadosDAL.getIncomingFriendRequestsDAL(uid);

                if (usuarios == null || usuarios.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ninguna solicitud de amistad entrante para ese usuario");
                }
                else
                {
                    salida = Ok(usuarios);
                }
            }
            catch (Exception ex)
            {
                salida = BadRequest(ex.Message);
            }

            return salida;
        }

        // GET api/<User>/5/outrequests
        [HttpGet("{uid}/outrequests")]
        [SwaggerOperation(
            Summary = "Obtiene las solicitudes de amistad entrantes",
            Description = "Este método obtiene todas las solicitudes de amistad enviadas de un usuario y las devuelve como una lista.<br>" +
            "Si no se encuentra ninguna solicitud saliente se devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de solicitudes de amistad enviadas", typeof(List<Usuario>))]
        [SwaggerResponse(404, "No se ha encontrado ninguna solicitud de amistad saliente para ese usuario")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetOutgoingFriendRequests(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid
        )
        {
            IActionResult salida;

            List<Usuario> usuarios;

            try
            {
                usuarios = ListadosDAL.getOutgoingFriendRequestsDAL(uid);

                if (usuarios == null || usuarios.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ninguna solicitud de amistad saliente para ese usuario");
                }
                else
                {
                    salida = Ok(usuarios);
                }
            }
            catch (Exception ex)
            {
                salida = BadRequest(ex.Message);
            }

            return salida;
        }

        // GET api/<User>/5/friendslikedtrack/1
        [HttpGet("{uid}/friendslikedtrack/{idTrack}")]
        [SwaggerOperation(
            Summary = "Obtiene los amigos que le han dado \"me gusta\" a la canción",
            Description = "Este método obtiene todos los amigos del usuario que le han dado \"me gusta\" a la canción y los devuelve como una lista.<br>" +
            "Si no se encuentra ningun amigo que le haya dado \"me gusta\" se devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de amigos", typeof(List<Usuario>))]
        [SwaggerResponse(404, "No se ha encontrado ningún amigo que le haya dado \"me gusta\" a esta canción")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetFriendsWhoLikedTrack(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "ID de la canción")]
            long idTrack
        )
        {
            IActionResult salida;

            List<Usuario> usuarios;

            try
            {
                usuarios = ListadosDAL.getFriendsWhoLikedTrackDAL(uid, idTrack);

                if (usuarios == null || usuarios.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún amigo que le haya dado \"me gusta\" a esta canción");
                }
                else
                {
                    salida = Ok(usuarios);
                }
            }
            catch (Exception ex)
            {
                salida = BadRequest(ex.Message);
            }

            return salida;
        }

        // GET api/<User>/5/blocked
        [HttpGet("{uid}/blocked")]
        [SwaggerOperation(
            Summary = "Obtiene los usuarios que el usuario ha bloqueado",
            Description = "Este método obtiene todos los usuarios que el usuario ha bloqueado y los devuelve como una lista.<br>" +
            "Si no se encuentra ningun usuario bloqueado se devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de usuarios bloqueados", typeof(List<Usuario>))]
        [SwaggerResponse(404, "No se ha encontrado ningún usuario bloqueado")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetBlocked(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid
        )
        {
            IActionResult salida;

            List<Usuario> usuarios;

            try
            {
                usuarios = ListadosDAL.getUsersBlockedDAL(uid);

                if (usuarios == null || usuarios.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún usuario bloqueado");
                }
                else
                {
                    salida = Ok(usuarios);
                }
            }
            catch (Exception ex)
            {
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

        // GET api/<User>/check-username
        [HttpGet("check-username/{username}")]
        [SwaggerOperation(
            Summary = "Comprueba que no exista el username",
            Description = "Este método obtiene un username y comprueba que no exista en la base de datos<br>" +
            "Devuelve un boolean"
        )]
        [SwaggerResponse(200, "Comprobado", typeof(bool))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetCheckUsername(String username)
        {
            IActionResult salida;

            if (!string.IsNullOrEmpty(username))
            {
                bool exists = MetodosDAL.checkUsername(username);
                
                salida = Ok(exists);
            } else
            {
                salida = BadRequest("Username no válido");
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

        // POST api/<User>/5/artists
        [HttpPost("{uid}/artists")]
        [SwaggerOperation(
            Summary = "Obtiene el UID de un usuario y una lista de ids de artistas y lo guarda en la base de datos",
            Description = "Este método obtiene un UID de un usuario y una lista de ids de artistas y lo guarda en la base de datos.<br>" +
            "Devuelve el número de filas afectadads"
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult PostArtistas(String uid, [FromBody] List<int> artistas)
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosDAL.addArtistsToFavorites(uid, artistas);
                if (numFilasAfectadas == 0)
                {
                    salida = NotFound("No se ha podido añadir a los artistas como favoritos");
                }
                else
                {
                    salida = Ok(numFilasAfectadas);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // POST api/<User>/5/genres
        [HttpPost("{uid}/genres")]
        [SwaggerOperation(
            Summary = "Obtiene el UID de un usuario y una lista de ids de géneros y lo guarda en la base de datos",
            Description = "Este método obtiene un UID de un usuario y una lista de ids de géneros y lo guarda en la base de datos.<br>" +
            "Devuelve el número de filas afectadads"
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult PostGeneros(String uid, [FromBody] List<int> generos)
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosDAL.addGenresToFavorites(uid, generos);
                if (numFilasAfectadas == 0)
                {
                    salida = NotFound("No se ha podido añadir los géneros como favoritos");
                }
                else
                {
                    salida = Ok(numFilasAfectadas);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }


        // PUT api/<User>/5
        [HttpPut("{uid}")]
        [SwaggerOperation(
            Summary = "Obtiene un UID de usuario, un usuario actualizado y lo actualiza en la base de datos",
            Description = "Este método obtiene un UID de usuario, el usuario actualizado y lo actualiza de la base de datos.<br>" +
            "Si no se ha podido actualizar devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(404, "No se ha podido actualizar al usuario")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult Put(
            [SwaggerParameter(Description = "UID del usuario a actualizar")]
            String uid,
            [SwaggerParameter(Description = "Usuario actualizado")]
            [FromBody] Usuario user)
        {
            IActionResult salida;

            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosDAL.updateUser(user);

                if (numFilasAfectadas == 0)
                {
                    salida = NotFound("No se ha podido actualizar al usuario");
                }
                else
                {
                    salida = Ok(user);
                }
            }
            catch (Exception e) {
                salida = BadRequest(e.Message);
            }

            return salida;
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
