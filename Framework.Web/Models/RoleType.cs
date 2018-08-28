using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.Web
{
    /// <summary>
    /// 用户类型
    /// </summary>
    public enum RoleType
    {
        /// <summary>
        /// 平台管理人员
        /// </summary>
        Admin = 0,

        /// <summary>
        /// 维修单位
        /// </summary>
        RepairMan = 1,

        /// <summary>
        /// 使用单位（学校）
        /// </summary>
        School = 2,

    }
}