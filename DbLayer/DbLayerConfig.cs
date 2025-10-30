using DbLayer.Data;
using DbLayer.Interfaces;
using DbLayer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DbLayer
{
	public static class DbLayerConfig
	{
		/// <summary>
		/// Add Data Access Layer services to the DI container
		/// <param name="services">Service collection to be added to DI</param>
		/// <param name="configuration">Application configuration</param>
		/// <returns></returns>
		public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");

			// Initialize database (create database and tables if not exist)
			DatabaseInit.Initialize(connectionString);

			services.AddScoped<IProductRepository>(sp => new ProductRepository(connectionString));

			return services;
		}
	}
}
