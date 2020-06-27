

namespace Polaris.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    public static class HttpUtil
    {
        public static HttpClient Client { get; private set; }

        static HttpUtil()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.UseProxy = false; // 有些电脑IE浏览器是设置了代理的，浏览器本身能访问，但HttpClient却不能访问，所以此处添加了这个设置
            Client = new HttpClient(clientHandler);
        }

        public static async Task<byte[]> PostJson(String urlVal, Dictionary<String, Object> header, Object paramData = null, Dictionary<String, Object> urlParam = null)
        {
            var bytesData = new byte[0];
            var requestContent = "";
            if (paramData != null)
            {
                requestContent = JsonUtil.Serialize(paramData);
                bytesData = Encoding.UTF8.GetBytes(requestContent);
            }

            var urlParamList = new List<String>();
            if (urlParam != null)
            {
                foreach (var item in urlParam)
                {
                    urlParamList.Add($"{item.Key}={HttpUtility.UrlEncode(item.Value.ToString())}");
                }
            }

            //获取url
            if(urlParamList.Count>0)
            {
                urlVal= $"{urlVal}?{String.Join("&", urlParamList)}";
            }

            var content = new ByteArrayContent(bytesData);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var requestMessage = GetRequestMessage(urlVal, HttpMethod.Post, header, content);

            HttpResponseMessage responseObj;

            responseObj = await Client.SendAsync(requestMessage);

            if (responseObj.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"HttpCode Is No Ok:{responseObj.StatusCode}");
            }

            var responseBytes = await responseObj.Content.ReadAsByteArrayAsync();

            return responseBytes;
        }

        public static async Task<byte[]> PostForm(String urlVal, Dictionary<String, Object> header, Dictionary<String, String> formParamData = null, Dictionary<String, Object> urlParam = null)
        {
            if (formParamData == null)
            {
                formParamData = new Dictionary<string, string>();
            }

            var urlParamList = new List<String>();
            if (urlParam != null)
            {
                foreach (var item in urlParam)
                {
                    urlParamList.Add($"{item.Key}={HttpUtility.UrlEncode(item.Value.ToString())}");
                }
            }

            if (header == null)
            {
                header = new Dictionary<string, object>();
            }

            //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");

            //获取url            
            if (urlParamList.Count > 0)
            {
                urlVal = $"{urlVal}?{String.Join("&", urlParamList)}";
            }


            HttpResponseMessage responseObj = null;

            var requestMessage = GetRequestMessage(urlVal, HttpMethod.Post, header, new FormUrlEncodedContent(formParamData));
            responseObj = await Client.SendAsync(requestMessage);

            if (responseObj.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"HttpCode Is No Ok:{responseObj.StatusCode}");
            }

            var responseBytes = await responseObj.Content.ReadAsByteArrayAsync();

            return responseBytes;

        }

        public static async Task<byte[]> Post(String urlVal, Dictionary<String, Object> header, byte[] bodyData = null, Dictionary<String, Object> urlParam = null)
        {
            var urlParamList = new List<String>();
            if (urlParam != null)
            {
                foreach (var item in urlParam)
                {
                    urlParamList.Add($"{item.Key}={HttpUtility.UrlEncode(item.Value.ToString())}");
                }
            }

            if (header == null)
            {
                header = new Dictionary<string, object>();
            }

            //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");

            //获取url            
            if (urlParamList.Count > 0)
            {
                urlVal = $"{urlVal}?{String.Join("&", urlParamList)}";
            }


            HttpResponseMessage responseObj = null;

            var requestMessage = GetRequestMessage(urlVal, HttpMethod.Post, header, new ByteArrayContent(bodyData));
            responseObj = await Client.SendAsync(requestMessage);

            if (responseObj.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"HttpCode Is No Ok:{responseObj.StatusCode}");
            }

            var responseBytes = await responseObj.Content.ReadAsByteArrayAsync();

            return responseBytes;

        }
        public static async Task<byte[]> Post(String urlVal, Dictionary<String, Object> header, String bodyData = null, Dictionary<String, Object> urlParam = null)
        {
            var urlParamList = new List<String>();
            if (urlParam != null)
            {
                foreach (var item in urlParam)
                {
                    urlParamList.Add($"{item.Key}={HttpUtility.UrlEncode(item.Value.ToString())}");
                }
            }

            if (header == null)
            {
                header = new Dictionary<string, object>();
            }

            //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");

            //获取url            
            if (urlParamList.Count > 0)
            {
                urlVal = $"{urlVal}?{String.Join("&", urlParamList)}";
            }


            HttpResponseMessage responseObj = null;

            var requestMessage = GetRequestMessage(urlVal, HttpMethod.Post, header, new StringContent(bodyData));
            responseObj = await Client.SendAsync(requestMessage);

            if (responseObj.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"HttpCode Is No Ok:{responseObj.StatusCode}");
            }

            var responseBytes = await responseObj.Content.ReadAsByteArrayAsync();

            return responseBytes;

        }

        public static async Task<byte[]> Get(String urlVal, Dictionary<String, Object> header, Dictionary<String, Object> paramData = null)
        {
            //获取请求参数
            var paramList = new List<String>();
            if (paramData != null)
            {
                foreach (var item in paramData)
                {
                    paramList.Add($"{item.Key}={HttpUtility.UrlEncode(item.Value.ToString())}");
                }
            }

            if (paramList.Count > 0)
            {
                urlVal = $"{urlVal}?{String.Join("&", paramList)}";
            }

            HttpResponseMessage responseObj = null;

            var requestMessage = GetRequestMessage(urlVal, HttpMethod.Get, header);
            responseObj = await Client.SendAsync(requestMessage);


            if (responseObj.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"HttpCode Is No Ok:{responseObj.StatusCode}");
            }

            var responseBytes = await responseObj.Content.ReadAsByteArrayAsync();

            return responseBytes;
        }

        private static HttpRequestMessage GetRequestMessage(String requestUrl, HttpMethod httpMethod, Dictionary<String, Object> header, HttpContent content = null)
        {
            HttpRequestMessage message = new HttpRequestMessage();
            message.Method = httpMethod;
            message.RequestUri = new Uri(requestUrl);

            // 设置请求头信息
            //httpClient.DefaultRequestHeaders.Add("UserAgent",
            //    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11");

            if (header != null)
            {
                foreach (var headerItem in header)
                {
                    message.Headers.Add(headerItem.Key, headerItem.Value.ToString());
                }
            }

            if (content != null)
            {
                message.Content = content;
            }

            return message;
        }
    }
}
