using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TraceLogs;

namespace Framework.Utility.Tool
{
    /// <summary>
    /// http请求的封装
    /// </summary>
    public class HttpHelper
    {

        private static ILogger logger = LoggerManager.Instance.GetSLogger("Http");

        /// <summary>
        /// 接口地址
        /// </summary>
        private static readonly string sDomain = ConfigHelper.GetAppSetting("InterfaceAddress");

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="sUrl">请求的url</param>
        /// <returns></returns>
        public static string HttpGet(HttpParameter Parameter)
        {
            string sResult = string.Empty;
            string sUrl = sDomain + Parameter.Serialize();
            try
            {
                HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sUrl);
                webRequest.ProtocolVersion = HttpVersion.Version10;
                webRequest.Timeout = 30000;
                webRequest.Method = WebRequestMethods.Http.Get;
                webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");

                HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                if (webResponse.ContentEncoding.ToLower() == "gzip")//如果使用了GZip则先解压
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (var zipStream = new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (StreamReader sr = new System.IO.StreamReader(zipStream))
                            {
                                sResult = sr.ReadToEnd();
                            }
                        }
                    }
                }
                else if (webResponse.ContentEncoding.ToLower() == "deflate")//如果使用了deflate则先解压
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (var deflateStream = new System.IO.Compression.DeflateStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (StreamReader sr = new System.IO.StreamReader(deflateStream))
                            {
                                sResult = sr.ReadToEnd();
                            }
                        }
                    }
                }
                else
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive))
                        {
                            sResult = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
                logger.Fatal(ex);
            }
            logger.Info("请求的Url:" + sUrl);
            logger.Info("返回的结果:" + sResult);
            return sResult;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="sUrl">请求的链接</param>
        /// <param name="PostData">请求的参数</param>
        /// <returns></returns>
        public static string HttpPost(HttpParameter Parameter, string PostData)
        {
            string sResult = string.Empty;
            string sUrl = sDomain + Parameter.Serialize();
            try
            {
                HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sUrl);
                webRequest.ProtocolVersion = HttpVersion.Version10;
                webRequest.Timeout = 30000;
                webRequest.Method = "POST";
                webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                byte[] bPostData = System.Text.Encoding.UTF8.GetBytes(PostData);
                if (bPostData != null)
                {
                    Stream postDataStream = webRequest.GetRequestStream();
                    postDataStream.Write(bPostData, 0, bPostData.Length);
                }
                HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                if (webResponse.ContentEncoding.ToLower() == "gzip")//如果使用了GZip则先解压
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (var zipStream =
                            new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (StreamReader sr = new System.IO.StreamReader(zipStream))
                            {
                                sResult = sr.ReadToEnd();
                            }
                        }
                    }
                }
                else if (webResponse.ContentEncoding.ToLower() == "deflate")//如果使用了deflate则先解压
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (var deflateStream = new System.IO.Compression.DeflateStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (StreamReader sr = new System.IO.StreamReader(deflateStream))
                            {
                                sResult = sr.ReadToEnd();
                            }
                        }
                    }
                }
                else
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive))
                        {
                            sResult = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
                logger.Fatal(ex);
            }
            logger.Info("请求的Url:" + sUrl);
            logger.Info("返回的结果:" + sResult);
            return sResult;
        }
    }

    public class HttpParameter
    {
        /// <summary>
        /// 请求的方法名称
        /// </summary>
        public string method
        {
            get; set;
        }

        public Dictionary<string, object> ArgsArray
        {
            get; set;
        }

        /// <summary>
        /// 序列化参数
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            StringBuilder sArgsStr = new StringBuilder();
            sArgsStr.AppendFormat("?method={0}", this.method);
            foreach (var key in ArgsArray.Keys)
            {
                sArgsStr.AppendFormat("&{0}={1}", key, ArgsArray[key]);
            }
            return sArgsStr.ToString();
        }

    }
}
