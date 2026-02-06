using Consul;
namespace RedPanda.Service.Governance
{
    public interface IConsulClientFactory
    {
        IConsulClient Create();
    }
}
