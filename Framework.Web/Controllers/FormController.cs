using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Utility;

namespace Framework.Web.Controllers
{
    public class FormController : BaseController
    {
        // GET: Form
        
        /// <summary>
        /// 老师保修单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Teacher(PageInfo pageInfo,string startDate,string endDate,int state=0)
        {
            if (!Request.IsAjaxRequest())
            {
                return View();
            }
            else
            {
                Parameter.method = "GetRepairApplyList";//分页获取保修单
                Parameter.ArgsArray.Add("page", pageInfo.page);
                Parameter.ArgsArray.Add("rows", pageInfo.rows);
                Parameter.ArgsArray.Add("state", state);
                //时间的过滤查询
                if (!string.IsNullOrEmpty(startDate)) 
                    Parameter.ArgsArray.Add("startDate", startDate);
                if (!string.IsNullOrEmpty(endDate))
                    Parameter.ArgsArray.Add("endDate", endDate);
                var respone=HttpHelper.HttpGet(Parameter);
                if (respone.Code == 1)
                {
                    result.success = true;
                    result.data =JsonHelper.ToJsonString(respone.Data);
                }
                result.info = respone.Msg;
                return Json(result);
            } 
        }


        /// <summary>
        /// 老师添加保修单
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert()
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