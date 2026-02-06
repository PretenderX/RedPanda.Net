using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedPanda.Service.Governance.Registration
{
    public interface IServiceRegistration
    {
        void RegisterSelf(Action<Dictionary<string, string>> appendMetaAction = null);

        Task RegisterSelfAsync(Action<Dictionary<string, string>> appendMetaAction = null);

        void DeregisterSelf();

        Task DeregisterSelfAsync();
    }
}
