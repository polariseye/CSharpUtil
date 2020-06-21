namespace Polaris.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// base64工具类
    /// </summary>
    public class Base64Utlil
    {
        /// <summary>
        /// 将字符串转换成base64格式,使用UTF8字符集
        /// </summary>
        /// <param name="content">加密内容</param>
        /// <returns></returns>
        public static string Base64EncodeString(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 将base64格式，转换utf8
        /// </summary>
        /// <param name="content">解密内容</param>
        /// <returns></returns>
        public static string Base64DecodeString(string content)
        {
            byte[] bytes = Convert.FromBase64String(content);
            return Encoding.UTF8.GetString(bytes);
        }


        /// <summary>
        /// 将字符串转换成base64格式,使用UTF8字符集
        /// </summary>
        /// <param name="content">加密内容</param>
        /// <returns></returns>
        public static string Base64EncodeBytes(byte[] content)
        {
            if (content == null || content.Length <= 0)
            {
                return String.Empty;
            }

            return Convert.ToBase64String(content);
        }

        /// <summary>
        /// 将base64格式，转换utf8
        /// </summary>
        /// <param name="content">解密内容</param>
        /// <returns></returns>
        public static byte[] Base64DecodeBytes(string content)
        {
            if (content == null || content.Length <= 0)
            {
                return null;
            }

            return Convert.FromBase64String(content);
        }
    }
}
