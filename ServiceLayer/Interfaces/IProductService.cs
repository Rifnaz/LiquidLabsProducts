using DbLayer.Data.Models;

namespace ServiceLayer.Interfaces
{
	public interface IProductService
	{
		/// <summary>
		/// Get all products from database, if not exist get from external API and save to database and return
		/// </summary>
		/// <returns>
		/// Succeed: List of products
		/// Error: Empty list
		/// </returns>
		Task<List<Product>> GetProducts();

		/// <summary>
		/// Get product details by id
		/// </summary>
		/// <param name="id">Primary Key of Product</param>
		/// <returns>
		/// Succeed: A object with Product details
		/// Error: An empty object
		/// </returns>
		Task<Product> GetProductById(int id);
	}
}
