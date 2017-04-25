using System;

namespace Nop.Core.Data
{
    /// <summary>
    /// 数据设置帮助类
    /// </summary>
    public partial class DataSettingsHelper
    {
        private static bool? _databaseIsInstalled;

        /// <summary>
        /// 返回一个值，表示数据库是否已经安装
        /// </summary>
        /// <returns></returns>
        public static bool DatabaseIsInstalled()
        {
            if (!_databaseIsInstalled.HasValue)
            {
                var manager = new DataSettingsManager();
                var settings = manager.LoadSettings();
                _databaseIsInstalled = settings != null && !string.IsNullOrEmpty(settings.DataConnectionString);
            }
            return _databaseIsInstalled.Value;
        }

        //重置信息缓存在“DatabaseIsInstalled”方法中
        public static void ResetCache()
        {
            _databaseIsInstalled = null;
        }
    }
}
