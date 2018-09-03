using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 扫码支付的响应
    /// </summary>
    public class WeChatPayByScanRespone: WeChatPayRespone
    {
        /// <summary>
        /// 返回的支付二维码链接
        /// </summary>
        public string sCodeUrl { get; set; }
    }
}
