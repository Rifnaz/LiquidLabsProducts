using DbLayer.Data;
using DbLayer.Interfaces;

namespace DbLayer.Repositories
{
	public class ProductRepository : IProductRepository
	{
		public async Task<List<Product>> GetProducts()
		{
			return new List<Product>();
		}

		public async Task<Product> GetProductById(int id)
		{
			return new Product();
		}

		public async Task<(bool succeed, string meesage)> AddProducts(List<Product> products)
		{
			return (true, "Product added successfully.");
		}

		public async Task<bool> IsAnyProductExist()
		{
			return false;
		}
	}
}
