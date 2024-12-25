using Steeltoe.Discovery;
using Steeltoe.Discovery.Eureka;
using Yarp.ReverseProxy.Configuration;

namespace SampleApiWithEureka.ServiceRegistery.Extensions
{
	public class EurekaProxyConfigProvider : BackgroundService, IProxyConfigProvider
	{
		private EurekaProxyConfig _config;

		public IProxyConfig GetConfig() => _config;

		private readonly List<RouteConfig> _routes;

		private readonly DiscoveryClient _discovetClient;

		public EurekaProxyConfigProvider(IDiscoveryClient client)
		{
			_discovetClient = client as DiscoveryClient;

			_routes = new List<RouteConfig>
			{
				new RouteConfig()
				{
					RouteId = "category_route",
					ClusterId = "ServiceCategory",
					Match = new RouteMatch
					{
						Path = "g/{**catch-all}"
					},
					Transforms = new List<Dictionary<string, string>>
					{
						new Dictionary<string, string>
						{
							{ "PathPattern", "{**catch-all}" }
						}
					}
				},

				new RouteConfig()
				{
					RouteId = "course_route",
					ClusterId = "ServiceCourse",
					Match = new RouteMatch
					{
						Path = "c/{**catch-all}"
					},
					Transforms = new List<Dictionary<string, string>>
					{
						new Dictionary<string, string>
						{
							{ "PathPattern", "{**catch-all}" }
						}
					}
				}
			};

			PopulateConfig();
		}

		private void PopulateConfig()
		{
			List<ClusterConfig> clusters = new List<ClusterConfig>();

			var apps = _discovetClient.Applications.GetRegisteredApplications();
			foreach (var app in apps)
			{
				Dictionary<string, DestinationConfig> destinations = new Dictionary<string, DestinationConfig>();
				foreach (var item in app.Instances)
				{
					destinations.Add(
						item.InstanceId,
						new DestinationConfig
						{
							Address = $"http://{item.HostName}:{item.Port}/"
						});
				}

				IReadOnlyDictionary<string, DestinationConfig> readonlyDestinations = destinations;
				var newCluster = new ClusterConfig
				{
					LoadBalancingPolicy = "RoundRobin",
					ClusterId = app.Name,
					Destinations = readonlyDestinations
				};
				clusters.Add(newCluster);
			}

			var temp = _config;
			_config = new EurekaProxyConfig(_routes, clusters);
			temp?.SignalChange();
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			if (!stoppingToken.IsCancellationRequested)
			{
				PopulateConfig();
				await Task.Delay(10000, stoppingToken);
			}
		}
	}
}
