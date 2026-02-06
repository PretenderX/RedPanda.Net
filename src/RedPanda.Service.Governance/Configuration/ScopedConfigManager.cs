using Consul;
using RedPanda.Service.Governance.Common;
using RedPanda.Service.Governance.Exceptions;
using System.Text;
using System.Threading.Tasks;

namespace RedPanda.Service.Governance.Configuration
{
    public class ScopedConfigManager : IScopedConfigManager
    {
        protected readonly IJsonProvider JsonConfigProvider;
        protected readonly IConsulClient _consulClient;

        public ScopedConfigManager(IConsulClient consulClient)
        {
            _consulClient = consulClient;
            JsonConfigProvider = ServiceGovernanceConfig.JsonProvider;
        }

        public Task<bool> PutSettingToLocalSpaceAsync(string key, string value)
        {
            VallidateSettingKey(key);

            string localServiceScope = GetLocalServiceScope();

            return PutSettingAsync(key, value, localServiceScope);
        }

        public async Task<bool> PutSettingAsync(string key, string value, string serviceSpace = null)
        {
            VallidateSettingKey(key);

            string scopedKey = GetScopedKey(key, serviceSpace);
            var putPair = new KVPair(scopedKey)
            {
                Value = Encoding.UTF8.GetBytes(value)
            };

            var result = await _consulClient.KV.Put(putPair);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.Response;
            }

            return false;
        }

        public Task<string> GetSettingOfLocalSpaceAsync(string key)
        {
            VallidateSettingKey(key);

            string localServiceScope = GetLocalServiceScope();

            return GetSettingAsync(key, localServiceScope);
        }

        public async Task<string> GetSettingAsync(string key, string serviceSpace = null)
        {
            VallidateSettingKey(key);

            string scopedKey = GetScopedKey(key, serviceSpace);

            var result = await _consulClient.KV.Get(scopedKey);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Encoding.UTF8.GetString(result.Response.Value, 0, result.Response.Value.Length);
            }

            return null;
        }

        public async Task<TValue> GetSettingOfLocalSpaceAsync<TValue>(string key)
        {
            VallidateSettingKey(key);

            var value = await GetSettingOfLocalSpaceAsync(key);

            return value.As<TValue>();
        }

        public async Task<TValue> GetSettingAsync<TValue>(string key, string serviceSpace = null)
        {
            VallidateSettingKey(key);

            var value = await GetSettingAsync(key, serviceSpace);

            return value.As<TValue>();
        }

        public Task<string> GetJsonSettingOfLocalSpaceAsync(string key, string semicolonsJoinedPropertyNames)
        {
            VallidateSettingKey(key);

            return GetJsonSettingOfLocalSpaceAsync<string>(key, semicolonsJoinedPropertyNames);
        }

        public Task<TValue> GetJsonSettingOfLocalSpaceAsync<TValue>(string key, string jsonPath)
        {
            VallidateSettingKey(key);

            string localServiceScope = GetLocalServiceScope();

            return GetJsonSettingAsync<TValue>(key, jsonPath, localServiceScope);
        }

        public Task<string> GetJsonSettingAsync(string key, string semicolonsJoinedPropertyNames, string serviceSpace = null)
        {
            VallidateSettingKey(key);

            return GetJsonSettingAsync(key, semicolonsJoinedPropertyNames, serviceSpace);
        }

        public async Task<TValue> GetJsonSettingAsync<TValue>(string key, string semicolonsJoinedPropertyNames, string serviceSpace = null)
        {
            VallidateSettingKey(key);

            string scopedKey = GetScopedKey(key, serviceSpace);

            var result = await _consulClient.KV.Get(scopedKey);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var returnValue = JsonConfigProvider.GetValueFromJson<TValue>(result.Response.Value, semicolonsJoinedPropertyNames);

                return returnValue;
            }

            return default;
        }

        public Task<bool> ImportConfigToLocalSpaceAsync<TConfig>(string key, TConfig obj) where TConfig : new()
        {
            VallidateSettingKey(key);

            string localServiceScope = GetLocalServiceScope();

            return ImportConfigAsync(key, obj, localServiceScope);
        }

        public async Task<bool> ImportConfigAsync<TConfig>(string key, TConfig obj, string serviceSpace = null) where TConfig : new()
        {
            VallidateSettingKey(key);

            string scopedKey = GetScopedKey(key, serviceSpace);
            var putPair = new KVPair(scopedKey)
            {
                Value = JsonConfigProvider.SerializeToUtf8Bytes(obj, false)
            };

            var result = await _consulClient.KV.Put(putPair);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.Response;
            }

            return false;
        }

        public Task<TConfig> LoadConfigOfLocalSpaceAsync<TConfig>(string key) where TConfig : new()
        {
            VallidateSettingKey(key);

            string localServiceScope = GetLocalServiceScope();

            return LoadConfigAsync<TConfig>(key, localServiceScope);
        }

        public async Task<TConfig> LoadConfigAsync<TConfig>(string key, string serviceSpace = null) where TConfig : new()
        {
            VallidateSettingKey(key);

            string scopedKey = GetScopedKey(key, serviceSpace);

            var result = await _consulClient.KV.Get(scopedKey);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var returnValue = JsonConfigProvider.Deserialize<TConfig>(result.Response.Value);

                return returnValue;
            }

            return default;
        }

        public Task<string> ExportConfigAsJsonOfLocalSpaceAsync<TConfig>(string key, bool indented = false) where TConfig : new()
        {
            VallidateSettingKey(key);

            string localServiceScope = GetLocalServiceScope();

            return ExportConfigAsJsonAsync<TConfig>(key, indented, localServiceScope);
        }

        public async Task<string> ExportConfigAsJsonAsync<TConfig>(string key, bool indented = false, string serviceSpace = null) where TConfig : new()
        {
            VallidateSettingKey(key);

            string scopedKey = GetScopedKey(key, serviceSpace);

            var result = await _consulClient.KV.Get(scopedKey);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var returnValue = JsonConfigProvider.DeserializeUtf8BytesToJson<TConfig>(result.Response.Value, indented);

                return returnValue;
            }

            return default;
        }

        public Task<bool> DeleteSettingKeyOfLocalSpaceAsync(string key)
        {
            VallidateSettingKey(key);

            string localServiceScope = GetLocalServiceScope();

            return DeleteSettingKeyAsync(key, localServiceScope);
        }

        public async Task<bool> DeleteSettingKeyAsync(string key, string serviceSpace = null)
        {
            VallidateSettingKey(key);

            string scopedKey = GetScopedKey(key, serviceSpace);

            var result = await _consulClient.KV.Delete(scopedKey);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.Response;
            }

            return false;
        }

        public Task<bool> DeleteAllSettingKeysOfLocalSpace(string path = null)
        {
            string localServiceScope = GetLocalServiceScope();

            return DeleteAllSettingKeys(localServiceScope, path);
        }

        public async Task<bool> DeleteAllSettingKeys(string serviceSpace, string path = null)
        {
            string scopedKey = GetScopedKey(path, serviceSpace);

            var result = await _consulClient.KV.DeleteTree(scopedKey);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.Response;
            }

            return false;
        }

        private static void VallidateSettingKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new SettingKeyIsNullOrEmptyException();
            }
        }

        private static string GetLocalServiceScope()
        {
            var localServiceSpace = LocalServiceDescriptionProvider.GetLocalServiceSpace();
            var serviceScope = string.IsNullOrEmpty(localServiceSpace) ? null : localServiceSpace;

            return serviceScope;
        }

        private static string GetScopedKey(string key, string serviceScope)
        {
            return string.IsNullOrEmpty(serviceScope) ? key : $"{serviceScope}/{key}";
        }
    }
}
