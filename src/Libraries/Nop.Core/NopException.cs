using System;
using System.Runtime.Serialization;

namespace Nop.Core
{
    /// <summary>
    ///表示应用程序执行期间发生的错误
    /// </summary>
    [Serializable]
    public class NopException : Exception
    {
        /// <summary>
        /// 初始化Exception类的新实例。
        /// </summary>
        public NopException()
        {
        }

        /// <summary>
        /// 使用指定的错误消息初始化Exception类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        public NopException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///使用指定的错误消息初始化Exception类的新实例。
        /// </summary>
		/// <param name="messageFormat">异常消息格式。</param>
		/// <param name="args">异常消息参数。</param>
        public NopException(string messageFormat, params object[] args)
			: base(string.Format(messageFormat, args))
		{
		}

        /// <summary>
        ///使用序列化数据初始化Exception类的新实例。
        /// </summary>
        /// <param name="info">SerializationInfo保存有关正在抛出的异常的序列化对象数据。</param>
        /// <param name="context">StreamingContext包含有关源或目的地的上下文信息。</param>
        protected NopException(SerializationInfo
            info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// 使用指定的错误消息和对引发此异常的内部异常的引用初始化Exception类的新实例。
        /// </summary>
        /// <param name="message">该错误消息说明了异常的原因。</param>
        /// <param name="innerException">异常是当前异常的原因，如果没有指定内部异常，则为null引用。</param>
        public NopException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
