using Yarp.ReverseProxy.Configuration;

namespace SampleApiWithEureka.ServiceRegistery.Extensions
{
	public static class DependencyInjections
	{
		public static IReverseProxyBuilder LoadFromEureka(this IReverseProxyBuilder builder)
		{
			builder.Services.AddSingleton<EurekaProxyConfigProvider>();

			builder.Services.AddSingleton<IProxyConfigProvider>(
				c => c.GetRequiredService<EurekaProxyConfigProvider>());

			builder.Services.AddSingleton<IHostedService>(
				c => c.GetRequiredService<EurekaProxyConfigProvider>());

			return builder;
		}
	}
}
