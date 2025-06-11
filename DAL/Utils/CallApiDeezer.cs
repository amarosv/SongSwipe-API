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
            int maxRetries = 3;
            int retries = 0;

            while (retries < maxRetries)
            {
                await HandleRateLimit(); // Asegura la lógica preventiva

                var response = await _httpClient.GetAsync($"{DeezerApiUrl}/track/{trackId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Track>(content);
                }
                else if ((int)response.StatusCode == 429)
                {
                    retries++;

                    // Intenta leer el tiempo de espera recomendado por la API
                    if (response.Headers.TryGetValues("Retry-After", out var values))
                    {
                        if (int.TryParse(values.FirstOrDefault(), out int secondsToWait))
                        {
                            await Task.Delay(TimeSpan.FromSeconds(secondsToWait));
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(5)); // fallback si no se puede parsear
                        }
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5)); // fallback si no hay cabecera
                    }
                }
                else
                {
                    // Si no es éxito ni 429, rompe el bucle para no repetir en errores permanentes (404, 500, etc.)
                    break;
                }
            }

            return null;
        }


        public static async Task<Album> HandleRateLimitAndGetAlbum(long albumId)
        {
            int maxRetries = 3;
            int retries = 0;

            while (retries < maxRetries)
            {
                await HandleRateLimit(); // Precaución adicional si tienes control de cuota

                var response = await _httpClient.GetAsync($"{DeezerApiUrl}/album/{albumId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Album>(content);
                }
                else if ((int)response.StatusCode == 429)
                {
                    retries++;

                    if (response.Headers.TryGetValues("Retry-After", out var values))
                    {
                        if (int.TryParse(values.FirstOrDefault(), out int secondsToWait))
                        {
                            await Task.Delay(TimeSpan.FromSeconds(secondsToWait));
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(5)); // fallback si no es numérico
                        }
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5)); // fallback si no existe la cabecera
                    }
                }
                else
                {
                    break; // Otros errores (404, 500, etc.) no se reintentan
                }
            }

            return null;
        }


        public static async Task<Artist> HandleRateLimitAndGetArtist(long artistId)
        {
            int maxRetries = 3;
            int retries = 0;

            while (retries < maxRetries)
            {
                await HandleRateLimit();

                var response = await _httpClient.GetAsync($"{DeezerApiUrl}/artist/{artistId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Artist>(content);
                }
                else if ((int)response.StatusCode == 429)
                {
                    retries++;
                    // Intenta esperar el tiempo que diga la cabecera Retry-After, si está disponible
                    if (response.Headers.TryGetValues("Retry-After", out var values))
                    {
                        if (int.TryParse(values.FirstOrDefault(), out int secondsToWait))
                        {
                            await Task.Delay(TimeSpan.FromSeconds(secondsToWait));
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(5)); // fallback
                        }
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5)); // fallback
                    }
                }
                else
                {
                    break; // otro error que no es 429
                }
            }

            return null;
        }

        public static async Task<Genre> HandleRateLimitAndGetGenre(long genreId)
        {
            int maxRetries = 3;
            int retries = 0;

            while (retries < maxRetries)
            {
                await HandleRateLimit(); // Medida preventiva, si es útil

                var response = await _httpClient.GetAsync($"{DeezerApiUrl}/genre/{genreId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Genre>(content);
                }
                else if ((int)response.StatusCode == 429)
                {
                    retries++;

                    if (response.Headers.TryGetValues("Retry-After", out var values) &&
                        int.TryParse(values.FirstOrDefault(), out int secondsToWait))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(secondsToWait));
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5)); // Espera por defecto si no hay header
                    }
                }
                else
                {
                    break; // Otros errores (404, 500, etc.) no se reintentan
                }
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
