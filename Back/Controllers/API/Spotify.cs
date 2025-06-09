using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Back.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class Spotify : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public Spotify(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("refresh")]
        [SwaggerOperation(
            Summary = "Recibe una petición para cambiar el token de Spotify y lo cambia",
            Description = "Este método recibe una petición para cambiar el token de Spotify y lo cambia"
        )]
        [SwaggerResponse(200, "Nuevo token", typeof(String))]
        [SwaggerResponse(500, "Error interno del servidor")]
        public async Task<IActionResult> RefreshToken(
            [SwaggerParameter(Description = "Petición")]
            [FromBody] RefreshRequest request
        )
        {
            var clientId = _configuration["Spotify:ClientId"];
            var clientSecret = _configuration["Spotify:ClientSecret"];
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            using var http = new HttpClient();
            var postData = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", request.RefreshToken),
        });

            http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);

            var response = await http.PostAsync("https://accounts.spotify.com/api/token", postData);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(content);
            }

            return Ok(content);
        }
    }
}
