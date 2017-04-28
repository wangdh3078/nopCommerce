using System.Collections.Generic;
using System.Web.Mvc;

namespace Nop.Web.Framework.Mvc
{
    /// <summary>
    /// Base nopCommerce model
    /// </summary>
    [ModelBinder(typeof(NopModelBinder))]
    public partial class BaseNopModel
    {
        public BaseNopModel()
        {
            this.CustomProperties = new Dictionary<string, object>();
            PostInitialize();
        }

        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }

        /// <summary>
        /// 开发人员可以在自定义部分类中覆盖此方法
        /// 以便为构造函数添加一些自定义的初始化代码
        /// </summary>
        protected virtual void PostInitialize()
        {
            
        }

        /// <summary>
        /// 使用此属性可存储模型的任何自定义值。
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }
    }

    /// <summary>
    ///基本nopCommerce实体模型
    /// </summary>
    public partial class BaseNopEntityModel : BaseNopModel
    {
        public virtual int Id { get; set; }
    }
}
