using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace Nop.Web.Framework.Mvc
{
    public class NopModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            if (model is BaseNopModel)
            {
                ((BaseNopModel)model).BindModel(controllerContext, bindingContext);
            }
            return model;
        }

        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, object value)
        {
            //检查数据类型的值是否为System.String
            if (propertyDescriptor.PropertyType == typeof(string))
            {
                //开发人员可以使用[NoTrim]属性将属性标记为不被修剪
                if (propertyDescriptor.Attributes.Cast<object>().All(a => a.GetType() != typeof (NoTrimAttribute)))
                {
                        var stringValue = (string)value;
                        value = string.IsNullOrEmpty(stringValue) ? stringValue : stringValue.Trim();
                }
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}