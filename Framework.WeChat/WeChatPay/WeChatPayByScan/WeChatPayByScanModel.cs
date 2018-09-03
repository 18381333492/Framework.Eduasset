using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信扫码支付的参数Model
    /// </summary>
    public class WeChatPayByScanModel: WeChatPayModel
    {
        /// <summary>
        /// 交易类型
        /// </summary>
        public WeChatPayType sTradeType
        {
            get;
            set;
        } = WeChatPayType.NATIVE;

        /// <summary>
        /// trade_type=NATIVE时（即扫码支付），此参数必传。
        /// 此参数为二维码中包含的商品ID，商户自行定义。
        /// </summary>
        public string sProductId { get; set; }

        /// <summary>
        /// 自定义参数
        /// </summary>
        public string sAttach { get; set; }
    }
}
