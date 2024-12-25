using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace SampleApiWithEureka.ServiceRegistery.Extensions
{
	public class EurekaProxyConfig : IProxyConfig
	{
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		public IReadOnlyList<RouteConfig> Routes { get; }

		public IReadOnlyList<ClusterConfig> Clusters { get; }

		public IChangeToken ChangeToken { get; }

		public EurekaProxyConfig(
			IReadOnlyList<RouteConfig> routes, 
			IReadOnlyList<ClusterConfig> clusters)
		{
			Routes = routes;
			Clusters = clusters;
			ChangeToken = new CancellationChangeToken(_cts.Token);
		}

		public void SignalChange() => _cts.Cancel();
	}
}
