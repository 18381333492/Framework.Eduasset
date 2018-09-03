using Framework.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ThoughtWorks.QRCode.Codec;
using TraceLogs;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信支付帮助类
    /// </summary>
    public class WeChatPayHelper
    {
        //日志组件
        private static ILogger logger = LoggerManager.Instance.GetSLogger("WeChatPay");

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="sContent"></param>
        /// <returns></returns>
        public static byte[] MakeCode(string sContent)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 7;
            if (!string.IsNullOrEmpty(sContent))
            {
                Bitmap newBitmap = qrCodeEncoder.Encode(sContent);
                MemoryStream ms = new MemoryStream();
                newBitmap.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
            return null;
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GetRandomStr()
        {
            Random Ran = new Random();
            string nonce_str = Ran.Next(11111111, 99999999).ToString() + "CNMB";
            return SecurityHelper.md5(nonce_str);
        }

        /// <summary>
        /// 获取时间撮(自1970年以来的秒数)
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 解密退款通知结果
        /// </summary>
        /// <param name="sPayKey"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static Dictionary<string, string> DecryptRefundNotifyResult(string sPayKey, Dictionary<string, string> Parameters)
        {
            var strA = Parameters["req_info"];//得到加密字符串
            string key = SecurityHelper.md5(sPayKey).ToLower();
            var strB =AESDecrypt(strA, key);//AES解密
            Parameters = Parameters.Union(GetDictionaryFromCDATAXml(strB)).ToDictionary(pi => pi.Key, pi => pi.Value);
            return Parameters;
        }

        /// <summary>
        /// 从微信请求的参数中获取字典集合
        /// </summary>
        /// <param name="sXmlContent"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionaryFromXml(string sXmlContent)
        {
            if (!string.IsNullOrEmpty(sXmlContent))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sXmlContent);
                //得到XML文档根节点
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodeList = root.ChildNodes;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (XmlNode node in nodeList)
                {
                    dic.Add(node.Name, node.InnerText);
                }
                return dic;
            }
            return null;
        }

        /// <summary>
        /// 将POST请求的返回的数据转化为字典集合
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionaryFromCDATAXml(string xmlData)
        {
            if (!string.IsNullOrEmpty(xmlData))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlData);
                //得到XML文档根节点
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodeList = root.ChildNodes;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (XmlNode node in nodeList)
                {
                    dic.Add(node.Name, node.InnerText.Replace("<![CDATA[", string.Empty).Replace("]]>", string.Empty));
                }
                return dic;
            }
            return null;
        }

        /// <summary>
        /// 拼接的[CDATA]XML数据
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static string InstallCDATAXml(Dictionary<string, string> ht)
        {
            string xml = "<xml>";
            foreach (string key in ht.Keys)
            {
                xml += "<" + key + "><![CDATA[" + ht[key] + "]]></" + key + ">";
            }
            xml += "</xml>";
            return xml;
        }

        /// <summary>
        /// 拼接统一下单支付请求的XML数据
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static string InstallXml(Dictionary<string, string> ht)
        {
            string xml = "<xml>";
            foreach (string key in ht.Keys)
            {
                xml += "<" + key + ">" + ht[key] + "</" + key + ">";
            }
            xml += "</xml>";
            return xml;
        }

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static string CreateSha1Sign(Dictionary<string, string> Parameters)
        {
            StringBuilder Sign = new StringBuilder();
            var Keys = new ArrayList(Parameters.Keys);
            Keys.Sort();//字典排序
            foreach (string key in Keys)
            {
                if (!string.IsNullOrEmpty(Parameters[key]))
                {//拼接成键值对字符串
                    Sign.Append(key + "=" + Parameters[key] + "&");
                }
            }
            string sign = SecurityHelper.SHA1(Sign.ToString().TrimEnd('&')).ToLower();
            return sign;
        }

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static string CreateSign(Dictionary<string, string> Parameters,string sPayKey)
        {
            StringBuilder Sign = new StringBuilder();
            var Keys = new ArrayList(Parameters.Keys);
            Keys.Sort();//字典排序
            foreach (string key in Keys)
            {
                if (!string.IsNullOrEmpty(Parameters[key]))
                {//拼接成键值对字符串
                    Sign.Append(key + "=" + Parameters[key] + "&");
                }
            }
            Sign.Append("key=" + sPayKey);
            string sign =SecurityHelper.md5(Sign.ToString()).ToUpper();
            return sign;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="Parameters">字典集合参数</param>
        /// <returns></returns>
        public static bool CheckSign(Dictionary<string, string> Parameters, string sPayKey)
        {
            bool bResult = false;
            if (Parameters.ContainsKey("sign"))
            {
                string old_sign = Parameters["sign"];
                Parameters.Remove("sign");
                string sign = CreateSign(Parameters, sPayKey);
                if (old_sign == sign)//验证成功
                    bResult = true;
            }
            return bResult;
        }

        /// <summary>  
        /// AES解密(无向量)  
        /// </summary>  
        /// <param name="encryptedBytes">被加密的明文</param>  
        /// <param name="key">密钥</param>  
        /// <returns>明文</returns>  
        public static string AESDecrypt(String Data, String Key)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(Data);
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            MemoryStream mStream = new MemoryStream(encryptedBytes);
            //mStream.Write( encryptedBytes, 0, encryptedBytes.Length );  
            //mStream.Seek( 0, SeekOrigin.Begin );  
            RijndaelManaged aes = new RijndaelManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;
            //aes.IV = _iV;  
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] tmp = new byte[encryptedBytes.Length + 32];
                int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + 32);
                byte[] ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                return Encoding.UTF8.GetString(ret);
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }


        /// <summary>
        /// Post请求微信系统
        /// </summary>
        /// <param name="sUrl">请求的Url</param>
        /// <param name="PostData">传输的数据</param>
        /// <param name="sCertPath">证书路径</param>
        /// <param name="IsNeedCert">是否需要证书</param>
        /// <returns></returns>
        public static string HttpPost(string sUrl, string PostData, WeChatPayModel Model, bool isUseCert = false)
        {
            byte[] bPostData = System.Text.Encoding.UTF8.GetBytes(PostData);
            string sResult = string.Empty;
            try
            {
                HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sUrl);
                webRequest.ProtocolVersion = HttpVersion.Version10;
                webRequest.Timeout = 30000;
                webRequest.Method = "POST";
                webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");

                //判断是否需要证书
                if (isUseCert)
                {//微信退款需要证书
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Model.sCertPath);
                    X509Certificate2 cert = new X509Certificate2(path, Model.sMchId);//证书的密码就是商户号
                    webRequest.ClientCertificates.Add(cert);
                }
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
                            using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.GetEncoding(webResponse.CharacterSet)))
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
                        using (var zipStream =
                            new System.IO.Compression.DeflateStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.GetEncoding(webResponse.CharacterSet)))
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
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.UTF8))
                        {
                            sResult = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("微信支付相关的请求异常:" + ex.Message);
                logger.Info(ex);
            }
            logger.Info("请求的Url:" + sUrl);
            logger.Info("返回的结果:" + sResult);
            return sResult;
        }
    }
}
