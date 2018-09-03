﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WeChat.WeChatMessage
{
    /// <summary>
    /// 微信推送关注事件的实体
    /// </summary>
    public class SubscribeEvent:IRMessage
    {
        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// </summary>
        public string EventKey
        {
            get;
            set;
        }

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket
        {
            get;
            set;
        }
    }
}
