using DbLayer.Data.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DbLayer.Data
{
	public static class DatabaseInit
	{
		/// <summary>
		/// Initialize database and create tables if not exists for object models
		/// </summary>
		/// <param name="connectionString"></param>
		public static void Initialize(string connectionString)
		{

			try
			{
				CreateDatabase(connectionString);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Database creation failed:");
				Console.WriteLine(ex.Message);
				return;
			}

			using var con = new SqlConnection(connectionString);
			con.Open();			

			var tblProductQuery = $@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{nameof(Product)}' AND xtype='U') 
									CREATE TABLE {nameof(Product)} (
									{nameof(Product.Id)} INT IDENTITY(1,1) PRIMARY KEY,
									{nameof(Product.Title)} NVARCHAR(50) NOT NULL,
									{nameof(Product.Description)} NVARCHAR(255),
									{nameof(Product.Category)} NVARCHAR(50) NOT NULL,
									{nameof(Product.Price)} DECIMAL(18,2) NOT NULL,
									{nameof(Product.Stock)} INT NOT NULL DEFAULT 0);";

			var tblReviewtQuery = $@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{nameof(Reviews)}' AND xtype='U') 
									CREATE TABLE {nameof(Reviews)} (
									{nameof(Reviews.Id)} INT IDENTITY(1,1) PRIMARY KEY,
									{nameof(Reviews.ProductId)} INT NOT NULL,
									{nameof(Reviews.Comment)} NVARCHAR(255),
									[{nameof(Reviews.Date)}] Date NOT NULL DEFAULT CAST(GETDATE() AS DATE),
									{nameof(Reviews.ReviewerName)} NVARCHAR(255),
									{nameof(Reviews.ReviewerEmail)} NVARCHAR(255)
									CONSTRAINT FK_Reviews_Product FOREIGN KEY ({nameof(Reviews.ProductId)}) REFERENCES {nameof(Product)}({nameof(Product.Id)}));";



			try
			{

				CreateTable(con, tblProductQuery);
				CreateTable(con, tblReviewtQuery);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Database initialization failed:");
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// Create table if not exists
		/// </summary>
		/// <param name="con"></param>
		/// <param name="query"></param>
		private static void CreateTable(SqlConnection con, string query)
		{
			try
			{
				using var cmd = new SqlCommand(query, con);
				cmd.ExecuteNonQuery();

				Console.WriteLine(query);
				Console.WriteLine("Table exists or created successfully.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Table creation failed:");
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// Create database if not exists
		/// </summary>
		/// <param name="connectionString"></param>
		private static void CreateDatabase(string connectionString)
		{
			var builderConn               = new SqlConnectionStringBuilder(connectionString);
			var databaseName              = builderConn.InitialCatalog;
			string masterConnectionString = connectionString.Replace(databaseName, "master");

			using var con = new SqlConnection(masterConnectionString);
			con.Open();

			string query = $@"
							IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{databaseName}')
							BEGIN
								CREATE DATABASE [{databaseName}];
							END
						";
			try
			{
				using var cmd = new SqlCommand(query, con);
				cmd.ExecuteNonQuery();

				Console.WriteLine(query);
				Console.WriteLine($"Database '{databaseName}' exists or created successfully.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Database creation failed:");
				Console.WriteLine(ex.Message);
			}
		}
	}
}
