using Consul;
using RedPanda.Service.Governance.Common;
using System.Linq;
using System.Threading.Tasks;

namespace RedPanda.Service.Governance.Discovery
{
    public class ServiceDiscovery : IServiceDiscovery
    {
        protected readonly IConsulClient _consulClient;

        public ServiceDiscovery(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        public async Task<CatalogService[]> GetCatalogServicesAsync(string serviceName, string serviceSpace = null, string serviceTag = null)
        {
            var scopedServiceName = GetScopedServiceName(serviceName, serviceSpace);

            var queryResult = string.IsNullOrEmpty(serviceTag) ? await _consulClient.Catalog.Service(scopedServiceName) : await _consulClient.Catalog.Service(scopedServiceName, serviceTag);

            return queryResult.Response;
        }

        public Task<CatalogService[]> GetCatalogServicesByLocalSpaceAsync(string serviceName, string serviceTag = null)
        {
            var localServiceSpace = LocalServiceDescriptionProvider.GetLocalServiceSpace();

            return GetCatalogServicesAsync(serviceName, localServiceSpace, serviceTag);
        }

        public async Task<ServiceEntry[]> GetHealthyServicesAsync(string serviceName, string serviceSpace = null, string serviceTag = null)
        {
            var scopedServiceName = GetScopedServiceName(serviceName, serviceSpace);

            var queryResult = string.IsNullOrEmpty(serviceTag) ? await _consulClient.Health.Service(scopedServiceName) : await _consulClient.Health.Service(scopedServiceName, serviceTag);

            return queryResult.Response;
        }

        public Task<ServiceEntry[]> GetHealthyServicesByLocalSpaceAsync(string serviceName, string serviceTag = null)
        {
            var localServiceSpace = LocalServiceDescriptionProvider.GetLocalServiceSpace();

            return GetHealthyServicesAsync(serviceName, localServiceSpace, serviceTag);
        }

        public async Task<string[]> GetServiceAddressesAsync(string serviceName, string serviceSpace = null, string serviceTag = null)
        {
            var healthyServices = await GetHealthyServicesAsync(serviceName, serviceSpace, serviceTag);

            return healthyServices.Select(s => s.Service.GetHealthyServiceAddress()).ToArray();
        }

        public Task<string[]> GetServiceAddressesByLocalSpaceAsync(string serviceName, string serviceTag = null)
        {
            var localServiceSpace = LocalServiceDescriptionProvider.GetLocalServiceSpace();

            return GetServiceAddressesAsync(serviceName, localServiceSpace, serviceTag);
        }

        private string GetScopedServiceName(string serviceName, string serviceSpace)
        {
            var serviceFullNameFormat = LocalServiceDescriptionProvider.GetFullServiceNameFormat();
            return string.IsNullOrEmpty(serviceSpace) ? serviceName : string.Format(serviceFullNameFormat, serviceSpace, serviceName);
        }
    }
}
