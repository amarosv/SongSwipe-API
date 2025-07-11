﻿using DAL.Lists;
using DAL.Methods;
using DTO;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

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
                lista = ListadosUserDAL.getAllUsers();

                if (lista == null || lista.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún usuario");
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
                lista = ListadosUserDAL.getUsersByUsername(username);

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
                user = MetodosUserDAL.getUserByUIDDAL(uid);

                if (user == null)
                {
                    salida = NotFound("No se ha encontrado ningún usuario con ese UID");
                }
                else
                {
                    salida = Ok(user);
                }
            }
            catch (Exception ex)
            {
                salida = BadRequest(ex.Message);
            }

            return salida;
        }

        // GET api/<User>/email/a@a.com
        [HttpGet("email/{email}")]
        [SwaggerOperation(
            Summary = "Obtiene los datos de un usuario específico",
            Description = "Este método obtiene todos los datos de un usuario especificado por su Email.<br>" +
            "Si no se encuentra ningun usuario devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Usuario con sus datos", typeof(Usuario))]
        [SwaggerResponse(404, "No se ha encontrado ningún usuario con ese Email")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetUserByEmail(
            [SwaggerParameter(Description = "Email del usuario a buscar")]
            String email
        )
        {
            IActionResult salida;

            Usuario user;

            try
            {
                user = MetodosUserDAL.getUserByEmailDAL(email);

                if (user == null)
                {
                    salida = NotFound("No se ha encontrado ningún usuario con ese Email");
                }
                else
                {
                    salida = Ok(user);
                }
            }
            catch (Exception ex)
            {
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
                usuarios = ListadosUserDAL.getFriendsDAL(uid);

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
                usuarios = ListadosUserDAL.getIncomingFriendRequestsDAL(uid);

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
            Summary = "Obtiene las solicitudes de amistad salientes",
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
                usuarios = ListadosUserDAL.getOutgoingFriendRequestsDAL(uid);

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
                usuarios = ListadosUserDAL.getFriendsWhoLikedTrackDAL(uid, idTrack);

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
                usuarios = ListadosUserDAL.getUsersBlockedDAL(uid);

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
            [SwaggerParameter(Description = "Número de canciones que se muestran por página (Máximo 50)")]
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
            else if (limit > 50)
            {
                limit = 50;
            }

            String baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            try
            {
                paginatedTracks = await ListadosUserTrackDAL.getLikedTracksDAL(uid, page, limit, baseUrl);
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
            [SwaggerParameter(Description = "Número de canciones que se muestran por página (Máximo 50)")]
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
            else if (limit > 50)
            {
                limit = 50;
            }

            String baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            try
            {
                paginatedTracks = await ListadosUserTrackDAL.getDisLikedTracksDAL(uid, page, limit, baseUrl);
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

        // GET api/<User>/5/last_swipes
        [HttpGet("{uid}/last_swipes")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con los últimos 5 swipes del usuario",
            Description = "Este método obtiene los últimos 5 swipes del usuario y las devuelve como un listado.<br>" +
            "Si no se encuentra ninguna canción devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de canciones obtenida correctamente", typeof(List<Entidades.Track>))]
        [SwaggerResponse(404, "No se ha encontrado ninguna canción")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetLast5Swipes(
            [SwaggerParameter(Description = "UID del usuario.")]
            String uid)
        {
            IActionResult salida;
            List<Entidades.Track> tracks;

            try
            {
                tracks = await ListadosUserTrackDAL.getLast5SwipesDAL(uid);
                if (tracks.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ninguna canción");
                }
                else
                {
                    salida = Ok(tracks);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/top_liked_artists
        [HttpGet("{uid}/top_liked_artists")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con top 10 artistas con más me gustas del usuario",
            Description = "Este método obtiene el top 10 artistas con más me gustas del usuario y las devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de artistas obtenida correctamente", typeof(List<Entidades.Artist>))]
        [SwaggerResponse(404, "No se ha encontrado ninguna canción")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetTop10LikedArtistsByUser(
            [SwaggerParameter(Description = "UID del usuario.")]
            String uid)
        {
            IActionResult salida;
            List<Entidades.Artist> artists;

            try
            {
                artists = await ListadosUserArtistDAL.getTop10LikedArtistsByUserDAL(uid);
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

        // GET api/<User>/5/top_disliked_artists
        [HttpGet("{uid}/top_disliked_artists")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con top 10 artistas con más no me gustas del usuario",
            Description = "Este método obtiene el top 10 artistas con más no me gustas del usuario y las devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de artistas obtenida correctamente", typeof(List<Entidades.Artist>))]
        [SwaggerResponse(404, "No se ha encontrado ninguna canción")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetTop10DislikedArtistsByUser(
            [SwaggerParameter(Description = "UID del usuario.")]
            String uid)
        {
            IActionResult salida;
            List<Entidades.Artist> artists;

            try
            {
                artists = await ListadosUserArtistDAL.getTop10DislikedArtistsByUserDAL(uid);
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

        // GET api/<User>/5/top_swipes_artists
        [HttpGet("{uid}/top_swipes_artists")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con top 10 artistas con más swipes del usuario",
            Description = "Este método obtiene el top 10 artistas con más swipes del usuario y las devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de artistas obtenida correctamente", typeof(List<Entidades.Artist>))]
        [SwaggerResponse(404, "No se ha encontrado ninguna canción")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetTop10SwipesArtistsByUser(
            [SwaggerParameter(Description = "UID del usuario.")]
            String uid)
        {
            IActionResult salida;
            List<Entidades.Artist> artists;

            try
            {
                artists = await ListadosUserArtistDAL.getTop10SwipesArtistsByUserDAL(uid);
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

        // GET api/<User>/5/top_liked_albums
        [HttpGet("{uid}/top_liked_albums")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con top 10 albumes con más me gustas del usuario",
            Description = "Este método obtiene el top 10 albumes con más me gustas del usuario y las devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de albumes obtenida correctamente", typeof(List<Entidades.Album>))]
        [SwaggerResponse(404, "No se ha encontrado ninguna álbum")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetTop10LikedAlbumsByUser(
            [SwaggerParameter(Description = "UID del usuario.")]
            String uid)
        {
            IActionResult salida;
            List<Entidades.Album> albums;

            try
            {
                albums = await ListadosUserAlbumDAL.getTop10LikedAlbumsByUserDAL(uid);
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

        // GET api/<User>/5/top_disliked_albums
        [HttpGet("{uid}/top_disliked_albums")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con top 10 albumes con más no me gustas del usuario",
            Description = "Este método obtiene el top 10 albumes con más no me gustas del usuario y las devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de albumes obtenida correctamente", typeof(List<Entidades.Album>))]
        [SwaggerResponse(404, "No se ha encontrado ninguna álbum")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetTop10DislikedAlbumsByUser(
            [SwaggerParameter(Description = "UID del usuario.")]
            String uid)
        {
            IActionResult salida;
            List<Entidades.Album> albums;

            try
            {
                albums = await ListadosUserAlbumDAL.getTop10DislikedAlbumsByUserDAL(uid);
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

        // GET api/<User>/5/top_swipes_albums
        [HttpGet("{uid}/top_swipes_albums")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con top 10 albumes con más swipes del usuario",
            Description = "Este método obtiene el top 10 albumes con más swipes del usuario y las devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de albumes obtenida correctamente", typeof(List<Entidades.Album>))]
        [SwaggerResponse(404, "No se ha encontrado ninguna álbum")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetTop10SwipesAlbumsByUser(
            [SwaggerParameter(Description = "UID del usuario.")]
            String uid)
        {
            IActionResult salida;
            List<Entidades.Album> albums;

            try
            {
                albums = await ListadosUserAlbumDAL.getTop10SwipesAlbumsByUserDAL(uid);
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


        // GET api/<User>/5/artists
        [HttpGet("{uid}/artists")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con todos los artistas seguidos por el usuario",
            Description = "Este método obtiene todos los artistas seguidos y los devuelve como un listado.<br>" +
            "Si no se encuentra ningún artista devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de artistas obtenida correctamente", typeof(List<PaginatedArtists>))]
        [SwaggerResponse(404, "No se ha encontrado ningún artista")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetFavoriteArtists(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "Número de página de la que obtener los artistas")]
            int page,
            [SwaggerParameter(Description = "Número de artistas que se muestran por página")]
            int limit)
        {
            IActionResult salida;

            PaginatedArtists paginatedArtists;

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
                paginatedArtists = await ListadosUserArtistDAL.getFavArtistsDAL(uid, page, limit, baseUrl);
                if (paginatedArtists.Artists.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún artista");
                }
                else
                {
                    salida = Ok(paginatedArtists);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/genres
        [HttpGet("{uid}/genres")]
        [SwaggerOperation(
            Summary = "Obtiene un listado con todos los géneros que el usuario ha marcado como favoritos",
            Description = "Este método obtiene todos los géneros que el usuario ha marcado como favoritos y los devuelve como un listado.<br>" +
            "Si no se encuentra ningún género devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Lista de géneros obtenida correctamente", typeof(List<PaginatedGenres>))]
        [SwaggerResponse(404, "No se ha encontrado ningún artista")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> GetFavoriteGenres(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "Número de página de la que obtener los géneros")]
            int page,
            [SwaggerParameter(Description = "Número de géneros que se muestran por página")]
            int limit)
        {
            IActionResult salida;

            PaginatedGenres paginatedGenres;

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
                paginatedGenres = await ListadosUserGenresDAL.getFavGenresDAL(uid, page, limit, baseUrl);
                if (paginatedGenres.Genres.Count == 0)
                {
                    salida = NotFound("No se ha encontrado ningún género");
                }
                else
                {
                    salida = Ok(paginatedGenres);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/check-username/asasas
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

            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    bool exists = MetodosUserDAL.checkUsernameDAL(username);

                    salida = Ok(exists);
                }
                else
                {
                    salida = BadRequest("Username no válido");
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/check-email/a@gmail.com
        [HttpGet("check-email/{email}")]
        [SwaggerOperation(
            Summary = "Comprueba que no exista el email",
            Description = "Este método obtiene un email y comprueba que no exista en la base de datos<br>" +
            "Devuelve un boolean"
        )]
        [SwaggerResponse(200, "Comprobado", typeof(bool))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetCheckEmail(String email)
        {
            IActionResult salida;

            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    bool exists = MetodosUserDAL.checkEmailDAL(email);

                    salida = Ok(exists);
                }
                else
                {
                    salida = BadRequest("Email no válido");
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/profile
        [HttpGet("{uid}/profile")]
        [SwaggerOperation(
            Summary = "Obtiene los datos a mostrar en la pantalla de perfil de usuario",
            Description = "Este método obtiene los datos a mostrar en la pantalla de perfil de usuario<br>" +
            "Si no se encuentran datos, devuelve un mensaje de error"
        )]
        [SwaggerResponse(200, "Usuario con los datos a mostrar", typeof(UserProfile))]
        [SwaggerResponse(404, "No se han encontrado datos")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetUserProfileData(
            [SwaggerParameter(Description = "UID del usuario a obtener sus datos")]
            String uid
        )
        {
            IActionResult salida;
            UserProfile userProfile = null;

            try
            {
                userProfile = MetodosUserDAL.getUserProfileDataDAL(uid);

                if (userProfile == null)
                {
                    salida = NotFound("No se han encontrado datos");
                }
                else
                {
                    salida = Ok(userProfile);
                }

            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/settings
        [HttpGet("{uid}/settings")]
        [SwaggerOperation(
            Summary = "Obtiene los ajustes de usuario",
            Description = "Este método obtiene los ajustes de usuario<br>" +
            "Si no se encuentran ajustes, devuelve un mensaje de error"
        )]
        [SwaggerResponse(200, "Ajustes del usuario", typeof(Settings))]
        [SwaggerResponse(404, "No se han encontrado ajustes")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetUserSettings(
            [SwaggerParameter(Description = "UID del usuario a obtener sus ajustes")]
            String uid
        )
        {
            IActionResult salida;
            Settings settings = null;

            try
            {
                settings = MetodosUserDAL.getUserSettingsDAL(uid);

                if (settings == null)
                {
                    salida = NotFound("No se han encontrado ajustes");
                }
                else
                {
                    salida = Ok(settings);
                }

            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/artists_ids
        [HttpGet("{uid}/artists_ids")]
        [SwaggerOperation(
            Summary = "Obtiene los ids de los artistas que el usuario sigue",
            Description = "Este método obtiene los ids de los artistas que sigue el usuario<br>" +
            "Si no se encuentran artistas, devuelve un mensaje de error"
        )]
        [SwaggerResponse(200, "Ids de los artistas", typeof(List<long>))]
        [SwaggerResponse(404, "No se han encontrado artistas")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetUserFavoriteArtistsIds(
            [SwaggerParameter(Description = "UID del usuario a obtener los ids de los artistas que sigue")]
            String uid
        )
        {
            IActionResult salida;
            List<long> artistsIds = null;

            try
            {
                artistsIds = ListadosUserArtistDAL.getUserFavoriteArtistsIdsDAL(uid);

                if (artistsIds.IsNullOrEmpty())
                {
                    salida = NotFound("No se han encontrado artistas");
                }
                else
                {
                    salida = Ok(artistsIds);
                }

            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/genres_ids
        [HttpGet("{uid}/genres_ids")]
        [SwaggerOperation(
            Summary = "Obtiene los ids de los géneros que el usuario sigue",
            Description = "Este método obtiene los ids de los géneros que sigue el usuario<br>" +
            "Si no se encuentran géneros, devuelve un mensaje de error"
        )]
        [SwaggerResponse(200, "Ids de los géneros", typeof(List<long>))]
        [SwaggerResponse(404, "No se han encontrado géneros")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetUserFavoriteGenresIds(
            [SwaggerParameter(Description = "UID del usuario a obtener los ids de los géneros que sigue")]
            String uid
        )
        {
            IActionResult salida;
            List<long> genresIds = null;

            try
            {
                genresIds = ListadosUserGenresDAL.getUserFavoriteGenresIdsDAL(uid);

                if (genresIds.IsNullOrEmpty())
                {
                    salida = NotFound("No se han encontrado géneros");
                }
                else
                {
                    salida = Ok(genresIds);
                }

            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/is_my_friend/6
        [HttpGet("{uid}/is_my_friend/{friend}")]
        [SwaggerOperation(
            Summary = "Obtiene si son amigos",
            Description = "Este método recibe dos UIDs y comprueba si son amigos"
        )]
        [SwaggerResponse(200, "Son o no amigos", typeof(bool))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetIsUserFriend(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "UID del amigo")]
            String friend
        )
        {
            IActionResult salida;
            bool isFriend = false;

            try
            {
                isFriend = MetodosUserDAL.isMyFriendDAL(uid, friend);

                salida = Ok(isFriend);

            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/following/6
        [HttpGet("{uid}/following/{followed}")]
        [SwaggerOperation(
            Summary = "Obtiene si lo sigue",
            Description = "Este método recibe dos UIDs y comprueba si los igue"
        )]
        [SwaggerResponse(200, "Son o no amigos", typeof(bool))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetIsFollowing(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "UID del seguido")]
            String followed
        )
        {
            IActionResult salida;
            bool following = false;

            try
            {
                following = MetodosUserDAL.followingDAL(uid, followed);

                salida = Ok(following);

            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/request_sent/6
        [HttpGet("{uid}/request_sent/{friend}")]
        [SwaggerOperation(
            Summary = "Obtiene si se le ha mandado una solicitud de amistad",
            Description = "Este método recibe dos UIDs y comprueba si se le ha mandado la solicitud de amistad"
        )]
        [SwaggerResponse(200, "Se ha enviado solicitud", typeof(bool))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetIsRequestSent(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "UID del amigo")]
            String friend
        )
        {
            IActionResult salida;
            bool isFriend = false;

            try
            {
                isFriend = MetodosUserDAL.requestSentDAL(uid, friend);

                salida = Ok(isFriend);

            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/list_sent_requests
        [HttpGet("{uid}/list_sent_requests")]
        [SwaggerOperation(
            Summary = "Obtiene una lista de los usuarios a los que les ha mandado una solicitud de amistad",
            Description = "Este método recibe un UID y devuelve una lista con los usuarios a los que les ha mandado una solicitu de amistad." +
            "Si no se encuentran solicitudes devuelve un mensaje de error"
        )]
        [SwaggerResponse(200, "Lista de usuarios", typeof(List<Usuario>))]
        [SwaggerResponse(404, "No se han encontrado solicitudes")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetListSentRequests(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid
        )
        {
            IActionResult salida;
            List<Usuario> usuarios;

            try
            {
                usuarios = ListadosUserDAL.getSentRequestDAL(uid);

                if (usuarios.IsNullOrEmpty())
                {
                    salida = NotFound("No se han encontrado solicitudes");
                } else
                {
                    salida = Ok(usuarios);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/list_receive_requests
        [HttpGet("{uid}/list_receive_requests")]
        [SwaggerOperation(
            Summary = "Obtiene una lista de los usuarios que le han mandado una solicitud de amistad",
            Description = "Este método recibe un UID y devuelve una lista con los usuarios que le han mandado una solicitu de amistad." +
            "Si no se encuentran solicitudes devuelve un mensaje de error"
        )]
        [SwaggerResponse(200, "Lista de usuarios", typeof(List<Usuario>))]
        [SwaggerResponse(404, "No se han encontrado solicitudes")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetListReceiveRequests(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid
        )
        {
            IActionResult salida;
            List<Usuario> usuarios;

            try
            {
                usuarios = ListadosUserDAL.getReceiveRequestDAL(uid);

                if (usuarios.IsNullOrEmpty())
                {
                    salida = NotFound("No se han encontrado solicitudes");
                }
                else
                {
                    salida = Ok(usuarios);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/is_track_liked/7
        [HttpGet("{uid}/is_track_liked/{idTrack}")]
        [SwaggerOperation(
            Summary = "Obtiene si la canción está marcada como me gusta",
            Description = "Este método recibe un UID y un ID de canción y devuelve si el usuario la ha marcado como me gusta." +
            "Si no se encuentra la canción devuelve un mensaje de error"
        )]
        [SwaggerResponse(200, "Se ha marcado o no", typeof(bool))]
        [SwaggerResponse(404, "No se ha encontrado la canción")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetIsTrackLiked(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "ID de la canción")]
            long idTrack
        )
        {
            IActionResult salida;
            bool exists = false;

            try
            {
                exists = MetodosUserTrackDAL.isTrackLikedDAL(uid, idTrack);

                salida = Ok(exists);
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/is_artist_favorite/7
        [HttpGet("{uid}/is_artist_favorite/{idArtist}")]
        [SwaggerOperation(
            Summary = "Obtiene si el artista está marcado como favorito",
            Description = "Este método recibe un UID y un ID de un artista y devuelve si el usuario la ha marcado como favorito." +
            "Si no se encuentra el artista devuelve un mensaje de error"
        )]
        [SwaggerResponse(200, "Se ha marcado o no", typeof(bool))]
        [SwaggerResponse(404, "No se ha encontrado al artista")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetIsArtistFavorite(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "ID del artista")]
            long idArtist
        )
        {
            IActionResult salida;
            bool exists = false;

            try
            {
                exists = MetodosUserArtistDAL.isArtistFavorite(uid, idArtist);

                salida = Ok(exists);
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/liked_ids
        [HttpGet("{uid}/liked_ids")]
        [SwaggerOperation(
            Summary = "Devuelve la lista de IDs de las canciones que le han gustado",
            Description = "Este método recibe un UID y devuelve la lista de IDs de las canciones que le han gustado." +
            "Si no se encuentra canciones devuelve un mensaje de error"
        )]
        [SwaggerResponse(200, "Lista de IDs", typeof(List<long>))]
        [SwaggerResponse(404, "No se han encontrado canciones")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetLikedIDs(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid
        )
        {
            IActionResult salida;
            List<long> ids = [];

            try
            {
                ids = ListadosUserDAL.getLikedTracksIdsDAL(uid);

                if (ids.IsNullOrEmpty())
                {
                    salida = NotFound("No se han encontrado canciones");
                } else
                {
                    salida = Ok(ids);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/disliked_ids
        [HttpGet("{uid}/disliked_ids")]
        [SwaggerOperation(
            Summary = "Devuelve la lista de IDs de las canciones que no le han gustado",
            Description = "Este método recibe un UID y devuelve la lista de IDs de las canciones que no le han gustado." +
            "Si no se encuentra canciones devuelve un mensaje de error"
        )]
        [SwaggerResponse(200, "Lista de IDs", typeof(List<long>))]
        [SwaggerResponse(404, "No se han encontrado canciones")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetDislikedIDs(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid
        )
        {
            IActionResult salida;
            List<long> ids = [];

            try
            {
                ids = ListadosUserDAL.getDislikedTracksIdsDAL(uid);

                if (ids.IsNullOrEmpty())
                {
                    salida = NotFound("No se han encontrado canciones");
                }
                else
                {
                    salida = Ok(ids);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/swiped_ids
        [HttpGet("{uid}/swiped_ids")]
        [SwaggerOperation(
            Summary = "Devuelve la lista de IDs de las canciones que ha swipeado",
            Description = "Este método recibe un UID y devuelve la lista de IDs de las canciones que ha swipeado." +
            "Si no se encuentra canciones devuelve un mensaje de error"
        )]
        [SwaggerResponse(200, "Lista de IDs", typeof(List<long>))]
        [SwaggerResponse(404, "No se han encontrado canciones")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetSwipedIDs(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid
        )
        {
            IActionResult salida;
            List<long> ids = [];

            try
            {
                ids = ListadosUserDAL.getSwipedTracksIdsDAL(uid);

                if (ids.IsNullOrEmpty())
                {
                    salida = NotFound("No se han encontrado canciones");
                }
                else
                {
                    salida = Ok(ids);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/followers
        [HttpGet("{uid}/followers")]
        [SwaggerOperation(
            Summary = "Devuelve la lista de los seguidores del usuario",
            Description = "Este método recibe un UID y devuelve la lista de los seguidores del usuario"
        )]
        [SwaggerResponse(200, "Lista de Usuarios", typeof(List<Usuario>))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetFollowers(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid
        )
        {
            IActionResult salida;
            List<Usuario> users = [];

            try
            {
                users = ListadosUserDAL.getFollowersDAL(uid);

                salida = Ok(users);
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // GET api/<User>/5/following
        [HttpGet("{uid}/following")]
        [SwaggerOperation(
            Summary = "Devuelve la lista de los seguidos del usuario",
            Description = "Este método recibe un UID y devuelve la lista de los seguidos del usuario"
        )]
        [SwaggerResponse(200, "Lista de Usuarios", typeof(List<Usuario>))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult GetFollowing(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid
        )
        {
            IActionResult salida;
            List<Usuario> users = [];

            try
            {
                users = ListadosUserDAL.getFollowigDAL(uid);

                salida = Ok(users);
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // POST api/<User>/5/tracks_not_saved
        [HttpPost("{uid}/tracks_not_saved")]
        [SwaggerOperation(
            Summary = "Devuelve una lista de ids de canciones que no han sido guardadas aún",
            Description = "Este método recibe el uid del usuario y una lista de ids de canciones y devuelve una lista con los ids de las canciones que no se han guardado aún<br>"
        )]
        [SwaggerResponse(200, "Canción guardada o no", typeof(List<long>))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult IsTrackSaved(
            [SwaggerParameter(Description = "UID del usuario a obtener los ids de los géneros que sigue")]
            [FromRoute] String uid,
            [SwaggerParameter(Description = "Lista de ids de canciones")]
            [FromBody] List<long> idsTracks
        )
        {
            IActionResult salida;
            List<long> tracksNotSaved = [];

            try
            {
                tracksNotSaved = MetodosUserTrackDAL.hasUserSavedTrackDAL(uid, idsTracks);

                salida = Ok(tracksNotSaved);

            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // POST api/<User>/5/send_request
        [HttpPost("{uid}/send_request")]
        [SwaggerOperation(
            Summary = "Envía una solicitud de amistad",
            Description = "Este método recibe dos UIDs y envía una solicitud de amistad<br>"
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult SendRequest(
            [SwaggerParameter(Description = "UID del usuario emisor")]
            [FromRoute] String uid,
            [SwaggerParameter(Description = "UID del usuario receptor")]
            [FromBody] String uidFriend
        )
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserDAL.sendRequestDAL(uid, uidFriend);

                salida = Ok(numFilasAfectadas);

            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // POST api/<User>/5/delete_request
        [HttpPost("{uid}/delete_request")]
        [SwaggerOperation(
            Summary = "Elimina una solicitud de amistad",
            Description = "Este método recibe dos UIDs y elimina una solicitud de amistad<br>"
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult DeleteRequest(
            [SwaggerParameter(Description = "UID del usuario emisor")]
            [FromRoute] String uid,
            [SwaggerParameter(Description = "UID del usuario receptor")]
            [FromBody] String uidFriend
        )
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserDAL.deleteRequestDAL(uid, uidFriend);

                salida = Ok(numFilasAfectadas);

            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // POST api/<User>/5/accept_request
        [HttpPost("{uid}/accept_request")]
        [SwaggerOperation(
            Summary = "Acepta una solicitud de amistad",
            Description = "Este método recibe dos UIDs y acepta una solicitud de amistad<br>"
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult AcceptRequest(
            [SwaggerParameter(Description = "UID del usuario emisor")]
            [FromRoute] String uid,
            [SwaggerParameter(Description = "UID del usuario receptor")]
            [FromBody] String uidFriend
        )
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserDAL.acceptRequestDAL(uid, uidFriend);

                salida = Ok(numFilasAfectadas);

            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // POST api/<User>/5/decline_request
        [HttpPost("{uid}/decline_request")]
        [SwaggerOperation(
            Summary = "Rechazar una solicitud de amistad",
            Description = "Este método recibe dos UIDs y rechaza una solicitud de amistad<br>"
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult DeclineRequest(
            [SwaggerParameter(Description = "UID del usuario emisor")]
            [FromRoute] String uid,
            [SwaggerParameter(Description = "UID del usuario receptor")]
            [FromBody] String uidFriend
        )
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserDAL.declineRequestDAL(uid, uidFriend);

                salida = Ok(numFilasAfectadas);

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
        public IActionResult Post(
            [SwaggerParameter(Description = "Datos del usuario")]
            [FromBody] Usuario user
        )
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserDAL.createUserDAL(user);
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
        public IActionResult PostArtistas(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "Lista de ids de artistas")]
            [FromBody] List<long> artistas)
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserArtistDAL.addArtistsToFavoritesDAL(uid, artistas);
                if (numFilasAfectadas == 0)
                {
                    salida = NotFound(numFilasAfectadas);
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
        public IActionResult PostGeneros(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "Lista de ids de géneros")]
            [FromBody] List<long> generos)
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserGenresDAL.addGenresToFavoritesDAL(uid, generos);
                if (numFilasAfectadas == 0)
                {
                    salida = NotFound(numFilasAfectadas);
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

        // POST api/<User>/5/save_swipes
        [HttpPost("{uid}/save_swipes")]
        [SwaggerOperation(
            Summary = "Obtiene el UID de un usuario y una lista de swipes y los guarda en la base de datos",
            Description = "Este método obtiene un UID de un usuario y una lista de swipes y lo guarda en la base de datos.<br>" +
            "Devuelve el número de filas afectadads"
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult PostSwipes(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "Lista de Swipes")]
            [FromBody] List<Swipe> swipes)
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserTrackDAL.saveSwipesDAL(uid, swipes);
                if (numFilasAfectadas == 0)
                {
                    salida = NotFound(numFilasAfectadas);
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

        // POST api/<User>/5/delete_friend
        [HttpPost("{uid}/delete_friend")]
        [SwaggerOperation(
            Summary = "Obtiene el UID de un usuario y el uid de su amigo y lo elimina",
            Description = "Este método obtiene un UID de un usuario y el uid de su amigo y lo elimina<br>" +
            "Devuelve el número de filas afectadads"
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult PostDeleteFriend(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "UID del amigo")]
            [FromBody] String uidFriend)
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserDAL.deleteFriendDAL(uid, uidFriend);
                if (numFilasAfectadas == 0)
                {
                    salida = NotFound(numFilasAfectadas);
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

        // POST api/<User>/5/accept_all_requests
        [HttpPost("{uid}/accept_all_requests")]
        [SwaggerOperation(
            Summary = "Obtiene el UID de un usuario y acepta todas sus solicitudes de amistad entrantes",
            Description = "Este método obtiene un UID de un usuario y acepta todas sus solicitudes de amistad entrantes"
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(void))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult AcceptAllRequests(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid)
        {
            IActionResult salida;

            try
            {
                MetodosUserDAL.acceptAllRequestsDAL(uid);

                salida = Ok();
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
            [FromBody] Usuario user
        )
        {
            IActionResult salida;

            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserDAL.updateUserDAL(user);

                if (numFilasAfectadas == 0)
                {
                    salida = NotFound("No se ha podido actualizar al usuario");
                }
                else
                {
                    salida = Ok(user);
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // PUT api/<User>/5/settings/
        [HttpPut("{uid}/settings")]
        [SwaggerOperation(
            Summary = "Obtiene un UID de usuario, sus ajustes actualizados y lo actualiza en la base de datos",
            Description = "Este método obtiene un UID de usuario, sus ajustes actualizados y lo actualiza de la base de datos.<br>" +
            "Si no se ha podido actualizar devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(404, "No se ha podido actualizar los ajustes")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult PutAjustes(
            [SwaggerParameter(Description = "UID del usuario a actualizar")]
            String uid,
            [SwaggerParameter(Description = "Ajustes actualizados")]
            [FromBody] Settings settings)
        {
            IActionResult salida;

            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserDAL.updateUserSettingsDAL(settings, uid);

                if (numFilasAfectadas == 0)
                {
                    salida = NotFound("No se ha podido actualizar los ajustes");
                }
                else
                {
                    salida = Ok("Ajustes actualizados correctamente");
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // PUT api/<User>/5/update_swipe
        [HttpPut("{uid}/update_swipe")]
        [SwaggerOperation(
            Summary = "Obtiene un UID de usuario, el ID de la canción y el nuevo valor del like y lo actualiza en la base de datos",
            Description = "Este método obtiene un UID de usuario, el ID de la canción y el nuevo valor del like y lo actualiza de la base de datos.<br>" +
            "Si no se ha podido actualizar devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(404, "No se ha podido actualizar el Swipe")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult PutSwipe(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "Simple Swipe con el nuevo valor del like")]
            [FromBody] SimpleSwipe simpleSwipe)
        {
            IActionResult salida;

            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserTrackDAL.updateSwipeDAL(uid, simpleSwipe);

                if (numFilasAfectadas == 0)
                {
                    salida = NotFound("No se ha podido actualizar el Swipe");
                }
                else
                {
                    salida = Ok("Swipe actualizado correctamente");
                }
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // PUT api/<User>/5/reactivate_account
        [HttpPut("{uid}/reactivate_account")]
        [SwaggerOperation(
            Summary = "Obtiene un UID de usuario y reactiva su cuenta eliminada",
            Description = "Este método obtiene un UID de usuario y reactiva su cuenta eliminada<br>" +
            "Si no se ha podido actualizar devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(404, "No se ha podido actualizar el Swipe")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult ReactivateAccount(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid)
        {
            IActionResult salida;

            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserDAL.reactivateAccountDAL(uid);

                if (numFilasAfectadas == 0)
                {
                    salida = NotFound(numFilasAfectadas);
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
            String uid
        )
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserDAL.deleteUserDAL(uid);
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

        // DELETE api/<User>/5/artist/3
        [HttpDelete("{uid}/artist/{idArtist}")]
        [SwaggerOperation(
            Summary = "Obtiene un UID de usuario y el ID de un artista y lo elimina de la sus favoritos",
            Description = "Este método obtiene un UID de usuario y el ID de un artista y lo borra de sus favoritos.<br>" +
            "Si no se encuentra ningún usuario devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(404, "No se ha podido eliminar al artista")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult DeleteArtist(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "ID del artista a eliminar de sus favoritos")]
            long idArtist
        )
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserArtistDAL.deleteArtistFromFavoritesDAL(uid, idArtist);
                salida = Ok(numFilasAfectadas);
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }

        // DELETE api/<User>/5/genre/3
        [HttpDelete("{uid}/genre/{idGenre}")]
        [SwaggerOperation(
            Summary = "Obtiene un UID de usuario y el ID de un género y lo elimina de la sus favoritos",
            Description = "Este método obtiene un UID de usuario y el ID de un género y lo borra de sus favoritos.<br>" +
            "Si no se encuentra ningún usuario devuelve un mensaje de error."
        )]
        [SwaggerResponse(200, "Número de filas afectadas", typeof(int))]
        [SwaggerResponse(404, "No se ha podido eliminar el género")]
        [SwaggerResponse(500, "Error interno del servidor")]
        public IActionResult DeleteGenre(
            [SwaggerParameter(Description = "UID del usuario")]
            String uid,
            [SwaggerParameter(Description = "ID del género a eliminar de sus favoritos")]
            long idGenre
        )
        {
            IActionResult salida;
            int numFilasAfectadas = 0;

            try
            {
                numFilasAfectadas = MetodosUserGenresDAL.deleteGenreFromFavoritesDAL(uid, idGenre);
                salida = Ok(numFilasAfectadas);
            }
            catch (Exception e)
            {
                salida = BadRequest(e.Message);
            }

            return salida;
        }
    }
}
