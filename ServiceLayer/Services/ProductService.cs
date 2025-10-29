using DbLayer.Data.Models;
using DbLayer.Helpers;
using DbLayer.Interfaces;
using ServiceLayer.Interfaces;
using System.Reflection.Metadata;
using System.Text.Json;

namespace ServiceLayer.Services
{
	public class ProductService : IProductService
	{
		private readonly HttpClient _httpClient;
		private readonly IProductRepository _product;

		public ProductService(HttpClient httpClient, IProductRepository product)
		{
			_httpClient = httpClient;
			_product   = product;
		}

		/// <summary>
		/// Get all products from database, if not exist get from external API and save to database and return
		/// </summary>
		/// <returns></returns>
		public async Task<List<Product>> GetProducts()
		{
			//var isAnyProductExist = await _product.IsAnyProductExist();

			//if (!isAnyProductExist)
			//{
			//	var productsFromApi = await GetProductsFromAPI();
			//	var addResult       = await _product.AddProducts(productsFromApi);

			//	if (!addResult.succeed)
			//		return new();
			//}
			
			//return await _product.GetProducts();

			return await GetProductsFromAPI();
		}

		/// <summary>
		/// Get product details by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<Product> GetProductById(int id)
		{
			return await _product.GetProductById(id);
		}

		/// <summary>
		/// Gets products from external public API (https://dummyjson.com/products)
		/// </summary>
		/// <returns></returns>
		public async Task<List<Product>> GetProductsFromAPI()
		{
			try
			{
				var response = await _httpClient.GetAsync(Constants.ProductsApiEndpoint);

				if (!response.IsSuccessStatusCode)
					return new();

				var content = await response.Content.ReadAsStringAsync();
				var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

				var productResponse = JsonSerializer.Deserialize<ProductResponse>(content, options);

				return productResponse?.Products ?? new();
			}
			catch (Exception ex)
			{
				return new();
			}
		}
	}
}
