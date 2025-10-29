using DbLayer.Data;
using DbLayer.Interfaces;
using DbLayer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DbLayer
{
	public static class DbLayerConfig
	{
		public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");

			DatabaseInit.Initialize(connectionString);

			services.AddScoped<IProductRepository>(sp => new ProductRepository(connectionString));

			return services;
		}
	}
}
