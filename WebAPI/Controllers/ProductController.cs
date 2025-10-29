using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interfaces;
using System.Text.Json;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly HttpClient _httpClient;
		private readonly IProductService _product;

		public ProductController(HttpClient httpClient, IProductService product)
		{
			_httpClient = httpClient;
			_product    = product;
		}

		/// <summary>
		/// Get all products 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var products =  await _product.GetProducts();

			if (products == null || !products.Any())
				return NotFound();

			return Ok(products);
		}

		/// <summary>
		/// Get product details by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var product = await _product.GetProductById(id);

			if (product == null)
				return NotFound();

			return Ok(product);
		}
	}
}
