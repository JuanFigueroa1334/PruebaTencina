using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PruebaTencina.Data;
using PruebaTencina.Models;

namespace PruebaTencina.Controllers
{
    [ApiController]
    [Route("api")]
    public class GifController : ControllerBase
    {

        private readonly AppDbContext _context;

        public GifController(AppDbContext context)
        {
            _context = context;
        }

        private static readonly HttpClient httpClient = new HttpClient();
        private const string GiphyApiKey = "voaNIOg1u7ONPbckzWK71C48YqCOkhVP";
        private const string GiphySearchUrl = "https://api.giphy.com/v1/gifs/search";
        [HttpGet]
        [Route("fact")]
        public async Task<IActionResult> CatFactGet() {
            string apiUrl = "https://catfact.ninja/fact";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Error al obtener el dato del gato.");
                }

                var content = await response.Content.ReadAsStringAsync();
                var fact = JsonConvert.DeserializeObject<CatFact>(content);

                return Ok(fact); // Devuelve el JSON recibido
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("gif")]
        public async Task<IActionResult> GetGifFromQuery([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest("El parámetro 'query' es obligatorio.");
                }

                // Limitar a las primeras 3 palabras si se envían más
                var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var searchQuery = string.Join(" ", words.Take(3));
                var random = new Random();
                int offset = random.Next(1, 15);
                // Armar URL de búsqueda de Giphy
                var giphyUrl = $"{GiphySearchUrl}?api_key={GiphyApiKey}&q={Uri.EscapeDataString(searchQuery)}&limit=1&offset={offset}&rating=g";

                HttpResponseMessage response = await httpClient.GetAsync(giphyUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Error al consultar Giphy API.");
                }

                var content = await response.Content.ReadAsStringAsync();
                var giphyData = JsonConvert.DeserializeObject<dynamic>(content);

                // Verificar si se encontró al menos un GIF
                if (giphyData.data.Count == 0)
                {
                    return NotFound("No se encontró ningún GIF para la búsqueda.");
                }

                // Devolver el primer resultado
                string gifUrl = giphyData.data[0].images.original.url;

                var busqueda = new BusquedaGif
                {
                    FechaBusqueda = DateTime.Now,
                    CatFact = query,
                    ParamSearch = searchQuery,
                    UrlGif = gifUrl
                };

                // ✅ Guardar en base de datos usando el contexto
                _context.BusquedaGifs.Add(busqueda);
                await _context.SaveChangesAsync();

                return Ok(new

                {
                    query = searchQuery,
                    gif = gifUrl
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
