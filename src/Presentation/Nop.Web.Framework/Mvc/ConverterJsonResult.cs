using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Nop.Core;

namespace Nop.Web.Framework.Mvc
{
    /// <summary>
    /// 用Json转换器表示自定义的JsonResult
    /// </summary>
    public class ConverterJsonResult : JsonResult
    {
        #region 字段

        private readonly JsonConverter[] _converters;

        #endregion

        #region 构造函数

        public ConverterJsonResult(params JsonConverter[] converters)
        {
            _converters = converters;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="context">执行结果的上下文</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (context.HttpContext == null || context.HttpContext.Response == null)
                return;

            context.HttpContext.Response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : MimeTypes.ApplicationJson;
            if (ContentEncoding != null)
                context.HttpContext.Response.ContentEncoding = ContentEncoding;

            //serialize data with any converters
            if (Data != null)
                context.HttpContext.Response.Write(JsonConvert.SerializeObject(Data, _converters));
        }

        #endregion
    }
}
