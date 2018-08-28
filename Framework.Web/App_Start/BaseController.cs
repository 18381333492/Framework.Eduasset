using Framework.Utility;
using Framework.Utility.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraceLogs;

namespace Framework.Web
{
    public class BaseController: Controller
    {

        protected static ILogger logger = LoggerManager.Instance.GetSLogger("Web");

        /// <summary>
        /// 缓存用户信息的SessionKey
        /// </summary>
        public static readonly string SessionKey = "@UserInfo";


        protected HttpParameter Parameter;

        /// <summary>
        /// 返回响应结果
        /// </summary>
        protected ResponeResult result;

        /// <summary>
        /// 登录信息
        /// </summary>
        protected LoginCacheInfo LoginStatus;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseController()
        {
            result = new ResponeResult();
            Parameter = new HttpParameter();
        }

        /// <summary>
        /// Action之前验证登录
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //是否需要验证登录
            bool needLogin = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoLogin), true).Length == 1 ? false : true;
            if (needLogin)
            {
                LoginStatus =(LoginCacheInfo)Session[SessionKey];
                if (LoginStatus != null)
                {
                    Parameter.ArgsArray.Add("userId", LoginStatus.UserID);
                    Parameter.ArgsArray.Add("psd", LoginStatus.Password);
                    Parameter.ArgsArray.Add("roleType", (int)LoginStatus.RoleType);
                }
                else
                {//登录过期
                    filterContext.Result = Redirect("/User/Login");
                }
            }
        }


        /// <summary>
        /// 异常捕捉
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            //异常的打印
            logger.Info("Controller的异常信息:" + filterContext.Exception.Message);
            logger.Fatal(filterContext.Exception);
            result.info = "系统异常";
            result.success = false;
            filterContext.Result = Content(JsonHelper.ToJsonString(result));
        }
    }
}