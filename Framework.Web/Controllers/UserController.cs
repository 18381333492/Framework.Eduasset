using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Utility;

namespace Framework.Web.Controllers
{
    public class UserController : BaseController
    {
        // GET: User

        /// <summary>
        ///  用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="psd"></param>
        /// <returns></returns>
        [NoLogin]
        public ActionResult Login(string userName,string psd)
        {
            if (!Request.IsAjaxRequest())
            {
                return View();
            }
            else
            {
                return Json(result);
            }
        }
    }
}