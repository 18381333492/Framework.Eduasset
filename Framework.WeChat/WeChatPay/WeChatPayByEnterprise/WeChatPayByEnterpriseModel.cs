using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatPay
{
    /// <summary>
    /// 企业付款实体Model
    /// </summary>
    public class WeChatPayByEnterpriseModel: WeChatPayModel
    {
        /// <summary>
        /// 微信用户的OpenId
        /// </summary>
        public string sOpenId { get; set; }

        /// <summary>
        /// 企业付款操作说明信息
        /// </summary>
        public string sDescribe { get; set; }
    }
}
