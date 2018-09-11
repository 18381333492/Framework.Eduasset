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
    }
}