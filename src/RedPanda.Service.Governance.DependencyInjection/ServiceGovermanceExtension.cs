using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedPanda.Service.Governance.Configuration;
using RedPanda.Service.Governance.Discovery;
using RedPanda.Service.Governance.Registration;
using System;
using System.Collections.Generic;

namespace RedPanda.Service.Governance.DependencyInjection
{
    public static class ServiceGovermanceExtension
    {
        public static IServiceCollection AddServiceGovermance(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IConsulClientFactory, ConsulClientFactory>();
            serviceCollection.AddSingleton(sp => sp.GetRequiredService<IConsulClientFactory>().Create());
            serviceCollection.AddSingleton<IServiceRegistration, ServiceRegistration>();
            serviceCollection.AddSingleton<IServiceDiscovery, ServiceDiscovery>();
            serviceCollection.AddSingleton<IScopedConfigManager, ScopedConfigManager>();

            return serviceCollection;
        }

        public static IServiceProvider UseServiceGovermance(this IServiceProvider serviceProvider, Action<Dictionary<string, string>> appendMetaAction = null)
        {
            var hostApplicationLifetime = serviceProvider.GetService<IHostApplicationLifetime>();
            var stoppingCalled = false;

            hostApplicationLifetime?.ApplicationStarted.Register(() => serviceProvider.GetService<IServiceRegistration>()?.RegisterSelf(appendMetaAction));
            hostApplicationLifetime?.ApplicationStopping.Register(() =>
            {
                if (stoppingCalled)
                {
                    return;
                }

                stoppingCalled = true;

                serviceProvider.GetService<IServiceRegistration>()?.DeregisterSelf();
            });

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                if (stoppingCalled)
                {
                    return;
                }

                stoppingCalled = true;

                serviceProvider.GetService<IServiceRegistration>()?.DeregisterSelf();
            };

            return serviceProvider;
        }
    }
}
