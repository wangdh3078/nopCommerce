using System.Configuration;
using System.Runtime.CompilerServices;
using Nop.Core.Configuration;

namespace Nop.Core.Infrastructure
{
    /// <summary>
    /// 提供访问Nop引擎的单例实例。
    /// </summary>
    public class EngineContext
    {
        #region Methods

        /// <summary>
        /// 初始化Nop工厂的静态实例。
        /// </summary>
        /// <param name="forceRecreate">即使工厂以前已初始化，也创建一个新的工厂实例。</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                Singleton<IEngine>.Instance = new NopEngine();

                var config = ConfigurationManager.GetSection("NopConfig") as NopConfig;
                Singleton<IEngine>.Instance.Initialize(config);
            }
            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        /// 将静态引擎实例设置为提供的引擎。 使用此方法提供您自己的引擎实现。
        /// </summary>
        /// <param name="engine">使用的引擎</param>
        /// <remarks>只有当你知道你在做什么时才使用这种方法。</remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取用于访问Nop服务的单例Nop引擎。
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<IEngine>.Instance;
            }
        }

        #endregion
    }
}
