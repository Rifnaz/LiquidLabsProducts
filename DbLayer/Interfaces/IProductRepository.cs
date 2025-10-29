using DbLayer.Data.Models;

namespace DbLayer.Interfaces
{
	public interface IProductRepository
	{
		/// <summary>
		/// Get all products from database
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
		/// Add products with their reviews in a transaction if any error occurs rollback the transaction will undo all changes
		/// </summary>
		/// <param name="products"></param>
		/// <returns></returns>
		Task<(bool succeed, string meesage)> AddProducts(List<Product> products);

		/// <summary>
		/// Check if any product exists in database
		/// </summary>
		/// <returns></returns>
		Task<bool> IsAnyProductExist();
	}
}
