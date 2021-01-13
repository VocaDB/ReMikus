using Microsoft.Extensions.DependencyInjection;

namespace VocaDb.ReMikus
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddLaravelMix(this IServiceCollection services) => services.AddSingleton<LaravelMix>();
	}
}
