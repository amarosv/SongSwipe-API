using Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Utils
{
    public class CallApiDeezer
    {

        private static readonly HttpClient _httpClient = new HttpClient();
        private const string DeezerApiUrl = "https://api.deezer.com/";
        private const int MaxRequestsPerWindow = 50;
        private static DateTime _lastRequestTime = DateTime.MinValue;
        private static int _requestCount = 0;

        public static async Task<Track> HandleRateLimitAndGetTrack(long trackId)
        {
            await HandleRateLimit();

            var response = await _httpClient.GetAsync($"{DeezerApiUrl}/track/{trackId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Track trackData = JsonConvert.DeserializeObject<Track>(content);
                return trackData;
            }
            return null;
        }

        public static async Task<Album> HandleRateLimitAndGetAlbum(long albumId)
        {
            await HandleRateLimit();

            var response = await _httpClient.GetAsync($"{DeezerApiUrl}/album/{albumId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Album albumData = JsonConvert.DeserializeObject<Album>(content);
                return albumData;
            }
            return null;
        }
        
        public static async Task<Artist> HandleRateLimitAndGetArtist(long artistId)
        {
            await HandleRateLimit();

            var response = await _httpClient.GetAsync($"{DeezerApiUrl}/artist/{artistId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Artist artistData = JsonConvert.DeserializeObject<Artist>(content);
                return artistData;
            }
            return null;
        }

        public static async Task<Genre> HandleRateLimitAndGetGenre(long genreId)
        {
            await HandleRateLimit();

            var response = await _httpClient.GetAsync($"{DeezerApiUrl}/genre/{genreId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Genre genreData = JsonConvert.DeserializeObject<Genre>(content);
                return genreData;
            }
            return null;
        }

        private static async Task HandleRateLimit()
        {
            if (_requestCount >= MaxRequestsPerWindow)
            {
                TimeSpan elapsed = DateTime.UtcNow - _lastRequestTime;
                if (elapsed.TotalSeconds < 5) // Espera si se superó el límite
                {
                    await Task.Delay(TimeSpan.FromSeconds(5 - elapsed.TotalSeconds));
                }
                _requestCount = 0;
            }

            _lastRequestTime = DateTime.UtcNow;
            _requestCount++;
        }

    }
}
