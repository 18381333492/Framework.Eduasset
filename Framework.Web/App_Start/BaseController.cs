using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.Web.App_Start
{
    public class BaseController: Controller
    {
        /// <summary>
        /// 缓存用户信息的SessionKey
        /// </summary>
        public static readonly string SessionKey = "@UserInfo";

        /// <summary>
        /// 返回响应结果
        /// </summary>
        protected ResponeResult result;

        /// <summary>
        /// 登录信息
        /// </summary>
        protected LoginCacheInfo LoginStatus
        {
            get
            {
                return (LoginCacheInfo)Session[SessionKey];
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseController()
        {
            result = new ResponeResult();
        }

        /// <summary>
        /// Action之前验证登录
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ////是否需要验证登录
            //bool needLogin = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoLogin), true).Length == 1 ? false : true;
            //if (needLogin)
            //{
            //    if (LoginStatus == null)
            //    {//登录过期
            //        filterContext.Result = new RedirectResult("/User/TimeOut");
            //    }
            //}
        }


        /// <summary>
        /// 异常捕捉
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            ////异常的打印
            //logger.Info("Controller的异常信息:" + filterContext.Exception.Message);
            //logger.Fatal("Controller的异常信息:" + filterContext.Exception.Message, filterContext.Exception);
            result.info = "系统异常";
            result.success = false;
           // filterContext.Result = Content(JsonHelper.ToJsonString(result));
        }
    }
}