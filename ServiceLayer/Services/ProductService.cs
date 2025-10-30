using DbLayer.Data.Models;
using DbLayer.Helpers;
using DbLayer.Interfaces;
using ServiceLayer.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using DbLayer.Helpers.Enums;

namespace ServiceLayer.Services
{
	public class ProductService : IProductService
	{
		private readonly HttpClient _httpClient;
		private readonly IProductRepository _product;
		private readonly IMemoryCache _cache;

		public ProductService(HttpClient httpClient, IProductRepository product, IMemoryCache cache)
		{
			_httpClient = httpClient;
			_product    = product;
			_cache      = cache;
		}

		/// <summary>
		/// Get all products from database, if not exist get from external API and save to database and return
		/// </summary>
		/// <returns>
		/// Succeed: List of products
		/// Error: Empty list
		/// </returns>
		public async Task<List<Product>> GetProducts()
		{
			if(_cache.TryGetValue(CacheKeys.ProductsCacheKey, out List<Product> cachedProducts))
				return cachedProducts;

			if (!await IsRecordReady())
				return new();

			var products = await _product.GetProducts();

			_cache.Set(CacheKeys.ProductsCacheKey, products, TimeSpan.FromMinutes(5));

			return products;
		}

		/// <summary>
		/// Get product details by id
		/// </summary>
		/// <param name="id">Primary Key of Product</param>
		/// <returns>
		/// Succeed: A object with Product details
		/// Error: An empty object
		/// </returns>
		public async Task<Product> GetProductById(int id)
		{
			if (_cache.TryGetValue(CacheKeys.ProductsCacheKey, out Product cachedProduct))
				return cachedProduct;

			if (!await IsRecordReady())
				return new();

			var product =  await _product.GetProductById(id);

			_cache.Set(CacheKeys.ProductByIdCacheKey, product, TimeSpan.FromMinutes(5));

			return product;
		}

		/// <summary>
		/// Gets products from external public API (https://dummyjson.com/products)
		/// </summary>
		/// <returns>
		/// Succeed: List of products from API
		/// Error: Empty list
		/// </returns>
		private async Task<List<Product>> GetProductsFromAPI()
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

		/// <summary>
		/// Check if records are ready in database, if not get from external API and save to database
		/// </summary>
		/// <returns>
		/// Succeed: True if records available or added successfully
		/// Error: False
		/// </returns>
		private async Task<bool> IsRecordReady()
		{
			if (await _product.IsAnyProductExist())
				return true;

			var productsFromApi = await GetProductsFromAPI();
			var addResult = await _product.AddProducts(productsFromApi, ClearCache);

			return addResult.succeed;
		}

		/// <summary>
		/// Clear cache entries
		/// </summary>
		private void ClearCache()
		{
			_cache.Remove(CacheKeys.ProductsCacheKey);
			_cache.Remove(CacheKeys.ProductByIdCacheKey);
		}
	}
}
