using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using Nop.Core.ComponentModel;
using Nop.Core.Plugins;

//Contributor: Umbraco (http://www.umbraco.com). Thanks a lot! 
//SEE THIS POST for full details of what this does - http://shazwazza.com/post/Developing-a-plugin-framework-in-ASPNET-with-medium-trust.aspx

[assembly: PreApplicationStartMethod(typeof(PluginManager), "Initialize")]
namespace Nop.Core.Plugins
{
    /// <summary>
    /// 插件管理
    /// </summary>
    public class PluginManager
    {
        #region 常量
        /// <summary>
        /// 插件安装记录路径
        /// </summary>
        private const string InstalledPluginsFilePath = "~/App_Data/InstalledPlugins.txt";
        /// <summary>
        /// 插件路径
        /// </summary>
        private const string PluginsPath = "~/Plugins";
        /// <summary>
        /// 插件复制路径
        /// </summary>
        private const string ShadowCopyPath = "~/Plugins/bin";

        #endregion

        #region 字段
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        /// <summary>
        /// 影像复制文件夹信息
        /// </summary>
        private static DirectoryInfo _shadowCopyFolder;
        /// <summary>
        /// 启动时清除影像复制文件
        /// </summary>
        private static bool _clearShadowDirectoryOnStartup;

        #endregion

        #region 方法

        /// <summary>
        ///获取或设置已被影像复制的插件集合
        /// </summary>
        public static IEnumerable<PluginDescriptor> ReferencedPlugins { get; set; }

        /// <summary>
        /// 获取或设置与当前版本不兼容的所有插件的集合
        /// </summary>
        public static IEnumerable<string> IncompatiblePlugins { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            using (new WriteLockDisposable(Locker))
            {
                // TODO: Add verbose exception handling / raising here since this is happening on app startup and could
                // prevent app from starting altogether
                var pluginFolder = new DirectoryInfo(CommonHelper.MapPath(PluginsPath));
                _shadowCopyFolder = new DirectoryInfo(CommonHelper.MapPath(ShadowCopyPath));

                var referencedPlugins = new List<PluginDescriptor>();
                var incompatiblePlugins = new List<string>();

                _clearShadowDirectoryOnStartup = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ClearPluginsShadowDirectoryOnStartup"]) &&
                   Convert.ToBoolean(ConfigurationManager.AppSettings["ClearPluginsShadowDirectoryOnStartup"]);

                try
                {
                    var installedPluginSystemNames = PluginFileParser.ParseInstalledPluginsFile(GetInstalledPluginsFilePath());

                    Debug.WriteLine("Creating shadow copy folder and querying for dlls");
                    //确保创建文件夹
                    Directory.CreateDirectory(pluginFolder.FullName);
                    Directory.CreateDirectory(_shadowCopyFolder.FullName);

                    //获取bin下所有文件的列表
                    var binFiles = _shadowCopyFolder.GetFiles("*", SearchOption.AllDirectories);
                    if (_clearShadowDirectoryOnStartup)
                    {
                        //清除影像复制的插件
                        foreach (var f in binFiles)
                        {
                            Debug.WriteLine("Deleting " + f.Name);
                            try
                            {
                                File.Delete(f.FullName);
                            }
                            catch (Exception exc)
                            {
                                Debug.WriteLine("Error deleting file " + f.Name + ". Exception: " + exc);
                            }
                        }
                    }

                    //加载描述文件
                    foreach (var dfd in GetDescriptionFilesAndDescriptors(pluginFolder))
                    {
                        var descriptionFile = dfd.Key;
                        var pluginDescriptor = dfd.Value;

                        //确保插件版本支持
                        if (!pluginDescriptor.SupportedVersions.Contains(NopVersion.CurrentVersion, StringComparer.InvariantCultureIgnoreCase))
                        {
                            incompatiblePlugins.Add(pluginDescriptor.SystemName);
                            continue;
                        }

                        //一些验证
                        if (string.IsNullOrWhiteSpace(pluginDescriptor.SystemName))
                            throw new Exception(string.Format("A plugin '{0}' has no system name. Try assigning the plugin a unique name and recompiling.", descriptionFile.FullName));
                        if (referencedPlugins.Contains(pluginDescriptor))
                            throw new Exception(string.Format("A plugin with '{0}' system name is already defined", pluginDescriptor.SystemName));

                        //设置 'Installed' 属性
                        pluginDescriptor.Installed = installedPluginSystemNames
                            .FirstOrDefault(x => x.Equals(pluginDescriptor.SystemName, StringComparison.InvariantCultureIgnoreCase)) != null;

                        try
                        {
                            if (descriptionFile.Directory == null)
                                throw new Exception(string.Format("Directory cannot be resolved for '{0}' description file", descriptionFile.Name));
                            //获取插件中所有DLL的列表（不在bin目录下！）
                            var pluginFiles = descriptionFile.Directory.GetFiles("*.dll", SearchOption.AllDirectories)
                                //只需确保我们没有注册影像复制插件
                                .Where(x => !binFiles.Select(q => q.FullName).Contains(x.FullName))
                                .Where(x => IsPackagePluginFolder(x.Directory))
                                .ToList();

                            //其他插件描述信息
                            var mainPluginFile = pluginFiles
                                .FirstOrDefault(x => x.Name.Equals(pluginDescriptor.PluginFileName, StringComparison.InvariantCultureIgnoreCase));
                            pluginDescriptor.OriginalAssemblyFile = mainPluginFile;

                            //影像复制主要插件文件
                            pluginDescriptor.ReferencedAssembly = PerformFileDeploy(mainPluginFile);

                            //现在加载所有其他引用的程序集
                            foreach (var plugin in pluginFiles
                                .Where(x => !x.Name.Equals(mainPluginFile.Name, StringComparison.InvariantCultureIgnoreCase))
                                .Where(x => !IsAlreadyLoaded(x)))
                                    PerformFileDeploy(plugin);

                            //初始化插件类型（每个程序集只允许一个插件）
                            foreach (var t in pluginDescriptor.ReferencedAssembly.GetTypes())
                                if (typeof(IPlugin).IsAssignableFrom(t))
                                    if (!t.IsInterface)
                                        if (t.IsClass && !t.IsAbstract)
                                        {
                                            pluginDescriptor.PluginType = t;
                                            break;
                                        }

                            referencedPlugins.Add(pluginDescriptor);
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            //添加一个插件名称。 这样我们可以很容易地识别一个有问题的插件
                            var msg = string.Format("Plugin '{0}'. ", pluginDescriptor.FriendlyName);
                            foreach (var e in ex.LoaderExceptions)
                                msg += e.Message + Environment.NewLine;

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                        catch (Exception ex)
                        {
                            //添加一个插件名称。 这样我们可以很容易地识别一个有问题的插件
                            var msg = string.Format("Plugin '{0}'. {1}", pluginDescriptor.FriendlyName, ex.Message);

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msg = string.Empty;
                    for (var e = ex; e != null; e = e.InnerException)
                        msg += e.Message + Environment.NewLine;

                    var fail = new Exception(msg, ex);
                    throw fail;
                }


                ReferencedPlugins = referencedPlugins;
                IncompatiblePlugins = incompatiblePlugins;

            }
        }

        /// <summary>
        /// 标记插件安装
        /// </summary>
        /// <param name="systemName">插件系统名称</param>
        public static void MarkPluginAsInstalled(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
                throw new ArgumentNullException("systemName");

            var filePath = CommonHelper.MapPath(InstalledPluginsFilePath);
            if (!File.Exists(filePath))
                using (File.Create(filePath))
                {
                    //we use 'using' to close the file after it's created
                }


            var installedPluginSystemNames = PluginFileParser.ParseInstalledPluginsFile(GetInstalledPluginsFilePath());
            bool alreadyMarkedAsInstalled = installedPluginSystemNames
                                .FirstOrDefault(x => x.Equals(systemName, StringComparison.InvariantCultureIgnoreCase)) != null;
            if (!alreadyMarkedAsInstalled)
                installedPluginSystemNames.Add(systemName);
            PluginFileParser.SaveInstalledPluginsFile(installedPluginSystemNames,filePath);
        }

        /// <summary>
        ///将插件标记为已卸载
        /// </summary>
        /// <param name="systemName">插件系统名称</param>
        public static void MarkPluginAsUninstalled(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
                throw new ArgumentNullException("systemName");

            var filePath = CommonHelper.MapPath(InstalledPluginsFilePath);
            if (!File.Exists(filePath))
                using (File.Create(filePath))
                {
                    //we use 'using' to close the file after it's created
                }


            var installedPluginSystemNames = PluginFileParser.ParseInstalledPluginsFile(GetInstalledPluginsFilePath());
            bool alreadyMarkedAsInstalled = installedPluginSystemNames
                                .FirstOrDefault(x => x.Equals(systemName, StringComparison.InvariantCultureIgnoreCase)) != null;
            if (alreadyMarkedAsInstalled)
                installedPluginSystemNames.Remove(systemName);
            PluginFileParser.SaveInstalledPluginsFile(installedPluginSystemNames,filePath);
        }

        /// <summary>
        ///将所有插件标记为已卸载
        /// </summary>
        public static void MarkAllPluginsAsUninstalled()
        {
            var filePath = CommonHelper.MapPath(InstalledPluginsFilePath);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 获取描述文件
        /// </summary>
        /// <param name="pluginFolder">插件目录信息</param>
        /// <returns>Original and parsed description files</returns>
        private static IEnumerable<KeyValuePair<FileInfo, PluginDescriptor>> GetDescriptionFilesAndDescriptors(DirectoryInfo pluginFolder)
        {
            if (pluginFolder == null)
                throw new ArgumentNullException("pluginFolder");

            //create list (<file info, parsed plugin descritor>)
            var result = new List<KeyValuePair<FileInfo, PluginDescriptor>>();
            //add display order and path to list
            foreach (var descriptionFile in pluginFolder.GetFiles("Description.txt", SearchOption.AllDirectories))
            {
                if (!IsPackagePluginFolder(descriptionFile.Directory))
                    continue;

                //解析文件
                var pluginDescriptor = PluginFileParser.ParsePluginDescriptionFile(descriptionFile.FullName);

                //填充列表
                result.Add(new KeyValuePair<FileInfo, PluginDescriptor>(descriptionFile, pluginDescriptor));
            }

            //sort list by display order. NOTE: Lowest DisplayOrder will be first i.e 0 , 1, 1, 1, 5, 10
            //it's required: http://www.nopcommerce.com/boards/t/17455/load-plugins-based-on-their-displayorder-on-startup.aspx
            result.Sort((firstPair, nextPair) => firstPair.Value.DisplayOrder.CompareTo(nextPair.Value.DisplayOrder));
            return result;
        }

        /// <summary>
        /// 指示装配文件是否已加载
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <returns>Result</returns>
        private static bool IsAlreadyLoaded(FileInfo fileInfo)
        {

            try
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                if (fileNameWithoutExt == null)
                    throw new Exception(string.Format("Cannot get file extension for {0}", fileInfo.Name));
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    string assemblyName = a.FullName.Split(new[] { ',' }).FirstOrDefault();
                    if (fileNameWithoutExt.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Cannot validate whether an assembly is already loaded. " + exc);
            }
            return false;
        }

        /// <summary>
        /// 执行文件部署
        /// </summary>
        /// <param name="plug">插件文件信息</param>
        /// <returns>Assembly</returns>
        private static Assembly PerformFileDeploy(FileInfo plug)
        {
            if (plug.Directory == null || plug.Directory.Parent == null)
                throw new InvalidOperationException("The plugin directory for the " + plug.Name + " file exists in a folder outside of the allowed nopCommerce folder hierarchy");

            FileInfo shadowCopiedPlug;

            if (CommonHelper.GetTrustLevel() != AspNetHostingPermissionLevel.Unrestricted)
            {
                //所有插件都需要复制到~/Plugins/bin/
                //这是绝对必需的，因为所有这些都依赖于在web.config中静态设置探测路径(probingPaths)

                //正在med中运行，所以复制到自定义bin文件夹
                var shadowCopyPlugFolder = Directory.CreateDirectory(_shadowCopyFolder.FullName);
                shadowCopiedPlug = InitializeMediumTrust(plug, shadowCopyPlugFolder);
            }
            else
            {
                var directory = AppDomain.CurrentDomain.DynamicDirectory;
                Debug.WriteLine(plug.FullName + " to " + directory);
                //正在完全信任地运行，所以复制到标准的动态文件夹
                shadowCopiedPlug = InitializeFullTrust(plug, new DirectoryInfo(directory));
            }

            //我们现在可以注册插件定义
            var shadowCopiedAssembly = Assembly.Load(AssemblyName.GetAssemblyName(shadowCopiedPlug.FullName));

            //添加对构建管理器的引用
            Debug.WriteLine("Adding to BuildManager: '{0}'", shadowCopiedAssembly.FullName);
            BuildManager.AddReferencedAssembly(shadowCopiedAssembly);

            return shadowCopiedAssembly;
        }

        /// <summary>
        /// 用于在完全信任环境中初始化插件
        /// </summary>
        /// <param name="plug">插件信息</param>
        /// <param name="shadowCopyPlugFolder">影像复制文件夹信息</param>
        /// <returns></returns>
        private static FileInfo InitializeFullTrust(FileInfo plug, DirectoryInfo shadowCopyPlugFolder)
        {
            var shadowCopiedPlug = new FileInfo(Path.Combine(shadowCopyPlugFolder.FullName, plug.Name));
            try
            {
                File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
            }
            catch (IOException)
            {
                Debug.WriteLine(shadowCopiedPlug.FullName + " is locked, attempting to rename");
                //this occurs when the files are locked,
                //for some reason devenv locks plugin files some times and for another crazy reason you are allowed to rename them
                //which releases the lock, so that it what we are doing here, once it's renamed, we can re-shadow copy
                try
                {
                    var oldFile = shadowCopiedPlug.FullName + Guid.NewGuid().ToString("N") + ".old";
                    File.Move(shadowCopiedPlug.FullName, oldFile);
                }
                catch (IOException exc)
                {
                    throw new IOException(shadowCopiedPlug.FullName + " rename failed, cannot initialize plugin", exc);
                }
                //ok, we've made it this far, now retry the shadow copy
                File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
            }
            return shadowCopiedPlug;
        }

        /// <summary>
        /// 用于在中等信任环境中初始化插件
        /// </summary>
        /// <param name="plug">插件信息</param>
        /// <param name="shadowCopyPlugFolder">影像复制文件夹信息</param>
        /// <returns></returns>
        private static FileInfo InitializeMediumTrust(FileInfo plug, DirectoryInfo shadowCopyPlugFolder)
        {
            var shouldCopy = true;
            var shadowCopiedPlug = new FileInfo(Path.Combine(shadowCopyPlugFolder.FullName, plug.Name));

            //检查影像复制的文件是否已经存在，如果是，则检查是否更新，如果不是不复制
            if (shadowCopiedPlug.Exists)
            {
                //最好使用LastWriteTimeUTC，但并不是所有的文件系统都有这个属性
                //也许比较文件哈希更好吗？
                var areFilesIdentical = shadowCopiedPlug.CreationTimeUtc.Ticks >= plug.CreationTimeUtc.Ticks;
                if (areFilesIdentical)
                {
                    Debug.WriteLine("Not copying; files appear identical: '{0}'", shadowCopiedPlug.Name);
                    shouldCopy = false;
                }
                else
                {
                    //删除现有文件

                    //More info: http://www.nopcommerce.com/boards/t/11511/access-error-nopplugindiscountrulesbillingcountrydll.aspx?p=4#60838
                    Debug.WriteLine("New plugin found; Deleting the old file: '{0}'", shadowCopiedPlug.Name);
                    File.Delete(shadowCopiedPlug.FullName);
                }
            }

            if (shouldCopy)
            {
                try
                {
                    File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
                }
                catch (IOException)
                {
                    Debug.WriteLine(shadowCopiedPlug.FullName + " is locked, attempting to rename");
                    //当文件被锁定时，会发生这种情况，
                    //for some reason devenv locks plugin files some times and for another crazy reason you are allowed to rename them
                    //which releases the lock, so that it what we are doing here, once it's renamed, we can re-shadow copy
                    try
                    {
                        var oldFile = shadowCopiedPlug.FullName + Guid.NewGuid().ToString("N") + ".old";
                        File.Move(shadowCopiedPlug.FullName, oldFile);
                    }
                    catch (IOException exc)
                    {
                        throw new IOException(shadowCopiedPlug.FullName + " rename failed, cannot initialize plugin", exc);
                    }
                    //ok, we've made it this far, now retry the shadow copy
                    File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
                }
            }

            return shadowCopiedPlug;
        }

        /// <summary>
        /// 确定文件夹是否是包的bin插件文件夹
        /// </summary>
        /// <param name="folder">文件夹信息</param>
        /// <returns></returns>
        private static bool IsPackagePluginFolder(DirectoryInfo folder)
        {
            if (folder == null) return false;
            if (folder.Parent == null) return false;
            if (!folder.Parent.Name.Equals("Plugins", StringComparison.InvariantCultureIgnoreCase)) return false;
            return true;
        }

        /// <summary>
        /// 获取InstalledPlugins.txt文件的完整路径
        /// </summary>
        /// <returns></returns>
        private static string GetInstalledPluginsFilePath()
        { 
            return CommonHelper.MapPath(InstalledPluginsFilePath);
        }

        #endregion
    }
}
