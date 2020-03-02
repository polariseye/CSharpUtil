namespace Polaris.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using static Polaris.Utility.ReaderWriterLockUtil;

    /// <summary>
    /// 读写锁工具类
    /// </summary>
    public class ReadWriteLockCollectionUtil
    {
        /// <summary>
        /// lockObjData同步对象
        /// </summary>
        private Object mLockObj = new Object();

        /// <summary>
        /// 锁集合
        /// </summary>
        private Dictionary<String, ReaderWriterLockUtil> mLockSlimDic = new Dictionary<String, ReaderWriterLockUtil>();

        /// <summary>
        /// 获取锁对象信息
        /// </summary>
        /// <param name="key">锁的唯一标识</param>
        /// <returns>返回锁对象</returns>
        private ReaderWriterLockUtil GetLockSlim(String key)
        {
            lock (mLockObj)
            {
                if (!mLockSlimDic.ContainsKey(key))
                {
                    // 如果获取不到锁，则创建一个锁对象
                    ReaderWriterLockUtil lockObj = new ReaderWriterLockUtil();
                    mLockSlimDic[key] = lockObj;

                    return lockObj;
                }

                return mLockSlimDic[key];
            }
        }

        /// <summary>
        /// 获取锁对象，获取过程会死等。直到获取到锁对象
        /// </summary>
        /// <param name="key">锁的唯一标识</param>
        /// <param name="lockType">锁类型</param>
        /// <returns>返回锁对象</returns>
        private IDisposable GetLockSlimByInfiniteWait(String key, LockTypeEnum lockType)
        {
            // 获取锁对象
            var lockObj = GetLockSlim(key);

            return lockObj.GetLock(lockType);
        }

        /// <summary>
        /// 获取锁对象
        /// </summary>
        /// <param name="key">锁的唯一标识</param>
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
        public IDisposable GetLock(String key, LockTypeEnum lockType, Int32 millisecondsTimeout = 100)
        {
            // 获取锁对象
            var lockSlimObj = GetLockSlim(key);

            return lockSlimObj.GetLock(lockType, millisecondsTimeout);
        }

        /// <summary>
        /// 主动释放锁资源，避免长久驻留内存
        /// </summary>
        /// <param name="key">锁的唯一标识</param>
        public void DisposeLock(String key)
        {
            lock (mLockObj)
            {
                if (mLockSlimDic.ContainsKey(key) == false)
                {
                    return;
                }

                // 先获取锁，避免其他地方在进行锁操作
                var lockItem = mLockSlimDic[key];
                try
                {
                    lockItem.LockObj.EnterWriteLock();

                    mLockSlimDic.Remove(key);

                }
                finally
                {
                    lockItem.LockObj.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// 主动清空所有锁资源，避免长久驻留内存
        /// </summary>
        public void DisposeAllLock()
        {
            lock (mLockObj)
            {
                foreach (var lockKey in this.mLockSlimDic.Keys.ToList())
                {
                    // 先获取锁，避免其他地方在进行锁操作
                    var lockItem = mLockSlimDic[lockKey];
                    try
                    {
                        lockItem.LockObj.EnterWriteLock();

                        mLockSlimDic.Remove(lockKey);

                    }
                    finally
                    {
                        lockItem.LockObj.ExitWriteLock();
                    }
                }
            }
        }
    }
}
