using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信支付相关参数实体
    /// </summary>
    public class WeChatPayModel
    {
        /// <summary>
        /// 微信公众号的APPID
        /// </summary>
        public string sAppId { get; set; }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string sMchId { get; set; }

        /// <summary>
        /// 证书路径
        /// </summary>
        public string sCertPath { get; set; }

        /// <summary>
        /// 支付秘钥
        /// </summary>
        public string sPayKey { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string sNonceStr { get; set; }

        /// <summary>
        /// 异步通知地址
        /// </summary>
        public string sNotifyUrl { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string sBody { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sSign { get; set; }

        /// <summary>
        /// 支付总金额(单位为分)
        /// </summary>
        public int iTotalFee { get; set; }

        /// <summary>
        /// APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP。
        /// </summary>
        public string spbill_create_ip { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string sOrderNo { get; set; }

        /// <summary>
        /// 商户自定义数据
        /// </summary>
        public string attach
        {
            get;set;
        }

    }
}
