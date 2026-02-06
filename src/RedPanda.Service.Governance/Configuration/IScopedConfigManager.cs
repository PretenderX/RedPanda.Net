using System.Threading.Tasks;

namespace RedPanda.Service.Governance.Configuration
{
    public interface IScopedConfigManager
    {
        /// <summary>
        /// 将配置项写入到本地配置的ServiceSpace中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> PutSettingToLocalSpaceAsync(string key, string value);

        /// <summary>
        /// 将配置项写入到指定的ServiceSpace中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<bool> PutSettingAsync(string key, string value, string serviceSpace = null);

        /// <summary>
        /// 从本地配置的ServiceSpace中获取配置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetSettingOfLocalSpaceAsync(string key);

        /// <summary>
        /// 从指定的ServiceSpace的中获取配置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<string> GetSettingAsync(string key, string serviceSpace = null);

        /// <summary>
        /// 从本地配置的ServiceSpace中获取配置值
        /// </summary>
        /// <typeparam name="TValue">系统类型或枚举类型</typeparam>
        /// <param name="key"></param>
        /// <param name="serviceScope"></param>
        /// <returns></returns>
        Task<TValue> GetSettingOfLocalSpaceAsync<TValue>(string key);

        /// <summary>
        /// 从指定的ServiceSpace的获取配置值
        /// </summary>
        /// <typeparam name="TValue">系统类型或枚举类型，有限支持</typeparam>
        /// <param name="key"></param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<TValue> GetSettingAsync<TValue>(string key, string serviceSpace = null);

        /// <summary>
        /// 从本地配置的ServiceSpace中获取Json配置值
        /// </summary>
        /// <param name="key">系统类型或枚举类型</param>
        /// <param name="semicolonsJoinedPropertyNames">":"连接的属性名称</param>
        /// <returns></returns>
        Task<string> GetJsonSettingOfLocalSpaceAsync(string key, string semicolonsJoinedPropertyNames);

        /// <summary>
        /// 从本地配置的ServiceSpace中获取Json配置值
        /// </summary>
        /// <typeparam name="TValue">系统类型或枚举类型</typeparam>
        /// <param name="key"></param>
        /// <param name="semicolonsJoinedPropertyNames">":"连接的属性名称</param>
        /// <returns></returns>
        Task<TValue> GetJsonSettingOfLocalSpaceAsync<TValue>(string key, string semicolonsJoinedPropertyNames);

        /// <summary>
        /// 从指定的ServiceSpace中获取Json配置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="semicolonsJoinedPropertyNames">":"连接的属性名称</param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<string> GetJsonSettingAsync(string key, string semicolonsJoinedPropertyNames, string serviceSpace = null);

        /// <summary>
        /// 从指定的ServiceSpace中获取Json配置值
        /// </summary>
        /// <typeparam name="TValue">配置项类型</typeparam>
        /// <param name="key"></param>
        /// <param name="semicolonsJoinedPropertyNames">":"连接的属性名称</param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<TValue> GetJsonSettingAsync<TValue>(string key, string semicolonsJoinedPropertyNames, string serviceSpace = null);

        /// <summary>
        /// 将配置类的实例作为配置项导入到本地配置的ServiceSpace中
        /// </summary>
        /// <typeparam name="TConfig">配置项类型</typeparam>
        /// <param name="key"></param>
        /// <param name="obj">配置类的实例</param>
        /// <returns></returns>
        Task<bool> ImportConfigToLocalSpaceAsync<TConfig>(string key, TConfig obj) where TConfig : new();

        /// <summary>
        /// 将配置类的实例作为配置项导入到指定的ServiceSpace中
        /// </summary>
        /// <typeparam name="TConfig">配置项类型</typeparam>
        /// <param name="key"></param>
        /// <param name="obj">配置类的实例</param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<bool> ImportConfigAsync<TConfig>(string key, TConfig obj, string serviceSpace = null) where TConfig : new();

        /// <summary>
        /// 从本地配置的ServiceSpace中加载配置类的实例
        /// </summary>
        /// <typeparam name="TConfig">配置项类型</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<TConfig> LoadConfigOfLocalSpaceAsync<TConfig>(string key) where TConfig : new();

        /// <summary>
        /// 从指定的ServiceSpace中加载配置类的实例
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="key"></param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<TConfig> LoadConfigAsync<TConfig>(string key, string serviceSpace = null) where TConfig : new();

        /// <summary>
        /// 从本地配置的ServiceSpace中导出配置类所序列化的Json
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="key"></param>
        /// <param name="indented">是否缩进</param>
        /// <returns></returns>
        Task<string> ExportConfigAsJsonOfLocalSpaceAsync<TConfig>(string key, bool indented = false) where TConfig : new();

        /// <summary>
        /// 从指定的ServiceSpace中导出配置类所序列化的Json
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="key"></param>
        /// <param name="indented">是否缩进</param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<string> ExportConfigAsJsonAsync<TConfig>(string key, bool indented = false, string serviceSpace = null) where TConfig : new();

        /// <summary>
        /// 删除本地配置的ServiceSpace中的指定配置项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> DeleteSettingKeyOfLocalSpaceAsync(string key);

        /// <summary>
        /// 删除指定的ServiceSpace中的指定配置项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<bool> DeleteSettingKeyAsync(string key, string serviceSpace = null);

        /// <summary>
        /// 删除本地配置的ServiceSpace中的所有配置项
        /// </summary>
        /// <param name="path">子路径</param>
        /// <returns></returns>
        Task<bool> DeleteAllSettingKeysOfLocalSpace(string path = null);

        /// <summary>
        /// 删除指定的ServiceSpace中的所有配置项
        /// </summary>
        /// <param name="serviceSpace"></param>
        /// <param name="path">子路径</param>
        /// <returns></returns>
        Task<bool> DeleteAllSettingKeys(string serviceSpace, string path = null);
    }
}
