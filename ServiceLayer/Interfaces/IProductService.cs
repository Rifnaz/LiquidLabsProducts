using DbLayer.Data;

namespace ServiceLayer.Interfaces
{
	public interface IProductService
	{
		Task<List<Product>> GetProducts();

		Task<Product> GetProductById(int id);

		Task<List<Product>> GetProductsFromAPI();
	}
}
