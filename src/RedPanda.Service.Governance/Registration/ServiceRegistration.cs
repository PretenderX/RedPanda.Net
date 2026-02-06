using Consul;
using RedPanda.Service.Governance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedPanda.Service.Governance.Registration
{
    public class ServiceRegistration : IServiceRegistration
    {
        private readonly IConsulClient _consulClient;

        public ServiceRegistration(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        public void RegisterSelf(Action<Dictionary<string, string>> appendMetaAction = null)
        {
            RegisterSelfAsync(appendMetaAction).Wait();
        }

        public async Task RegisterSelfAsync(Action<Dictionary<string, string>> appendMetaAction = null)
        {
            var serviceDescription = LocalServiceDescriptionProvider.Build();
            var serviceMeta = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(serviceDescription.ServiceSchema))
            {
                serviceMeta.Add(ServiceGovernanceConsts.ServiceSchema, serviceDescription.ServiceSchema);
            }

            if (!string.IsNullOrEmpty(serviceDescription.ServiceVirtualDirectory))
            {
                serviceMeta.Add(ServiceGovernanceConsts.ServiceVirtualDirectory, serviceDescription.ServiceVirtualDirectory);
            }

            appendMetaAction?.Invoke(serviceMeta);

            var serviceCheck = new AgentServiceCheck
            {
                Interval = TimeSpan.FromSeconds(serviceDescription.ServiceCheckInterval),
                HTTP = serviceDescription.HealthCheckUrl,
                Timeout = TimeSpan.FromSeconds(serviceDescription.ServiceCheckTimeout),
            };

            if (serviceDescription.DeregisterCriticalServiceAfter.HasValue && serviceDescription.DeregisterCriticalServiceAfter.Value > 0)
            {
                serviceCheck.DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(serviceDescription.DeregisterCriticalServiceAfter.Value);
            }

            foreach (var serviceName in serviceDescription.RegistrationServiceNames)
            {
                var serviceRegistration = new AgentServiceRegistration
                {
                    Checks = new[] { serviceCheck },
                    ID = Guid.NewGuid().ToString(),
                    Name = serviceName,
                    Address = serviceDescription.ServiceHost,
                    Port = serviceDescription.ServicePort,
                    Meta = serviceMeta,
                    Tags = new[] { serviceDescription.ServiceSpace, $"urlprefix-/{serviceName}" },
                };

                await DeregisterServiceAsync(serviceName, serviceDescription);

                var result = await _consulClient.Agent.ServiceRegister(serviceRegistration);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"注册服务成功: {serviceRegistration.ID}");
                    
                }
                else
                {
                    Console.WriteLine($"注册服务失败: {serviceRegistration.ID}");
                }
            }

        }

        public void DeregisterSelf()
        {
            DeregisterSelfAsync().Wait();
        }

        public async Task DeregisterSelfAsync()
        {
            var serviceDescription = LocalServiceDescriptionProvider.Build();

            foreach (var serviceName in serviceDescription.RegistrationServiceNames)
            {
                await DeregisterServiceAsync(serviceName, serviceDescription);
            }
        }

        private async Task DeregisterServiceAsync(string serviceName, ServiceDescription serviceDescription)
        {
            var existingServices = (await _consulClient.Catalog.Service(serviceName)).Response;

            foreach (var service in existingServices.Where(s => s.ServiceAddress == serviceDescription.ServiceHost && s.ServicePort == serviceDescription.ServicePort))
            {
                await _consulClient.Agent.ServiceDeregister(service.ServiceID);
                await _consulClient.Catalog.Deregister(new CatalogDeregistration
                {
                    Node = service.Node,
                    ServiceID = service.ServiceID
                });

                Console.WriteLine($"注销服务: {service.ServiceID}");
            }
        }
    }
}
