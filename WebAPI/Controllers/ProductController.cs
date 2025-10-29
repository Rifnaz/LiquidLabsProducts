using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly HttpClient _httpClient;

		public ProductController(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		// GET: api/<ProductController>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			string ApiUrl = "https://dummyjson.com/products";

			try
			{
				var response = await _httpClient.GetAsync(ApiUrl);

				if (!response.IsSuccessStatusCode)
					return BadRequest(response.RequestMessage);

				var content = await response.Content.ReadAsStringAsync();

				var json = JsonDocument.Parse(content);

				return Ok(json.RootElement);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// GET api/<ProductController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			string ApiUrl = $"https://dummyjson.com/products/{id}";

			try
			{
				var response = await _httpClient.GetAsync(ApiUrl);

				if (!response.IsSuccessStatusCode)
					return BadRequest(response.RequestMessage);

				var content = await response.Content.ReadAsStringAsync();

				var json = JsonDocument.Parse(content);

				return Ok(json.RootElement);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
