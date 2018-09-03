using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信支付的响应
    /// </summary>
    public class WeChatPayRespone
    {
        /// <summary>
        /// 本次请求的成功标识
        /// </summary>
        public bool bSuccess { get; set; }

        /// <summary>
        /// 微信返回的提示消息
        /// </summary>
        public string sReturnMsg { get; set; }

        /// <summary>
        /// 微信直接返回的XML数据
        /// </summary>
        public Dictionary<string,string> sReturnParams { get; set; }

    }
}
