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
                Parameter.method = "Login";
                Parameter.ArgsArray.Add("userName", userName);
                Parameter.ArgsArray.Add("psd", psd);
                var respone=HttpHelper.HttpGet(Parameter);
                if (respone.Code == 1)
                {//登录成功
                    LoginStatus = JsonHelper.Deserialize<LoginCacheInfo>(JsonHelper.ToJsonString(respone.Data));
                    LoginStatus.Password = psd;
                    //缓存用户信息
                    Session[SessionKey] = LoginStatus;
                    result.success = true;
                    result.data =(int)LoginStatus.RoleType;
                }
                else
                {
                    result.info = respone.Msg;
                }
                return Json(result);
            }
        }
    }
}