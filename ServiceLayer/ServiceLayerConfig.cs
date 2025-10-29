using ServiceLayer.Interfaces;
using ServiceLayer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceLayer
{
	public static class ServiceLayerConfig
	{
		public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
		{
			services.AddMemoryCache();
			services.AddTransient<IProductService, ProductService>();

			return services;
		}
	}
}
