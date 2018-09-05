using Framework.Utility;
using Framework.WeChat;
using Framework.WeChat.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Framework.Web.Controllers
{
    /// <summary>
    /// 微信相关的控制器
    /// </summary>
    public class WeChatController :BaseController
    {
        // GET: WeChat
        private static readonly string sAppId = ConfigHelper.GetAppSetting("sAppId");
        private static readonly string sAppSecret = ConfigHelper.GetAppSetting("sAppSecret");
        private static readonly string ImageUrl = ConfigHelper.GetAppSetting("ImageUrl");

        /// <summary>
        /// 获取微信配置信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWeChatConfig()
        {
            GetJsApiAccessTokenModel model = new GetJsApiAccessTokenModel();
            model.sAppId = sAppId;
            model.sAppSecret = sAppSecret;
            IWeChatRequest request = new GetJsApiAccessTokenRequest();
            var respone = request.Execute<GetJsApiAccessTokenRespone>(model);
            string noncestr = WeChatHelper.GetRandomStr();
            long timestamp = WeChatHelper.GetTimeStamp();
            string sign = "";
            if (respone.errcode == 0)
            {
                string url = Request.UrlReferrer.AbsoluteUri;
                var paramters = new Dictionary<string, string>();
                paramters.Add("jsapi_ticket", respone.ticket);
                paramters.Add("noncestr", noncestr);
                paramters.Add("timestamp", timestamp.ToString());
                paramters.Add("url", url);
                sign = WeChatHelper.CreateSha1Sign(paramters);
                //返回的配置
                paramters.Add("appId", sAppId);
                paramters.Add("signature", sign);
                result.success = true;
                result.data = paramters;
            }
            else
            { //公众号配置信息错误
                result.info = "公众号配置信息错误";
                result.success = false;
            }
            return Content(JsonHelper.ToJsonString(result));
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadImage(string serverIds)
        {
            var ImageList =new List<object>();  //返回的图片的Url
            List<string> serverIdArray = serverIds.Split(',').ToList();
            foreach (var serverId in serverIdArray)
            {
                WeChatModel model = new WeChatModel();
                model.sAppId = sAppId;
                model.sAppSecret = sAppSecret;
                string sRequestUrl = "https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}";
                sRequestUrl = string.Format(sRequestUrl, model.sAccessToken, serverId);
                /************************************微信服务器上下载图片************************************/
                HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sRequestUrl);
                webRequest.ProtocolVersion = HttpVersion.Version10;
                webRequest.Timeout = 3000;
                webRequest.Method = WebRequestMethods.Http.Get;
                webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                Stream streamReceive = webResponse.GetResponseStream();

                //响应流转内存流
                MemoryStream memoryStream = StreamToMemoryStream(streamReceive);
                string baseStr = Convert.ToBase64String(memoryStream.ToArray());
                //获取图片后缀名
                string format = string.Empty;
                Image image = Bitmap.FromStream(memoryStream);
                var ImageFormats = GetImageFormats();
                foreach (var pair in ImageFormats)
                {
                    if (pair.Value.Guid == image.RawFormat.Guid)
                    {
                        format = pair.Key;
                        break;
                    }
                }
                //上传图片到服务器
                Parameter.method = "UploadPicture";
                JArray ImageArray = new JArray();
                ImageArray.Add(new JObject(new JProperty("picName", GuidHelper.GuidTo16String() + format),
                                           new JProperty("picCoding", baseStr.Replace("+", "%2b"))));
                string sBody = string.Format("moduleName=保修单&picCodingList={0}", ImageArray.ToString());
                var response = HttpHelper.HttpPost(Parameter, sBody);
                if (response.Code == 1)
                {
                    string PicPath =Convert.ToString(JsonHelper.Deserialize<JArray>(JsonHelper.ToJsonString(response.Data))[0]["PicPath"]);
                    string sUrl = ImageUrl + PicPath;
                    ImageList.Add(sUrl);
                }
            }
            result.success = true;
            result.data = ImageList;
            return Json(result);
        }

        /// <summary>
        /// 获取下载图片的格式
        /// </summary>
        /// <returns></returns>
        private Dictionary<String, ImageFormat> GetImageFormats()
        {
            var dic = new Dictionary<String, ImageFormat>();
            var properties = typeof(ImageFormat).GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var property in properties)
            {
                var format = property.GetValue(null, null) as ImageFormat;
                if (format == null) continue;
                dic.Add(("." + property.Name).ToLower(), format);
            }
            return dic;
        }

        /// <summary>
        /// 响应流转内存流
        /// </summary>
        /// <param name="instream"></param>
        /// <returns></returns>
        private MemoryStream StreamToMemoryStream(Stream instream)
        {
            MemoryStream outstream = new MemoryStream();
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ((count = instream.Read(buffer, 0, bufferLen)) > 0)
            {
                outstream.Write(buffer, 0, count);
            }
            return outstream;
        }
    }
}