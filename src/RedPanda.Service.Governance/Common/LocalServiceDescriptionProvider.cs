﻿using System;

namespace RedPanda.Service.Governance.Common
{
    public static class LocalServiceDescriptionProvider
    {
        public static ServiceDescription Build()
        {
            return new ServiceDescription
            {
                ServiceSpace = GetAppSetting(ServiceGovernanceConsts.ServiceSpace),
                ServiceName = GetAppSetting(ServiceGovernanceConsts.ServiceName, false),
                ServiceAliases = GetAppSetting(ServiceGovernanceConsts.ServiceAliases),
                ServiceSchema = GetAppSetting(ServiceGovernanceConsts.ServiceSchema),
                Host = GetAppSetting(ServiceGovernanceConsts.ServiceHost, false),
                Port = Convert.ToInt32(GetAppSetting(ServiceGovernanceConsts.ServicePort, false)),
                VirtualDirectory = GetAppSetting(ServiceGovernanceConsts.ServiceVirtualDirectory),
                HealthCheckRoute = GetAppSetting(ServiceGovernanceConsts.ServiceHealthRoute),
            };
        }

        public static string GetLocalServiceSpace()
        {
            return GetAppSetting(ServiceGovernanceConsts.ServiceSpace);
        }

        private static string GetAppSetting(string name, bool isOptional = true) => ServiceGovernanceConfig.ServiceGovernanceConfigProvider.GetAppSetting(name, isOptional);
    }
}
