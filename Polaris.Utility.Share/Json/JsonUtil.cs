
namespace Polaris.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Json序列化操作类
    /// </summary>
    public class JsonUtil
    {
        /// <summary>
        /// 序列化处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String Serialize(Object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(String data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
