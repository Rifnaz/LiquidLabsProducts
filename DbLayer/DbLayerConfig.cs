using DbLayer.Interfaces;
using DbLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DbLayer
{
	public static class DbLayerConfig
	{
		public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
		{
			services.AddTransient<IProductRepository, ProductRepository>();

			return services;
		}
	}
}
