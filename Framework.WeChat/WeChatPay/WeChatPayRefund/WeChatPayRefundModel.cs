using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 微信申请退款实体
    /// </summary>
    public class WeChatPayRefundModel: WeChatPayModel
    {
        /// <summary>
        /// 退款订单号
        /// </summary>
        public string sRefundNo
        {
            get;set;
        }

        /// <summary>
        /// 退款金额(单位分)
        /// </summary>
        public int iRefundFee
        {
            get;set;
        }
    }
}
