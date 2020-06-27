namespace Polaris.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Html处理的工具类
    /// </summary>
    public class HtmlUtil
    {
        /// <summary>
        /// 去除Html内容中的Html标签
        /// </summary>
        /// <param name="html">Html内容</param>
        /// <param name="length">需要去除的标签</param>
        /// <returns></returns>
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = Regex.Replace(html, "<[^>]+>", "");
            strText = Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
    }
}
