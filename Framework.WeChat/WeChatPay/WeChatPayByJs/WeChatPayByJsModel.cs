using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信公众号支付的参数Model
    /// </summary>
    public class WeChatPayByJsModel : WeChatPayModel
    {
        /// <summary>
        /// 交易类型
        /// </summary>
        public WeChatPayType sTradeType
        {
            get;
            set;
        } = WeChatPayType.JSAPI;

        /// <summary>
        /// trade_type=JSAPI时（即公众号支付），此参数必传，
        /// 此参数为微信用户在商户对应appid下的唯一标识
        /// </summary>
        public string sOpenId { get; set; }
    }
}
