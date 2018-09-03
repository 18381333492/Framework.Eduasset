using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 公众号支付的响应
    /// </summary>
    public class WeChatPayByJsRespone: WeChatPayRespone
    {
        /// <summary>
        /// 微信生成的预支付会话标识，
        /// 用于后续接口调用中使用，
        /// 该值有效期为2小时
        /// </summary>
        public string sPrepayId { get; set; }
    }
}
