﻿namespace Polaris.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// 读写锁工具类
    /// </summary>
    public class ReaderWriterLockUtil
    {
        /// <summary>
        /// 锁原始对象
        /// </summary>
        public ReaderWriterLockSlim LockObj { get; set; } = new ReaderWriterLockSlim();

        /// <summary>
        /// 锁类型枚举
        /// </summary>
        public enum LockTypeEnum
        {
            /// <summary>
            /// 读,在此方式下，如果要切换到写。则会报异常
            /// </summary>
            Reader,

            /// <summary>
            /// 写
            /// </summary>
            Writer,

            /// <summary>
            /// 可升级的读，在读中可能需要切换到写死，用此方式，此方式性能比读高
            /// </summary>
            EnterUpgradeableReader
        }

        /// <summary>
        /// 自定义锁对象
        /// </summary>
        private class CustomMonitor : IDisposable
        {
            /// <summary>
            /// 锁对象
            /// </summary>
            private ReaderWriterLockSlim rwLockObj = null;

            /// <summary>
            /// 锁类型
            /// </summary>
            private LockTypeEnum lockType;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="rwLockObj">读写锁对象</param>
            /// <param name="lockType">获取方式</param>
            public CustomMonitor(ReaderWriterLockSlim rwLockObj, LockTypeEnum lockType)
            {
                this.rwLockObj = rwLockObj;
                this.lockType = lockType;
            }

            /// <summary>
            /// 锁释放
            /// </summary>
            public void Dispose()
            {
                if (lockType == LockTypeEnum.Reader && rwLockObj.IsReadLockHeld)
                {
                    rwLockObj.ExitReadLock();
                }
                else if (lockType == LockTypeEnum.Writer && rwLockObj.IsWriteLockHeld)
                {
                    rwLockObj.ExitWriteLock();
                }
                else if (lockType == LockTypeEnum.EnterUpgradeableReader && rwLockObj.IsUpgradeableReadLockHeld)
                {
                    rwLockObj.ExitUpgradeableReadLock();
                }
            }
        }

        /// <summary>
        /// lockObjData同步对象
        /// </summary>
        private Object mLockObj = new Object();

        /// <summary>
        /// 获取锁对象，获取过程会死等。直到获取到锁对象
        /// </summary>
        /// <param name="key">锁的唯一标识</param>
        /// <param name="lockType">锁类型</param>
        /// <returns>返回锁对象</returns>
        private IDisposable GetLockSlimByInfiniteWait(LockTypeEnum lockType)
        {
            // 获取锁对象
            ReaderWriterLockSlim lockObj = this.LockObj;

            // 进入锁
            switch (lockType)
            {
                case LockTypeEnum.Reader:
                    {
                        if (lockObj.IsReadLockHeld == false)
                        {
                            lockObj.EnterReadLock();
                        }

                        return new CustomMonitor(lockObj, lockType);
                    }
                case LockTypeEnum.Writer:
                    {
                        if (lockObj.IsWriteLockHeld == false)
                        {
                            lockObj.EnterWriteLock();
                        }

                        return new CustomMonitor(lockObj, lockType);
                    }
                case LockTypeEnum.EnterUpgradeableReader:
                    {
                        if (lockObj.IsUpgradeableReadLockHeld == false)
                        {
                            lockObj.EnterUpgradeableReadLock();
                        }

                        return new CustomMonitor(lockObj, lockType);
                    }
            }

            return null;
        }

        /// <summary>
        /// 获取锁对象
        /// </summary>
        /// <param name="lockType">锁类型</param>
        /// <param name="millisecondsTimeout">等待的毫秒数；&lt;=0表示无限期等待。</param>
        /// <returns>返回锁对象</returns>
        /// <exception cref="TimeoutException">获取锁对象超时时，抛出此异常</exception>
        /// <remarks>
        /// 对同一个锁操作时，读的代码块内不能包含写的代码块，也不能在写的代码块包含读的代码块，这会导致内部异常,如果确实想要写，请使用<see cref="LockTypeEnum.EnterUpgradeableReader"/>，比如：
        /// <code>
        /// using("test", LockTypeEnum.EnterUpgradeableReader)
        /// {
        ///     using("test",LockTypeEnum.Write)
        ///     {
        ///         // do something
        ///     }
        /// }
        /// </code>
        /// </remarks>
        public IDisposable GetLock(LockTypeEnum lockType, Int32 millisecondsTimeout = 100)
        {
            // 判断是否为无限期等待
            if (millisecondsTimeout <= 0)
            {
                return GetLockSlimByInfiniteWait(lockType);
            }

            // 获取锁对象
            ReaderWriterLockSlim lockSlimObj = this.LockObj;

            // 进入锁
            switch (lockType)
            {
                case LockTypeEnum.Reader:
                    {
                        if (lockSlimObj.IsUpgradeableReadLockHeld || lockSlimObj.IsReadLockHeld || lockSlimObj.TryEnterReadLock(millisecondsTimeout))
                        {
                            return new CustomMonitor(lockSlimObj, lockType);
                        }

                        throw new TimeoutException("等待读锁超时");
                    }
                case LockTypeEnum.Writer:
                    {
                        if (lockSlimObj.IsWriteLockHeld || lockSlimObj.TryEnterWriteLock(millisecondsTimeout))
                        {
                            return new CustomMonitor(lockSlimObj, lockType);
                        }

                        throw new TimeoutException("等待写锁超时");
                    }
                case LockTypeEnum.EnterUpgradeableReader:
                    {
                        if (lockSlimObj.IsUpgradeableReadLockHeld || lockSlimObj.TryEnterUpgradeableReadLock(millisecondsTimeout))
                        {
                            return new CustomMonitor(lockSlimObj, lockType);
                        }

                        throw new TimeoutException("等待读锁超时");
                    }
            }

            return null;
        }
    }
}
