using Framework.Utility;
using Framework.WeChat;
using Framework.WeChat.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                string url = Request.Url.AbsoluteUri;
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
    }
}