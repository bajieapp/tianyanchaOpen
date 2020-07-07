using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace tianyanchaOpen.AppConsole
{
    /// <summary>
    /// HttpClient帮助类
    /// </summary>
    public class HttpClientHelper
    {
        /// <summary>
        /// HttpClient
        /// </summary>
        private static readonly HttpClient _HttpClient;
        /// <summary>
        /// 主机
        /// </summary>
        public static string Host { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public static string Token { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        static HttpClientHelper()
        {
            var httpClientHandler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,  //压缩
            };
            _HttpClient = new HttpClient(httpClientHandler);
        }

        /// <summary>
        /// Get
        /// </summary>
        public static T Get<T>(string url) where T : class
        {
            return Get<T>(url, new Dictionary<string, string>());
        }

        /// <summary>
        /// Get
        /// </summary>
        public static T Get<T>(string url, Dictionary<string, string> datas) where T : class
        {
            //Get数据
            string parameter = ParameterHandle(datas);
            if(!string.IsNullOrEmpty(parameter))
            {
                parameter = "?" + parameter;
            }
            //头文件
            if (!string.IsNullOrEmpty(Token))
            {
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);  //Token
            }
            //Get
            string address = $"{Host}{url}{parameter}";
            var getResult = _HttpClient.GetAsync(address).Result;
            //Get失败
            if (getResult.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"通讯网址：{address}\r\n通讯错误：{getResult.StatusCode.ToString()}({(int)getResult.StatusCode})");
            }
            //Get成功
            var content = getResult.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(content);
        }


        /// <summary>
        /// Get
        /// </summary>
        public static string Get(string url)
        {
            //头文件
            if (!string.IsNullOrEmpty(Token))
            {
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);  //Token
            }
            //Get
            string address = $"{Host}{url}";
            var getResult = _HttpClient.GetAsync(address).Result;
            //Get失败
            if (getResult.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"通讯网址：{address}\r\n通讯错误：{getResult.StatusCode.ToString()}({(int)getResult.StatusCode})");
            }
            //Get成功
            var content = getResult.Content.ReadAsStringAsync().Result;
            return content;
        }

        /// <summary>
        /// Get
        /// </summary>
        public static Stream Get(string url, Dictionary<string, string> datas)
        {
            //Get数据
            string parameter = ParameterHandle(datas);
            //头文件
            if (!string.IsNullOrEmpty(Token))
            {
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);  //Token
            }
            //Get
            string address = $"{Host}{url}?{parameter}";
            var getResult = _HttpClient.GetAsync(address).Result;
            //Get失败
            if (getResult.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"通讯网址：{address}\r\n通讯错误：{getResult.StatusCode.ToString()}({(int)getResult.StatusCode})");
            }
            //Get成功
            var content = getResult.Content.ReadAsStreamAsync().Result;
            return content;
        }

        /// <summary>
        /// Post
        /// </summary>
        public static T Post<T>(string url, Dictionary<string, string> datas, List<string> files) where T : class
        {
            //数据
            var multipartFormDataContent = new MultipartFormDataContent();
            foreach (var data in datas)  //文本数据
            {
                multipartFormDataContent.Add(new StringContent(data.Value), data.Key);
            }
            foreach (var file in files)   //文件数据
            {
                if (File.Exists(file))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    multipartFormDataContent.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(file)), "file", fileInfo.Name);
                }
            }
            //头文件
            if (!string.IsNullOrEmpty(Token))
            {
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);  //Token
            }
            //Post
            string address = $"{Host}{url}";
            var postResult = _HttpClient.PostAsync(address, multipartFormDataContent).Result;
            //Post失败
            if (postResult.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"通讯网址：{address}\r\n通讯错误：{postResult.StatusCode.ToString()}({(int)postResult.StatusCode})");
            }
            //Post成功
            var content = postResult.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(content);
        }

        /// <summary>
        /// Post
        /// </summary>
        public static T Post<T>(string url, object json, List<string> files) where T : class
        {
            //数据
            var multipartFormDataContent = new MultipartFormDataContent();

            multipartFormDataContent.Add(new StringContent(JsonSerializer.Serialize(json)));

            foreach (var file in files)   //文件数据
            {
                if (File.Exists(file))
                {
                    multipartFormDataContent.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(file)));
                }
            }
            //头文件
            if (!string.IsNullOrEmpty(Token))
            {
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);  //Token
            }
            //Post
            string address = $"{Host}{url}";
            var postResult = _HttpClient.PostAsync(address, multipartFormDataContent).Result;
            //Post失败
            if (postResult.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"通讯网址：{address}\r\n通讯错误：{postResult.StatusCode.ToString()}({(int)postResult.StatusCode})");
            }
            //Post成功
            var content = postResult.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(content);
        }

        /// <summary>
        /// Post
        /// </summary>
        public static T Post<T>(string url, object json) where T : class
        {
            //数据
            var stringContent = new StringContent(JsonSerializer.Serialize(json));
            //头文件
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); //数据格式
            if (!string.IsNullOrEmpty(Token))
            {
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);  //Token
            }
            //Post
            string address = $"{Host}{url}";
            var postResult = _HttpClient.PostAsync(address, stringContent).Result;
            //Post失败
            if (postResult.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"通讯网址：{address}\r\n通讯错误：{postResult.StatusCode.ToString()}({(int)postResult.StatusCode})");
            }
            //Post成功
            var data = postResult.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(data);
        }

        /// <summary>
        /// 参数处理
        /// </summary>
        private static string ParameterHandle(Dictionary<string, string> datas)
        {
            string result = "";
            foreach (var data in datas)
            {
                result += $"{data.Key}={data.Value}&";
            }
            return result == "" ? result : result.Remove(result.Length - 1);
        }
    }
}