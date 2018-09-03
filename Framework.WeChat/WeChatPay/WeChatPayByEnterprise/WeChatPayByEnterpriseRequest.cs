using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 企业付款请求
    /// </summary>
    public class WeChatPayByEnterpriseRequest:IWeChatPayRequest
    {
        /// <summary>
        ///  企业付款请求的Url
        /// </summary>
        private static string sUrl = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";

        /// <summary>
        /// 企业付款请求
        /// </summary>
        /// <param name="Model"></param>
        public WeChatPayRespone DoRequest(WeChatPayModel model)
        {
            WeChatPayByEnterpriseModel Model = model as WeChatPayByEnterpriseModel;
            WeChatPayByEnterpriseRespone Respone = new WeChatPayByEnterpriseRespone();
            var Parameters = new Dictionary<string, string>();
            Model.sNonceStr = WeChatPayHelper.GetRandomStr();//获取随机字符串
            Parameters.Add("mch_appid", Model.sAppId);
            Parameters.Add("mchid", Model.sMchId);
            Parameters.Add("nonce_str", Model.sNonceStr);
            Parameters.Add("partner_trade_no", Model.sOrderNo);
            Parameters.Add("openid", Model.sOpenId);
            Parameters.Add("check_name", "NO_CHECK");//不校验真实姓名
            Parameters.Add("amount", Model.iTotalFee.ToString());//付款金额
            Parameters.Add("desc", Model.sDescribe);
            Parameters.Add("spbill_create_ip", Model.spbill_create_ip);//该IP同在商户平台设置的IP白名单中的IP没有关联，该IP可传用户端或者服务端的IP。
            //创建签名
            Model.sSign = WeChatPayHelper.CreateSign(Parameters, Model.sPayKey);
            Parameters.Add("sign", Model.sSign);
            string sRequestXmlStr = WeChatPayHelper.InstallXml(Parameters);
            //获取微信响应结果
            string sResponeXmlStr = WeChatPayHelper.HttpPost(sUrl, sRequestXmlStr, model, true);//需要证书
            var ResponeParameter = WeChatPayHelper.GetDictionaryFromCDATAXml(sResponeXmlStr);
            Respone.sReturnParams = ResponeParameter;
            //判断返回结果
            if (ResponeParameter["return_code"] == "SUCCESS")
            {
                if (ResponeParameter["result_code"] == "SUCCESS")
                {
                    Respone.bSuccess = true;
                    Respone.sReturnMsg = ResponeParameter["return_msg"];
                }
                else
                {
                    Respone.bSuccess = false;
                    Respone.sReturnMsg = ResponeParameter["err_code_des"];
                }
            }
            else
            {
                Respone.bSuccess = false;
                Respone.sReturnMsg = ResponeParameter["return_msg"];
            }
            return Respone;
        }
    }
}
