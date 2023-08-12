using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Transforms;

namespace YARP.LoadBalancer
{
    public partial class CustomProxyConfigProvider : IProxyConfigProvider
    {
        private CustomMemoryConfig _config;

        public CustomProxyConfigProvider()
        {
            var routeConfigs = CreateRouteConfigs();
            var clusterConfigs = CreateClusterConfigs();

            _config = new CustomMemoryConfig(routeConfigs, clusterConfigs);
        }

        public IProxyConfig GetConfig() => _config;

        public void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            var oldConfig = _config;
            _config = new CustomMemoryConfig(routes, clusters);
            oldConfig.SignalChange();
        }

        private IEnumerable<string> GetMicroservicesFromDockerCompose()
        {
            return new List<string>
            {
                "user.api",
                "notification.api",
                "authentication.api",
                "gamification.api",
                "dreamdata.api",
                "dreamreport.api",
                "dreamscore.api",
                "foodanalyzer.api",
                "dreamanalyzer.api",
                "blog.api"
            };
        }

        private List<RouteConfig> CreateRouteConfigs()
        {
            var routeConfigs = new List<RouteConfig>();

            foreach (var service in GetMicroservicesFromDockerCompose())
            {
                var routeConfig = new RouteConfig
                {
                    RouteId = $"{service}_route",
                    ClusterId = $"{service}_cluster",
                    Match = new RouteMatch
                    {
                        Path = $"/{service}{{**catch-all}}"
                    }
                };

                routeConfig = routeConfig
                    .WithTransformPathRemovePrefix(prefix: $"/{service}")
                    .WithTransformResponseHeader(headerName: "Source", value: "YARP", append: true);

                routeConfigs.Add(routeConfig);
            }

            return routeConfigs;
        }

        private List<ClusterConfig> CreateClusterConfigs()
        {
            var clusterConfigs = new List<ClusterConfig>();

            foreach (var service in GetMicroservicesFromDockerCompose())
            {
                var destinationConfig = new DestinationConfig { Address = $"http://{service}:80" };

                var clusterConfig = new ClusterConfig
                {
                    ClusterId = $"{service}_cluster",
                    LoadBalancingPolicy = LoadBalancingPolicies.RoundRobin,
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        { $"{service}_destination1", new DestinationConfig { Address = $"http://{service}:80" } },
                        { $"{service}_destination2", new DestinationConfig { Address = $"https://localhost:5002/" } }
                    }
                };

                clusterConfigs.Add(clusterConfig);
            }

            return clusterConfigs;
        }
    }
}
