using Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.Web.Controllers
{
    /// <summary>
    /// 工单控制器相关
    /// </summary>
    public class WorkOrderController : BaseController
    {
        // GET: WorkOrder

        /// <summary>
        /// 获取维修单位工单数据列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 分页获取主管单位的待审批工单数据列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Manager()
        {
            return View();
        }


        /// <summary>
        /// 分页获取工单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWorkOrderList(PageInfo pageInfo, string startDate, string endDate,string orgName, int state = 0)
        {
            Parameter.method = "GetWorkOrderList";//分页获取保修单
            Parameter.ArgsArray.Add("page", pageInfo.page);
            Parameter.ArgsArray.Add("rows", pageInfo.rows);
            Parameter.ArgsArray.Add("state", state);
            //时间的过滤查询
            if (!string.IsNullOrEmpty(startDate))
                Parameter.ArgsArray.Add("startDate", startDate);
            if (!string.IsNullOrEmpty(endDate))
                Parameter.ArgsArray.Add("endDate", endDate);
            var respone = HttpHelper.HttpGet(Parameter);
            if (respone.Code == 1)
            {
                result.success = true;
                result.data = JsonHelper.ToJsonString(respone.Data);
            }
            result.info = respone.Msg;
            return Json(result);
        }

        /// <summary>
        /// 创建工单
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert()
        {
            return View();
        }

        /// <summary>
        ///  获取工单详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(string ID)
        {
            Parameter.method = "GetWorkOrderInfoByID";  //获取单条工单信息
            Parameter.ArgsArray.Add("ID", ID);
            var respone = HttpHelper.HttpGet(Parameter);
            if (respone.Code == 1)
            {
                ViewBag.RoleType = LoginStatus.RoleType;
                dynamic FormInfo = (dynamic)respone.Data;
                return View(FormInfo);
            }
            else
            {
                return Json(result);
            }
        }

        /// <summary>
        /// 根据ID获取工单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetInfoById(string ID)
        {
            Parameter.method = "GetWorkOrderInfoByID";  //获取单条工单信息
            Parameter.ArgsArray.Add("ID", ID);
            var respone = HttpHelper.HttpGet(Parameter);
            if (respone.Code == 1)
            {
                result.success = true;
                result.data = new
                {
                    info = JsonHelper.ToJsonString(respone.Data),
                    roleType = (int)LoginStatus.RoleType
                };
            }
            else
            {
                result.success = false;
                result.info = "获取数据失败";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取学校下来列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSchoolList()
        {
            Parameter.method = "GetSchoolList";
            var respone = HttpHelper.HttpGet(Parameter);
            if (respone.Code == 1)
            {
                result.data = JsonHelper.ToJsonString(respone.Data);
                result.success = true;
            }
            else
                result.info = "获取数据失败";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}