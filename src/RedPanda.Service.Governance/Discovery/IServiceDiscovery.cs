using Consul;
using System.Threading.Tasks;

namespace RedPanda.Service.Governance.Discovery
{
    public interface IServiceDiscovery
    {
        /// <summary>
        /// 使用指定的ServiceSpace的值获取服务地址
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceSpace"></param>
        /// <param name="serviceTag"></param>
        /// <returns></returns>
        Task<string[]> GetServiceAddressesAsync(string serviceName, string serviceSpace = null, string serviceTag = null);

        /// <summary>
        /// 使用本地配置的ServiceSpace的值获取服务地址
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceTag"></param>
        /// <returns></returns>
        Task<string[]> GetServiceAddressesByLocalSpaceAsync(string serviceName, string serviceTag = null);

        /// <summary>
        /// 使用指定的ServiceSpace的值获取健康服务目录
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceSpace"></param>
        /// <param name="serviceTag"></param>
        /// <returns></returns>
        Task<ServiceEntry[]> GetHealthyServicesAsync(string serviceName, string serviceSpace = null, string serviceTag = null);

        /// <summary>
        /// 使用本地配置的ServiceSpace的值获取健康服务目录
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceTag"></param>
        /// <returns></returns>
        Task<ServiceEntry[]> GetHealthyServicesByLocalSpaceAsync(string serviceName, string serviceTag = null);

        /// <summary>
        /// 使用指定的ServiceSpace的值获取服务目录
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceSpace"></param>
        /// <param name="serviceTag"></param>
        /// <returns></returns>
        Task<CatalogService[]> GetCatalogServicesAsync(string serviceName, string serviceSpace = null, string serviceTag = null);

        /// <summary>
        /// 使用本地配置的ServiceSpace的值获取服务目录
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceTag"></param>
        /// <returns></returns>
        Task<CatalogService[]> GetCatalogServicesByLocalSpaceAsync(string serviceName, string serviceTag = null);
    }
}
