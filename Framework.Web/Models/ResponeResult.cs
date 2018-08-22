using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.Web
{ 
    public class ResponeResult
    {
        /// <summary>
        /// 是否成功标识
        /// </summary>
        public bool success
        {
            get;set;
        }

        /// <summary>
        /// 返回消息提示
        /// </summary>
        public string info
        {
            get;set;
        }


        /// <summary>
        /// 返回的数据
        /// </summary>
        public object data
        {
            get;set;
        }
    }
}