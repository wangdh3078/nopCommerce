using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Nop.Web.Framework.Controllers
{
    /// <summary>
    /// 用于验证是否提交了某个表单名称（或值）的属性
    /// </summary>
    public class FormValueRequiredAttribute : ActionMethodSelectorAttribute
    {
        /// <summary>
        /// 提交按钮名称集合
        /// </summary>
        private readonly string[] _submitButtonNames;
        private readonly FormValueRequirement _requirement;
        private readonly bool _validateNameOnly;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="submitButtonNames">提交按钮名称集合</param>
        public FormValueRequiredAttribute(params string[] submitButtonNames):
            this(FormValueRequirement.Equal, submitButtonNames)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requirement"></param>
        /// <param name="submitButtonNames">提交按钮名称集合</param>
        public FormValueRequiredAttribute(FormValueRequirement requirement, params string[] submitButtonNames):
            this(requirement, true, submitButtonNames)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requirement"></param>
        /// <param name="validateNameOnly">仅验证名称</param>
        /// <param name="submitButtonNames">提交按钮名称集合</param>
        public FormValueRequiredAttribute(FormValueRequirement requirement, bool validateNameOnly, params string[] submitButtonNames)
        {
            //应至少找到一个提交按钮
            this._submitButtonNames = submitButtonNames;
            this._validateNameOnly = validateNameOnly;
            this._requirement = requirement;
        }
        /// <summary>
        /// 验证请求
        /// </summary>
        /// <param name="controllerContext">上下文</param>
        /// <param name="methodInfo">元数据</param>
        /// <returns></returns>
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            foreach (string buttonName in _submitButtonNames)
            {
                try
                {
                    switch (this._requirement)
                    {
                        case FormValueRequirement.Equal:
                            {
                                if (_validateNameOnly)
                                {
                                    //"name" only
                                    if (controllerContext.HttpContext.Request.Form.AllKeys.Any(x => x.Equals(buttonName, StringComparison.InvariantCultureIgnoreCase)))
                                        return true;
                                }
                                else
                                {
                                    //validate "value"
                                    //do not iterate because "Invalid request" exception can be thrown
                                    string value = controllerContext.HttpContext.Request.Form[buttonName];
                                    if (!string.IsNullOrEmpty(value))
                                        return true;
                                }
                            }
                            break;
                        case FormValueRequirement.StartsWith:
                            {
                                if (_validateNameOnly)
                                {
                                    //"name" only
                                    if (controllerContext.HttpContext.Request.Form.AllKeys.Any(x => x.StartsWith(buttonName, StringComparison.InvariantCultureIgnoreCase)))
                                        return true;
                                }
                                else
                                {
                                    //验证 "value"
                                    foreach (var formValue in controllerContext.HttpContext.Request.Form.AllKeys)
                                        if (formValue.StartsWith(buttonName, StringComparison.InvariantCultureIgnoreCase))
                                        { 
                                            var value = controllerContext.HttpContext.Request.Form[formValue];
                                            if (!string.IsNullOrEmpty(value))
                                                return true;
                                        }
                                }
                            }
                            break;
                    }
                }
                catch (Exception exc)
                {
                    //try-catch来确保没有异常抛出
                    Debug.WriteLine(exc.Message);
                }
            }
            return false;
        }
    }

    public enum FormValueRequirement
    {
        Equal,
        StartsWith
    }
}
