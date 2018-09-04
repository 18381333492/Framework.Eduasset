using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Utility;

namespace Framework.WeChat
{
    /// <summary>
    /// 微信所有请求的需要的参数Model
    /// </summary>
    public class WeChatModel 
    {

        /// <summary>
        /// 缓存的sAccessToken列表
        /// </summary>
        private static List<dynamic> AccessTokenList=new List<dynamic>();

        /// <summary>
        /// 微信公众号的AppId
        /// </summary>
        public string sAppId { get; set; }

        /// <summary>
        /// 微信公众号的AppSecret
        /// </summary>
        public string sAppSecret { get; set; }
  

        /// <summary>
        /// 调用微信接口的凭证
        /// </summary>
        public string sAccessToken
        {
            get { return GetAccessToken(this); }
        }


        /// <summary>
        /// 获取微信调用凭证
        /// </summary>
        private static string GetAccessToken(WeChatModel Model)
        {
            var model = AccessTokenList.FirstOrDefault(m =>m.sAppId == Model.sAppId);
            if (model == null)
            {
                IWeChatRequest request = new GetAccessTokenRequest();
                var respone = request.Execute<GetAccessTokenRespone>(Model);
                if (respone.errcode == 0)
                {
                    //添加凭证
                    AccessTokenList.Add(new
                    {
                        sAppId = Model.sAppId,
                        sAppSecret = Model.sAppSecret,
                        dDate = DateTime.Now.AddSeconds(respone.expires_in - 200),
                        sAccessToken = respone.access_token
                    });
                }
                return respone.access_token;
            }
            else
            {
                if (Convert.ToDateTime(model.dDate) > DateTime.Now)
                    return model.sAccessToken;
                else
                {
                    //凭证过期 更新凭证
                    IWeChatRequest request = new GetAccessTokenRequest();
                    var respone = request.Execute<GetAccessTokenRespone>(Model);
                    if (respone.errcode == 0)
                    {
                        //删除之前凭证
                        AccessTokenList.Remove(model);
                        //添加凭证
                        AccessTokenList.Add(new
                        {
                            sAppId = Model.sAppId,
                            sAppSecret = Model.sAppSecret,
                            dDate = DateTime.Now.AddSeconds(respone.expires_in - 200),
                            sAccessToken = respone.access_token
                        });
                    }
                    return respone.access_token;
                }
            }
        }
    }
}
