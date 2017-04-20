using System;
using System.Configuration;
using System.Xml;

namespace Nop.Core.Configuration
{
    /// <summary>
    /// 表示NopConfig
    /// </summary>
    public partial class NopConfig : IConfigurationSectionHandler
    {
        /// <summary>
        /// 创建一个配置部分处理程序。
        /// </summary>
        /// <param name="parent">父对象</param>
        /// <param name="configContext">配置上下文对象。</param>
        /// <param name="section">XML节点</param>
        /// <returns>创建的部分处理程序对象。</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new NopConfig();

            var startupNode = section.SelectSingleNode("Startup");
            config.IgnoreStartupTasks = GetBool(startupNode, "IgnoreStartupTasks");
           
            var redisCachingNode = section.SelectSingleNode("RedisCaching");
            config.RedisCachingEnabled = GetBool(redisCachingNode, "Enabled");
            config.RedisCachingConnectionString = GetString(redisCachingNode, "ConnectionString");

            var userAgentStringsNode = section.SelectSingleNode("UserAgentStrings");
            config.UserAgentStringsPath = GetString(userAgentStringsNode, "databasePath");
            config.CrawlerOnlyUserAgentStringsPath = GetString(userAgentStringsNode, "crawlersOnlyDatabasePath");

            var supportPreviousNopcommerceVersionsNode = section.SelectSingleNode("SupportPreviousNopcommerceVersions");
            config.SupportPreviousNopcommerceVersions = GetBool(supportPreviousNopcommerceVersionsNode, "Enabled");
            
            var webFarmsNode = section.SelectSingleNode("WebFarms");
            config.MultipleInstancesEnabled = GetBool(webFarmsNode, "MultipleInstancesEnabled");
            config.RunOnAzureWebApps = GetBool(webFarmsNode, "RunOnAzureWebApps");

            var azureBlobStorageNode = section.SelectSingleNode("AzureBlobStorage");
            config.AzureBlobStorageConnectionString = GetString(azureBlobStorageNode, "ConnectionString");
            config.AzureBlobStorageContainerName = GetString(azureBlobStorageNode, "ContainerName");
            config.AzureBlobStorageEndPoint = GetString(azureBlobStorageNode, "EndPoint");

            var installationNode = section.SelectSingleNode("Installation");
            config.DisableSampleDataDuringInstallation = GetBool(installationNode, "DisableSampleDataDuringInstallation");
            config.UseFastInstallationService = GetBool(installationNode, "UseFastInstallationService");
            config.PluginsIgnoredDuringInstallation = GetString(installationNode, "PluginsIgnoredDuringInstallation");

            return config;
        }

        private string GetString(XmlNode node, string attrName)
        {
            return SetByXElement<string>(node, attrName, Convert.ToString);
        }

        private bool GetBool(XmlNode node, string attrName)
        {
            return SetByXElement<bool>(node, attrName, Convert.ToBoolean);
        }

        private T SetByXElement<T>(XmlNode node, string attrName, Func<string, T> converter)
        {
            if (node == null || node.Attributes == null) return default(T);
            var attr = node.Attributes[attrName];
            if (attr == null) return default(T);
            var attrVal = attr.Value;
            return converter(attrVal);
        }

        /// <summary>
        /// 指示是否忽略启动任务
        /// </summary>
        public bool IgnoreStartupTasks { get; private set; }

        /// <summary>
        /// Path to database with user agent strings
        /// </summary>
        public string UserAgentStringsPath { get; private set; }

        /// <summary>
        /// 具有爬网程序的数据库路径只有用户代理字符串
        /// </summary>
        public string CrawlerOnlyUserAgentStringsPath { get; private set; }



        /// <summary>
        /// 指示是否应该使用Redis服务器进行缓存（而不是默认的内存中缓存）
        /// </summary>
        public bool RedisCachingEnabled { get; private set; }
        /// <summary>
        /// Redis连接字符串。 在启用Redis缓存时使用
        /// </summary>
        public string RedisCachingConnectionString { get; private set; }



        /// <summary>
        ///指示是否应该支持以前的nopCommerce版本（它可以稍微提高性能）
        /// </summary>
        public bool SupportPreviousNopcommerceVersions { get; private set; }



        /// <summary>
        /// 指示站点是否在多个实例上运行的值（例如，Web场，具有多个实例的Windows Azure等）。
        /// 如果您在Azure上运行但是仅使用一个实例，请不要启用它
        /// </summary>
        public bool MultipleInstancesEnabled { get; private set; }

        /// <summary>
        ///指示站点是否在Windows Azure Web Apps上运行的值
        /// </summary>
        public bool RunOnAzureWebApps { get; private set; }

        /// <summary>
        /// Azure BLOB存储的连接字符串
        /// </summary>
        public string AzureBlobStorageConnectionString { get; private set; }
        /// <summary>
        ///Azure BLOB存储的容器名称
        /// </summary>
        public string AzureBlobStorageContainerName { get; private set; }
        /// <summary>
        /// Azure BLOB存储的终点
        /// </summary>
        public string AzureBlobStorageEndPoint { get; private set; }


        /// <summary>
        ///指示店主是否可以在安装过程中安装样本数据的值
        /// </summary>
        public bool DisableSampleDataDuringInstallation { get; private set; }
        /// <summary>
        /// 默认情况下，此设置应始终设置为“False”（仅适用于高级用户）
        /// </summary>
        public bool UseFastInstallationService { get; private set; }
        /// <summary>
        /// 在nopCommerce安装期间忽略的插件列表
        /// </summary>
        public string PluginsIgnoredDuringInstallation { get; private set; }
    }
}
