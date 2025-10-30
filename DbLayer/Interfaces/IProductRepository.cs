using DbLayer.Data.Models;

namespace DbLayer.Interfaces
{
	public interface IProductRepository
	{
		/// <summary>
		/// Get all products from database
		/// </summary>
		/// <returns>
		/// Succeed: List of products	
		/// Error: Empty list
		/// </returns>
		Task<List<Product>> GetProducts();

		/// <summary>
		/// Get product details by id
		/// </summary>
		/// <param name="id">Primary Key of the Product</param>
		/// <returns>
		/// Succeed: A object with Product details
		/// Error: An empty object
		/// </returns>
		Task<Product> GetProductById(int id);

		/// <summary>
		/// Add products with their reviews in a transaction if any error occurs rollback the transaction will undo all changes
		/// </summary>
		/// <param name="products">List of products got from API response</param>
		/// <param name="onSuccess">Call back function to invoke on add record success</param>
		/// <returns>
		/// Succeed: Returns True with success message
		/// Error: Returns False with error message
		/// </returns>
		Task<(bool succeed, string meesage)> AddProducts(List<Product> products, Action onSuccess = null);

		/// <summary>
		/// Check if any product exists in database
		/// </summary>
		/// <returns>
		/// Succeed: True or False if any product exists or not
		/// Error: False
		/// </returns>
		Task<bool> IsAnyProductExist();
	}
}
