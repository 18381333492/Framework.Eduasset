using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信支付结果查询的参数实体
    /// </summary>
    public class WeChatPayQueryModel: WeChatPayModel
    {
        /// <summary>
        /// 微信订单号 (商户订单号和微信订单号二选一)
        /// </summary>
        public string sTransactionNo { get; set; }
    }
}
