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

		/// <summary>
		/// Get all products from database
		/// </summary>
		/// <returns>
		/// Succeed: List of products	
		/// Error: Empty list
		/// </returns>
		public async Task<List<Product>> GetProducts()
		{
			await using var con = new SqlConnection(_connectionString);
			await con.OpenAsync();

			try
			{
				var query        = $@"SELECT * FROM {nameof(Product)};";
				await using var cmd    = new SqlCommand(query, con);
				await using var reader = await cmd.ExecuteReaderAsync();

				var products = new List<Product>();

				while (await reader.ReadAsync())
				{
					var product = await MakeProduct(reader);

					products.Add(product);
				}

				return products;
			}
			catch (Exception ex)
			{
				return new();
			}
		}

		/// <summary>
		/// Get product details by id
		/// </summary>
		/// <param name="id">Primary Key of the Product</param>
		/// <returns>
		/// Succeed: A object with Product details
		/// Error: An empty object
		/// </returns>
		public async Task<Product> GetProductById(int id)
		{
			await using var con = new SqlConnection(_connectionString);
			await con.OpenAsync();

			try
			{
				var query = $@"SELECT * FROM {nameof(Product)} WHERE {nameof(Product.Id)} = @Id;";

				await using var cmd = new SqlCommand(query, con);
				cmd.Parameters.AddWithValue("@Id", id);

				await using var reader = await cmd.ExecuteReaderAsync();

				if (await reader.ReadAsync())
				{
					return await MakeProduct(reader);
				}

				return new Product();
			}
			catch (Exception ex)
			{
				return new Product();
			}
		}

		/// <summary>
		/// Add products with their reviews in a transaction if any error occurs rollback the transaction will undo all changes
		/// </summary>
		/// <param name="products">List of products got from API response</param>
		/// <param name="onSuccess">Call back function to invoke on add record success</param>
		/// <returns>
		/// Succeed: Returns True with success message
		/// Error: Returns False with error message
		/// </returns>
		public async Task<(bool succeed, string meesage)> AddProducts(List<Product> products, Action onSuccess = null)
		{
			await using var con = new SqlConnection(_connectionString);
			await con.OpenAsync();

			await using var transaction = await con.BeginTransactionAsync();

			try
			{

				foreach (var product in products)
				{
					var query = $@"INSERT INTO {nameof(Product)} 
							({nameof(Product.Title)}, {nameof(Product.Description)}, {nameof(Product.Category)}, {nameof(Product.Price)}, {nameof(Product.Stock)}) 
							VALUES (@Title, @Description, @Category, @Price, @Stock); SELECT CAST(SCOPE_IDENTITY() AS INT);";

					await using var cmd = new SqlCommand(query, con, (SqlTransaction)transaction);

					cmd.Parameters.AddWithValue("@Title", product.Title);
					cmd.Parameters.AddWithValue("@Description", product.Description);
					cmd.Parameters.AddWithValue("@Category", product.Category);
					cmd.Parameters.AddWithValue("@Price", product.Price);
					cmd.Parameters.AddWithValue("@Stock", product.Stock);

					var productId = (int)await cmd.ExecuteScalarAsync();

					if (product.Reviews != null && product.Reviews.Count > 0)
					{
						var addReviewsResult = await AddReviews(product.Reviews, productId, con, (SqlTransaction)transaction);
						
						if (!addReviewsResult.succeed)
						{
							await transaction.RollbackAsync();
							return (false, addReviewsResult.message);
						}
					}
				}

				await transaction.CommitAsync();
				onSuccess?.Invoke();
				return (true, "Product added successfully.");
			}

			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return (false, ex.Message);
			}
		}

		/// <summary>
		/// Check if any product exists in database
		/// </summary>
		/// <returns>
		/// Succeed: True or False if any product exists or not
		/// Error: False
		/// </returns>
		public async Task<bool> IsAnyProductExist()
		{
			await using var con = new SqlConnection(_connectionString);
			await con.OpenAsync();

			try
			{
				var query = $@"SELECT COUNT(1) FROM {nameof(Product)};";

				await using var cmd = new SqlCommand(query, con);
				var count = (int)await cmd.ExecuteScalarAsync();

				return count > 0;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		/// <summary>
		/// Prepare product from data reader
		/// </summary>
		/// <param name="reader">sql data reader</param>
		/// <returns>
		/// Succeed: Product object
		/// </returns>
		private async Task<Product> MakeProduct(SqlDataReader reader)
		{
			var productId = (int)reader[nameof(Product.Id)];

			var product = new Product
			{
				Id          = productId,
				Title       = reader[nameof(Product.Title)].ToString(),
				Description = reader[nameof(Product.Description)].ToString(),
				Category    = reader[nameof(Product.Category)].ToString(),
				Price       = (decimal)reader[nameof(Product.Price)],
				Stock       = (int)reader[nameof(Product.Stock)],
				Reviews     = await GetReviewsByProductd(productId)
			};

			return product;
		}

		/// <summary>
		/// Get reviews by product id
		/// </summary>
		/// <param name="productId">Primary Key of the product and Foriegn Key of Reviews</param>
		/// <returns>
		/// Succeed: List of reviews for the product
		/// Error: Empty list
		/// </returns>
		private async Task<List<Reviews>> GetReviewsByProductd(int productId)
		{
			await using var con = new SqlConnection(_connectionString);
			await con.OpenAsync();

			try
			{
				var query = $@"SELECT * FROM {nameof(Reviews)} WHERE {nameof(Reviews.ProductId)} = @ProductId;";

				await using var cmd = new SqlCommand(query, con);
				cmd.Parameters.AddWithValue("@ProductId", productId);

				await using var reader = await cmd.ExecuteReaderAsync();
				var reviews = new List<Reviews>();

				while (await reader.ReadAsync())
				{
					var review = new Reviews
					{
						Id            = (int)reader[nameof(Reviews.Id)],
						ProductId     = (int)reader[nameof(Reviews.ProductId)],
						Rating        = (int)reader[nameof(Reviews.Rating)],
						Comment       = reader[nameof(Reviews.Comment)].ToString(),
						Date          = (DateTime)reader[nameof(Reviews.Date)],
						ReviewerName  = reader[nameof(Reviews.ReviewerName)].ToString(),
						ReviewerEmail = reader[nameof(Reviews.ReviewerEmail)].ToString()
					};

					reviews.Add(review);
				}

				return reviews;
			}
			catch (Exception ex)
			{
				return new();
			}
		}

		/// <summary>
		/// Add reviews for a product within the same SQL connection
		/// </summary>
		/// <param name="reviews">List of Reviews</param>
		/// <param name="ProductId">Primary Key of the product and Foriegn Key of Reviews</param>
		/// <param name="con">Sql connection opened</param>
		/// <returns></returns>
		private async Task<(bool succeed, string message)> AddReviews(List<Reviews> reviews, int ProductId, SqlConnection con, SqlTransaction transaction)
		{
			try
			{
				foreach (var review in reviews)
				{
					var query = $@"INSERT INTO {nameof(Reviews)} 
							({nameof(Reviews.ProductId)}, {nameof(Reviews.Rating)}, {nameof(Reviews.Comment)}, {nameof(Reviews.Date)}, {nameof(Reviews.ReviewerName)}, {nameof(Reviews.ReviewerEmail)}) 
							VALUES (@ProductId, @Rating, @Comment, @Date, @ReviewerName, @ReviewerEmail);";

					await using var cmd = new SqlCommand(query, con, transaction);

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
