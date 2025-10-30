using ServiceLayer.Interfaces;
using ServiceLayer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceLayer
{
	public static class ServiceLayerConfig
	{
		/// <summary>
		/// Add Business Logic Layer services to the DI container
		/// </summary>
		/// <param name="services">Service collection to be added to DI container</param>
		/// <returns>
		/// Returns the service collection with Business Logic Layer services added
		/// </returns>
		public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
		{
			services.AddMemoryCache();
			services.AddTransient<IProductService, ProductService>();

			return services;
		}
	}
}
