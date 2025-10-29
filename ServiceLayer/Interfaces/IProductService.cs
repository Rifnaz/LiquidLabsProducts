using DbLayer.Data.Models;

namespace ServiceLayer.Interfaces
{
	public interface IProductService
	{
		Task<List<Product>> GetProducts();

		Task<Product> GetProductById(int id);

		Task<List<Product>> GetProductsFromAPI();
	}
}
