using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信支付接口
    /// </summary>
    public interface IWeChatPayRequest
    {
        /// <summary>
        /// 执行微信请求
        /// </summary>
        /// <param name="Model"></param>
        WeChatPayRespone DoRequest(WeChatPayModel model);
    }
}
