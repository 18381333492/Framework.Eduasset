using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信查询结果的响应
    /// </summary>
    public class WeChatPayQueryRespone: WeChatPayRespone
    {
        /// <summary>
        /// 交易状态
        /// </summary>
        public bool TradeState { get; set; }

        /// <summary>
        /// 交易状态描述
        /// </summary>
        public string TradeStateMsg { get; set; }
    }
}
