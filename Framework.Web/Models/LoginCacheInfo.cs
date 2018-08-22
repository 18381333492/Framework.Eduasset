using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.Web
{
    /// <summary>
    /// 登录缓存的信息
    /// </summary>
    [Serializable]
    public class LoginCacheInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public int RoleType { get; set; }

        /// <summary>
        /// 学校编码
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string OrgName { get; set; }
    }
}