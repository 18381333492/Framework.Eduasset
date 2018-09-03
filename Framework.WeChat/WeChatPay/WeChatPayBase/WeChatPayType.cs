using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    public enum WeChatPayType 
    {
        JSAPI,//微信公众号支付
        NATIVE,//原生支付(扫码支付)
        MWEB,//H5支付
        APP,//App支付
    }
}
