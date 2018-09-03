using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信公众号支付请求
    /// </summary>
    public class WeChatPayByJsRequest: IWeChatPayRequest
    {
        /// <summary>
        /// 公众号支付请求的Url
        /// </summary>
        private static string sUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 公众号支付下单
        /// </summary>
        /// <param name="Model"></param>
        public WeChatPayRespone DoRequest(WeChatPayModel model)
        {
            WeChatPayByJsModel Model = model as WeChatPayByJsModel;
            WeChatPayByJsRespone Respone = new WeChatPayByJsRespone();
            var Parameters = new Dictionary<string, string>();
            Model.sNonceStr = WeChatPayHelper.GetRandomStr();//获取随机字符串
            Parameters.Add("appid", Model.sAppId);
            Parameters.Add("mch_id", Model.sMchId);
            Parameters.Add("nonce_str", Model.sNonceStr);
            Parameters.Add("body", Model.sBody);
            Parameters.Add("out_trade_no", Model.sOrderNo);
            Parameters.Add("total_fee", Model.iTotalFee.ToString());
            Parameters.Add("spbill_create_ip", Model.spbill_create_ip);
            Parameters.Add("notify_url", Model.sNotifyUrl);
            Parameters.Add("trade_type", Enum.GetName(typeof(WeChatPayType), Model.sTradeType));
            Parameters.Add("openid", Model.sOpenId);
            if (!string.IsNullOrEmpty(model.attach))
                Parameters.Add("attach", model.attach); //添加商户自定义数据
            //创建签名
            Model.sSign = WeChatPayHelper.CreateSign(Parameters, Model.sPayKey);
            Parameters.Add("sign", Model.sSign);
            string sRequestXmlStr = WeChatPayHelper.InstallXml(Parameters);      
            //获取微信响应结果
            string sResponeXmlStr = WeChatPayHelper.HttpPost(sUrl, sRequestXmlStr, null);
            var ResponeParameter = WeChatPayHelper.GetDictionaryFromCDATAXml(sResponeXmlStr);
            Respone.sReturnParams = ResponeParameter;
            //判断返回结果
            if (ResponeParameter["return_code"] == "SUCCESS")
            {
                if (WeChatPayHelper.CheckSign(ResponeParameter, Model.sPayKey))
                {
                    if (ResponeParameter["result_code"] == "SUCCESS")
                    {
                        Respone.bSuccess = true;
                        Respone.sReturnMsg = ResponeParameter["return_msg"];
                        Respone.sPrepayId = ResponeParameter["prepay_id"];//微信生成的预支付会话标识，用于后续接口调用中使用，该值有效期为2小时
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
                    Respone.sReturnMsg = "签名验证失败";
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
