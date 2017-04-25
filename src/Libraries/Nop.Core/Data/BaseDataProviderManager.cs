using System;

namespace Nop.Core.Data
{
    /// <summary>
    /// 基础数据驱动管理
    /// </summary>
    public abstract class BaseDataProviderManager
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settings">数据配置</param>
        protected BaseDataProviderManager(DataSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            this.Settings = settings;
        }

        /// <summary>
        /// 获取或设置配置
        /// </summary>
        protected DataSettings Settings { get; private set; }

        /// <summary>
        /// 加载数据驱动
        /// </summary>
        /// <returns>数据驱动</returns>
        public abstract IDataProvider LoadDataProvider();
    }
}
