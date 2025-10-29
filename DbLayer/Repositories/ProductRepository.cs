using DbLayer.Data.Models;
using DbLayer.Interfaces;
using Microsoft.Data.SqlClient;

namespace DbLayer.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly string _connectionString;

		public ProductRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<List<Product>> GetProducts()
		{
			return new List<Product>();
		}

		public async Task<Product> GetProductById(int id)
		{
			return new Product();
		}

		/// <summary>
		/// Add products with their reviews in a transaction if any error occurs rollback the transaction will undo all changes
		/// </summary>
		/// <param name="products"></param>
		/// <returns></returns>
		public async Task<(bool succeed, string meesage)> AddProducts(List<Product> products)
		{
			using var con = new SqlConnection(_connectionString);
			await con.OpenAsync();

			using var transaction = await con.BeginTransactionAsync();

			try
			{

				foreach (var product in products)
				{
					var query = $@"INSERT INTO {nameof(Product)} 
							({nameof(Product.Title)}, {nameof(Product.Description)}, {nameof(Product.Category)}, {nameof(Product.Price)}, {nameof(Product.Stock)}) 
							VALUES (@Title, @Description, @Category, @Price, @Stock);";

					using var cmd = new SqlCommand(query, con);

					cmd.Parameters.AddWithValue("@Title", product.Title);
					cmd.Parameters.AddWithValue("@Description", product.Description);
					cmd.Parameters.AddWithValue("@Category", product.Category);
					cmd.Parameters.AddWithValue("@Price", product.Price);
					cmd.Parameters.AddWithValue("@Stock", product.Stock);

					var productId = (int)await cmd.ExecuteScalarAsync();

					if (product.Reviews != null && product.Reviews.Count > 0)
					{
						var addReviewsResult = await AddReviews(product.Reviews, productId, con);
						
						if (!addReviewsResult.succeed)
						{
							await transaction.RollbackAsync();
							return (false, addReviewsResult.message);
						}
					}
				}

				await transaction.CommitAsync();
				return (true, "Product added successfully.");
			}

			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return (false, ex.Message);
			}
		}

		public async Task<bool> IsAnyProductExist()
		{
			return false;
		}

		/// <summary>
		/// Add reviews for a product within the same SQL connection
		/// </summary>
		/// <param name="reviews"></param>
		/// <param name="ProductId"></param>
		/// <param name="con"></param>
		/// <returns></returns>
		private async Task<(bool succeed, string message)> AddReviews(List<Reviews> reviews, int ProductId, SqlConnection con)
		{
			try
			{
				foreach (var review in reviews)
				{
					var query = $@"INSERT INTO {nameof(Reviews)} 
							({nameof(Reviews.ProductId)}, {nameof(Reviews.Rating)}, {nameof(Reviews.Comment)}, {nameof(Reviews.Date)}, {nameof(Reviews.ReviewerName)}, {nameof(Reviews.ReviewerEmail)}) 
							VALUES (@ProductId, @Rating, @Comment, @Date, @ReviewerName, @ReviewerEmail);";

					using var cmd = new SqlCommand(query, con);

					cmd.Parameters.AddWithValue("@ProductId", ProductId);
					cmd.Parameters.AddWithValue("@Rating", review.Rating);
					cmd.Parameters.AddWithValue("@Comment", review.Comment);
					cmd.Parameters.AddWithValue("@Date", review.Date);
					cmd.Parameters.AddWithValue("@ReviewerName", review.ReviewerName);
					cmd.Parameters.AddWithValue("@ReviewerEmail", review.ReviewerEmail);

					await cmd.ExecuteNonQueryAsync();
				}

				return (true, "Reviews added successfully.");
			}
			catch (Exception ex)
			{
				return (false, ex.Message);
			}
		}
	}
}
