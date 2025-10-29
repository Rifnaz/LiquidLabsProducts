using DbLayer.Data.Models;

namespace ServiceLayer.Interfaces
{
	public interface IProductService
	{
		/// <summary>
		/// Get all products from database, if not exist get from external API and save to database and return
		/// </summary>
		/// <returns></returns>
		Task<List<Product>> GetProducts();

		/// <summary>
		/// Get product details by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<Product> GetProductById(int id);

		/// <summary>
		/// Gets products from external public API (https://dummyjson.com/products)
		/// </summary>
		/// <returns></returns>
		Task<List<Product>> GetProductsFromAPI();
	}
}
