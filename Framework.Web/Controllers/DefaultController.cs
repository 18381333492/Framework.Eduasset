using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.Web.Controllers
{
    public class DefaultController : BaseController
    {
        // GET: Default
        public ActionResult Index()
        {
            if (LoginStatus.RoleType == RoleType.School)
            {
                return Redirect("/Form/Teacher");
            }
            if (LoginStatus.RoleType == RoleType.RepairUnit)
            {
                return Redirect("/Form/Repair");
            }
            if (LoginStatus.RoleType == RoleType.RepairMan)
            {
                return Redirect("/Form/RepairMan");
            }
            if (LoginStatus.RoleType == RoleType.Manager)
            {
                return Redirect("/WorkOrder/Manager");
            }
            return View();
        }

        /// <summary>
        /// 通过扫一扫进入
        /// </summary>
        /// <param name="houseCode">教室门牌号编码</param>
        /// <returns></returns>
        [NoLogin]
        public ActionResult Scan(string houseCode)
        {
            if (Session[SessionKey] != null)
            {
                LoginStatus = (LoginCacheInfo)Session[SessionKey];
                if (LoginStatus.RoleType == RoleType.School)
                {//跳转报修页面
                    return Redirect("/Form/Insert?houseCode="+ houseCode);
                }
                if (LoginStatus.RoleType == RoleType.RepairUnit)
                {
                    return Redirect("/Form/Repair");
                }
                if (LoginStatus.RoleType == RoleType.RepairMan)
                {//跳转直接创建工单页面
                    return Redirect("/WorkOrder/DirectlyInsert?houseCode=" + houseCode);
                }
                if (LoginStatus.RoleType == RoleType.Manager)
                {
                    return Redirect("/WorkOrder/Manager");
                }
                return Redirect("/User/Login");
            }
            else
            {
                return Redirect("/User/Login?houseCode="+ houseCode);
            }
        }
    }
}