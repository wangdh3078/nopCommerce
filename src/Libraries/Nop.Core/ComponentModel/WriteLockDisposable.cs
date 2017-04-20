using System;
using System.Threading;

namespace Nop.Core.ComponentModel
{
    /// <summary>
    ///为实现锁定的资源访问提供了方便的方法。
    /// </summary>
    /// <remarks>
    /// 作为基础设施类。
    /// </remarks>
    public class WriteLockDisposable : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;

        /// <summary>
        /// 初始化<see cref ="WriteLockDisposable"/>类的新实例。
        /// </summary>
        /// <param name="rwLock">读写锁</param>
        public WriteLockDisposable(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
            _rwLock.EnterWriteLock();
        }

        void IDisposable.Dispose()
        {
            _rwLock.ExitWriteLock();
        }
    }
}
