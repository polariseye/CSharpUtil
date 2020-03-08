namespace Polaris.Utility
{
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// redis帮助类
    /// </summary>
    public class RedisUtil
    {
        /// <summary>
        /// 默认实例对象
        /// </summary>
        public static RedisUtil DefaulInstance { get; private set; } = new RedisUtil();

        /// <summary>
        /// Redis连接对象
        /// </summary>
        public ConnectionMultiplexer RedisConnection { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connstr">
        /// 常见格式1: 127.0.0.1:6379
        /// 常见格式2: 127.0.0.1:6379,connectTimeout=5000,password=123,defaultDatabase=1,responseTimeout=20000,syncTimeout=20000,asyncTimeout=20000
        /// </param>
        public void Init(string connstr)
        {
            RedisConnection = ConnectionMultiplexer.Connect(connstr);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOption">连接配置对象</param>
        public void Init(ConfigurationOptions connectionOption)
        {
            this.RedisConnection = ConnectionMultiplexer.Connect(connectionOption);
        }

        //public List<T> GetHashField<T>()
        //{
            
        //}

        //public List<T> SetHashField<T>()
        //{
        //    this.RedisConnection.GetDatabase(1).HashSet(,new HashEntry[])
        //}

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbIndex">数据库序号，不传使用默认数据库</param>
        public RedisValue Get(string key, int dbIndex = -1)
        {
            return this.RedisConnection.GetDatabase(dbIndex).StringGet(key);
        }

        /// <summary>
        /// 根据key获取缓存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dbIndex">数据库序号，不传使用默认数据库</param>
        /// <returns></returns>
        public T GetObject<T>(string key, int dbIndex = -1) where T : class
        {
            return Deserialize<T>(this.RedisConnection.GetDatabase(dbIndex).StringGet(key));
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbIndex">数据库序号，不传使用默认数据库</param>
        /// <param name="expireSeconds"></param>
        public void Set(string key, RedisValue value, int expireSeconds = 0, int dbIndex = -1)
        {
            this.RedisConnection.GetDatabase(dbIndex).StringSet(key, value, TimeSpan.FromSeconds(expireSeconds));
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbIndex">数据库序号，不传使用默认数据库</param>
        /// <param name="expireSeconds"></param>
        public void SetObject<T>(string key, T value, int expireSeconds = 0, int dbIndex = -1) where T : class
        {
            if (expireSeconds > 0)
            {
                this.RedisConnection.GetDatabase(dbIndex).StringSet(key, Serialize(value), TimeSpan.FromSeconds(expireSeconds));
            }
            else
            {
                this.RedisConnection.GetDatabase(dbIndex).StringSet(key, Serialize(value));
            }
        }

        /// <summary>
        /// 判断在缓存中是否存在该key的缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbIndex">数据库序号，不传使用默认数据库</param>
        /// <returns></returns>
        public bool Exists(string key, int dbIndex = -1)
        {
            return this.RedisConnection.GetDatabase(dbIndex).KeyExists(key); //可直接调用
        }

        /// <summary>
        /// 移除指定key的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbIndex">数据库序号，不传使用默认数据库</param>
        /// <returns></returns>
        public bool Remove(string key, int dbIndex = -1)
        {
            return this.RedisConnection.GetDatabase(dbIndex).KeyDelete(key);
        }

        /// <summary>
        /// 异步设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbIndex">数据库序号，不传使用默认数据库</param>
        public async Task SetAsync(string key, object value, int dbIndex = -1)
        {
            await this.RedisConnection.GetDatabase(dbIndex).StringSetAsync(key, Serialize(value));
        }

        /// <summary>
        /// 根据key获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbIndex">数据库序号，不传使用默认数据库</param>
        /// <returns></returns>
        public async Task<object> GetAsync(string key, int dbIndex = -1)
        {
            object value = await this.RedisConnection.GetDatabase(dbIndex).StringGetAsync(key);
            return value;
        }

        /// <summary>
        /// 实现递增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbIndex">数据库序号，不传使用默认数据库</param>
        /// <returns></returns>
        public long Increment(string key, int dbIndex = -1)
        {
            //三种命令模式
            //Sync,同步模式会直接阻塞调用者，但是显然不会阻塞其他线程。
            //Async,异步模式直接走的是Task模型。
            //Fire - and - Forget,就是发送命令，然后完全不关心最终什么时候完成命令操作。
            //即发即弃：通过配置 CommandFlags 来实现即发即弃功能，在该实例中该方法会立即返回，如果是string则返回null 如果是int则返回0.这个操作将会继续在后台运行，一个典型的用法页面计数器的实现：
            return this.RedisConnection.GetDatabase(dbIndex).StringIncrement(key, flags: CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 实现递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbIndex">数据库序号，不传使用默认数据库</param>
        /// <returns></returns>
        public long Decrement(string key, string value, int dbIndex = -1)
        {
            return this.RedisConnection.GetDatabase(dbIndex).HashDecrement(key, value, flags: CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            var jsonVal = JsonConvert.SerializeObject(o);
            return Encoding.UTF8.GetBytes(jsonVal);
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }
            var jsonStr = Encoding.UTF8.GetString(stream);

            T result = (T)JsonConvert.DeserializeObject<T>(jsonStr);
            return result;
        }

        #region  当作消息代理中间件使用 一般使用更专业的消息队列来处理这种业务场景

        /// <summary>
        /// 当作消息代理中间件使用
        /// 消息组建中,重要的概念便是生产者,消费者,消息中间件。
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public long Publish(string channel, string message)
        {
            ISubscriber sub = this.RedisConnection.GetSubscriber();
            //return sub.Publish("messages", "hello");
            return sub.Publish(channel, message);
        }

        /// <summary>
        /// 在消费者端得到该消息并输出
        /// </summary>
        /// <param name="channelFrom"></param>
        /// <param name="msgHandleAction">消息处理函数</param>
        /// <returns></returns>
        public void Subscribe(string channelFrom, Action<RedisChannel, RedisValue> msgHandleAction)
        {
            ISubscriber sub = this.RedisConnection.GetSubscriber();
            sub.Subscribe(channelFrom, msgHandleAction);
        }

        #endregion

        /// <summary>
        /// GetServer方法会接收一个EndPoint类或者一个唯一标识一台服务器的键值对
        /// 有时候需要为单个服务器指定特定的命令
        /// 使用IServer可以使用所有的shell命令，比如：
        /// DateTime lastSave = server.LastSave();
        /// ClientInfo[] clients = server.ClientList();
        /// 如果报错在连接字符串后加 ,allowAdmin=true;
        /// </summary>
        /// <returns></returns>
        public IServer GetServer(string host, int port)
        {
            IServer server = this.RedisConnection.GetServer(host, port);
            return server;
        }

        /// <summary>
        /// 获取全部终结点
        /// </summary>
        /// <returns></returns>
        public EndPoint[] GetEndPoints()
        {
            EndPoint[] endpoints = this.RedisConnection.GetEndPoints();
            return endpoints;
        }
    }
}
