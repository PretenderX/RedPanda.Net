using System.Collections.Generic;

namespace RedPanda.Service.Governance
{
    public class ServiceDescription
    {
        public const string SectionName = "ServiceGovernance";

        public string ServiceSpace { get; set; }

        public string ServiceName { get; set; }

        public string FullServiceNameFormat { get; set; } = ServiceGovernanceConsts.DefaultFullServiceNameFormat;

        public string FullServiceName => string.Format(FullServiceNameFormat, ServiceSpace, ServiceName);

        public string ServiceAliases { get; set; }

        /// <summary>
        /// http or https or ws or wss
        /// </summary>
        public string ServiceSchema { get; set; } = ServiceGovernanceConsts.DefaultServiceSchema;

        public string ServiceHost { get; set; }

        public int ServicePort { get; set; } = ServiceGovernanceConsts.DefaultServicePort;

        public string ServiceVirtualDirectory { get; set; }

        public string ServiceHealthCheckRoute { get; set; }

        public string ServiceAddress
        {
            get
            {
                var serviceSchema = string.IsNullOrEmpty(ServiceSchema) ? ServiceGovernanceConsts.DefaultServiceSchema : ServiceSchema;
                var address = $"{serviceSchema}://{ServiceHost}";
                var port = ServicePort == default ? ServiceGovernanceConsts.DefaultServicePort : ServicePort;
                var virtualDirectory = ServiceVirtualDirectory?.Trim('/');

                if (port > 0 && port != 80 && port != 443)
                {
                    address = $"{address}:{port}";
                }

                if (!string.IsNullOrEmpty(virtualDirectory))
                {
                    address = $"{address}/{virtualDirectory}";
                }

                return address;
            }
        }

        public string HealthCheckUrl => string.IsNullOrEmpty(ServiceHealthCheckRoute) ? ServiceAddress : $"{ServiceAddress}/{ServiceHealthCheckRoute.Trim('/')}";

        public int ServiceCheckInterval { get; set; } = ServiceGovernanceConsts.DefaultServiceCheckInterval;

        public int ServiceCheckTimeout { get; set; } = ServiceGovernanceConsts.DefaultServiceCheckTimeout;

        public int? DeregisterCriticalServiceAfter { get; set; }

        public string[] RegistrationServiceNames
        {
            get
            {
                var serviceNames = new List<string> { FullServiceName };

                if (string.IsNullOrEmpty(ServiceAliases))
                {
                    return serviceNames.ToArray();
                }

                foreach (var serviceAlias in ServiceAliases.Split(','))
                {
                    var trimmedAlias = serviceAlias.Trim();

                    if (!string.IsNullOrEmpty(trimmedAlias))
                    {
                        serviceNames.Add(string.Format(FullServiceNameFormat, ServiceSpace, trimmedAlias));
                    }
                }

                return serviceNames.ToArray();
            }
        }
    }
}
