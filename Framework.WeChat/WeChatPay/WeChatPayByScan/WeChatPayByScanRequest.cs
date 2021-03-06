﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信扫码支付请求
    /// </summary>
    public class WeChatPayByScanRequest: IWeChatPayRequest
    {
        /// <summary>
        /// 扫码支付请求的Url
        /// </summary>
        private static string sUrl ="https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 扫码支付下单
        /// </summary>
        /// <param name="Model"></param>
        public WeChatPayRespone DoRequest(WeChatPayModel model)
        {
            WeChatPayByScanModel Model = model as WeChatPayByScanModel;
            WeChatPayByScanRespone Respone = new WeChatPayByScanRespone();
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
            Parameters.Add("product_id", Model.sProductId);
            Parameters.Add("attach", Model.sAttach);//自定义参数
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
                        Respone.sCodeUrl = ResponeParameter["code_url"];//返回的支付二维码链接有效期2个小时
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
