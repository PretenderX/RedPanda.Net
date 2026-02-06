using Autofac;
using RedPanda.Service.Governance.Configuration;
using RedPanda.Service.Governance.Discovery;
using RedPanda.Service.Governance.Registration;

namespace RedPanda.Service.Governance
{
    public static class ContainerBuilderExtensionOfServiceGovernance
    {
        public static ContainerBuilder UseServiceGovernance(this ContainerBuilder builder)
        {
            builder.RegisterType<ConsulClientFactory>().AsImplementedInterfaces().SingleInstance();
            builder.Register(ctx => ctx.Resolve<IConsulClientFactory>().Create()).SingleInstance();
            builder.RegisterType<ServiceRegistration>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ServiceDiscovery>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ScopedConfigManager>().AsImplementedInterfaces().SingleInstance();

            return builder;
        }
    }
}