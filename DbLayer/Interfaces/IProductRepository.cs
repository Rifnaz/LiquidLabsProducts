using DbLayer.Data;

namespace DbLayer.Interfaces
{
	public interface IProductRepository
	{
		Task<List<Product>> GetProducts();

		Task<Product> GetProductById(int id);

		Task<(bool succeed, string meesage)> AddProducts(List<Product> products);

		Task<bool> IsAnyProductExist();
	}
}
