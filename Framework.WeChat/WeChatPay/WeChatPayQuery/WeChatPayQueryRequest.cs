using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信支付结果查询请求
    /// </summary>
    public class WeChatPayQueryRequest: IWeChatPayRequest
    {
        private static string sUrl = "https://api.mch.weixin.qq.com/pay/orderquery";


        /// <summary>
        /// 查询微信支付结果请求
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public WeChatPayRespone DoRequest(WeChatPayModel model)
        {
            WeChatPayQueryModel Model = model as WeChatPayQueryModel;
            WeChatPayQueryRespone Respone = new WeChatPayQueryRespone();
            var Parameters = new Dictionary<string, string>();
            Model.sNonceStr = WeChatPayHelper.GetRandomStr();//获取随机字符串
            Parameters.Add("appid", Model.sAppId);
            Parameters.Add("mch_id", Model.sMchId);
            Parameters.Add("transaction_id", Model.sTransactionNo);
            Parameters.Add("out_trade_no", Model.sOrderNo);
            Parameters.Add("nonce_str", Model.sNonceStr);
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
                    {//查询成功
                        if (ResponeParameter["trade_state"] == "SUCCESS")
                        {
                            Respone.bSuccess = true;
                            Respone.sReturnMsg = ResponeParameter["return_msg"];
                            Respone.TradeState = true;//交易状态 支付成功
                            Respone.TradeStateMsg = ResponeParameter["trade_state"];
                        }
                        else
                        {
                            Respone.bSuccess = true;
                            Respone.sReturnMsg = ResponeParameter["return_msg"];
                            Respone.TradeState = false;//交易状态
                            Respone.TradeStateMsg = ResponeParameter["trade_state"];
                        }
                    }
                    else
                    {
                        Respone.bSuccess = false;
                        Respone.sReturnMsg = ResponeParameter["err_code_des"];
                        Respone.TradeState = false;
                        Respone.TradeStateMsg = Respone.sReturnMsg;
                    }
                }
                else
                {
                    //签名验证失败
                    Respone.bSuccess = false;
                    Respone.sReturnMsg = "签名验证失败";
                    Respone.TradeState = false;
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
