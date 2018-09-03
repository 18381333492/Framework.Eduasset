using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信申请退款请求
    /// </summary>
    public class WeChatPayRefundRequest: IWeChatPayRequest
    {
        /// <summary>
        /// 申请退款的Url
        /// </summary>
        private static string sUrl = "https://api.mch.weixin.qq.com/secapi/pay/refund";

        /// <summary>
        /// 执行微信申请退款
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WeChatPayRespone DoRequest(WeChatPayModel model)
        {
            WeChatPayRefundModel Model = model as WeChatPayRefundModel;
            WeChatPayRefundRespone Respone = new WeChatPayRefundRespone();
            var Parameters = new Dictionary<string, string>();
            Model.sNonceStr = WeChatPayHelper.GetRandomStr();//获取随机字符串
            Parameters.Add("appid", Model.sAppId);
            Parameters.Add("mch_id", Model.sMchId);
            Parameters.Add("nonce_str", Model.sNonceStr);
            Parameters.Add("out_trade_no", Model.sOrderNo);
            Parameters.Add("out_refund_no", Model.sRefundNo);
            Parameters.Add("total_fee", Model.iTotalFee.ToString());//订单总金额
            Parameters.Add("refund_fee", Model.iRefundFee.ToString());//退款金额
            //创建签名
            Model.sSign = WeChatPayHelper.CreateSign(Parameters, Model.sPayKey);
            Parameters.Add("sign", Model.sSign);
            string sRequestXmlStr = WeChatPayHelper.InstallXml(Parameters);
            //获取微信响应结果
            string sResponeXmlStr = WeChatPayHelper.HttpPost(sUrl, sRequestXmlStr, Model,true);//申请退款需要证书
            var ResponeParameter = WeChatPayHelper.GetDictionaryFromCDATAXml(sResponeXmlStr);
            //判断返回结果
            if (ResponeParameter["return_code"] == "SUCCESS")
            {
                if (WeChatPayHelper.CheckSign(ResponeParameter, Model.sPayKey))
                {//验证签名正确
                    if (ResponeParameter["result_code"] == "SUCCESS")
                    {//申请退款成功
                        Respone.bSuccess = true;
                    }
                    else
                    {
                        Respone.bSuccess = false;
                        Respone.sReturnMsg = ResponeParameter["err_code_des"];
                    }
                }
                else
                {
                    //签名验证失败
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
