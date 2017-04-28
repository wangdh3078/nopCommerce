using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Services.Payments;

namespace Nop.Web.Framework.Controllers
{
    /// <summary>
    /// 支付插件的基类控制器
    /// </summary>
    public abstract class BasePaymentController : BasePluginController
    {
        public abstract IList<string> ValidatePaymentForm(FormCollection form);
        public abstract ProcessPaymentRequest GetPaymentInfo(FormCollection form);
    }
}
