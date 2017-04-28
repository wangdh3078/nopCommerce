using System.Collections.Generic;

namespace Nop.Web.Framework.Localization
{
    /// <summary>
    /// 本地化模式
    /// </summary>
    public interface ILocalizedModel
    {

    }
    /// <summary>
    /// 本地化模式(泛型)
    /// </summary>
    /// <typeparam name="TLocalizedModel">类型</typeparam>
    public interface ILocalizedModel<TLocalizedModel> : ILocalizedModel
    {
        /// <summary>
        /// 本地化设置集合
        /// </summary>
        IList<TLocalizedModel> Locales { get; set; }
    }
}
