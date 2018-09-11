using Framework.Utility;
using Newtonsoft.Json.Linq;
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
        private static readonly string ImageUrl = ConfigHelper.GetAppSetting("ImageUrl");
        /// <summary>
        /// 获取维修单位工单数据列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(LoginStatus);
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
        /// 获取维修人员历史任务
        /// </summary>
        /// <returns></returns>
        public ActionResult History()
        {
            return View();
        }

        /// <summary>
        /// 获取工单统计数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Count()
        {
            return View();
        }

        /// <summary>
        /// 获取工单各状态统计
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWorkOrderStateCount()
        {
            Parameter.method = "GetWorkOrderStateCount"; 
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
        /// 分页获取工单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWorkOrderList(PageInfo pageInfo, string startDate, string endDate, string orgName, string workOrderState, string state)
        {
            Parameter.method = "GetWorkOrderList";//分页获取保修单
            Parameter.ArgsArray.Add("page", pageInfo.page);
            Parameter.ArgsArray.Add("rows", pageInfo.rows);
            if (!string.IsNullOrEmpty(state))
                Parameter.ArgsArray.Add("state", state);
            if (!string.IsNullOrEmpty(workOrderState))
                Parameter.ArgsArray.Add("workOrderState", workOrderState);
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
        /// 根据保修单创建工单
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert(string ID,string submitDatas)
        {
            if (!Request.IsAjaxRequest())
            {
                var respone = GetRepairApplyInfoByID(ID);
                if (respone.Code == 1)
                {
                    ViewBag.ImageUrl = ImageUrl;
                    ViewBag.RealName = LoginStatus.RealName;
                    dynamic FormInfo = (dynamic)respone.Data;
                    return View(FormInfo);
                }
                else
                {
                    return View(result);
                }
            }
            else
            {
                Parameter.method = "SaveWorkOrderByRepairApply";//工单提交（通过报修单方式）
                submitDatas = submitDatas.Replace(ImageUrl, string.Empty);
                string sBody = string.Format("submitDatas={0}", submitDatas);
                var respone = HttpHelper.HttpPost(Parameter, sBody);
                if (respone.Code == 1)
                {
                    result.success = true;
                    result.info = "创建成功";
                }
                else
                {
                    result.info = "创建失败";
                }
                return Json(result);
            }
        }

        /// <summary>
        /// 直接创建工单
        /// </summary>
        /// <param name="submitDatas"></param>
        /// <returns></returns>
        public ActionResult DirectlyInsert(string submitDatas)
        {
            if(!Request.IsAjaxRequest())
            {
                return View(LoginStatus);
            }
            else
            {
                Parameter.method = "SaveWorkOrderDirectly";//直接保存工单
                submitDatas = submitDatas.Replace(ImageUrl, string.Empty);
                string sBody = string.Format("submitDatas={0}", submitDatas);
                var respone = HttpHelper.HttpPost(Parameter, sBody);
                if (respone.Code == 1)
                {
                    result.success = true;
                    result.info = "创建成功";
                }
                else
                {
                    result.info = "创建失败";
                }
                return Json(result);
            }
        }

        /// <summary>
        /// 工单撤销
        /// </summary>
        /// <param name="submitDatas"></param>
        /// <returns></returns>
        public ActionResult CancelWorkOrder(string submitDatas)
        {
            Parameter.method = "CancelWorkOrder";
            JObject job = JsonHelper.Deserialize<JObject>(submitDatas);
            job.Add(new JProperty("CancelerName", LoginStatus.RealName));
            string sBody = string.Format("submitDatas={0}", job.ToString());
            var respone = HttpHelper.HttpPost(Parameter, sBody);
            if (respone.Code == 1)
            {
                result.success = true;
                result.info = "撤回成功";
            }
            else
            {
                result.info = "撤回失败";
            }
            return Json(result);
        }

        /// <summary>
        /// 评价工单
        /// </summary>
        /// <param name="submitDatas"></param>
        /// <returns></returns>
        public ActionResult SureWorkOrder(string submitDatas)
        {
            Parameter.method = "CancelWorkOrder";
            JObject job = JsonHelper.Deserialize<JObject>(submitDatas);
            job.Add(new JProperty("SurerName", LoginStatus.RealName));
            string sBody = string.Format("submitDatas={0}", job.ToString());
            var respone = HttpHelper.HttpPost(Parameter, sBody);
            if (respone.Code == 1)
            {
                result.success = true;
                result.info = "评价成功";
            }
            else
            {
                result.info = "评价失败";
            }
            return Json(result);
        }

        /// <summary>
        /// 工单审批-通过
        /// </summary>
        /// <param name="submitDatas"></param>
        /// <returns></returns>
        public ActionResult PassWorkOrder(string submitDatas)
        {
            Parameter.method = "PassWorkOrder";
            JObject job = JsonHelper.Deserialize<JObject>(submitDatas);
            job.Add(new JProperty("ApproveName", LoginStatus.RealName));
            string sBody = string.Format("submitDatas={0}", job.ToString());
            var respone = HttpHelper.HttpPost(Parameter, sBody);
            if (respone.Code == 1)
            {
                result.success = true;
                result.info = "操作成功";
            }
            else
            {
                result.info = "操作失败";
            }
            return Json(result);
        }

        /// <summary>
        ///  工单审批-驳回
        /// </summary>
        /// <param name="submitDatas"></param>
        /// <returns></returns>
        public ActionResult UnPassWorkOrder(string submitDatas)
        {
            Parameter.method = "UnPassWorkOrder";
            JObject job = JsonHelper.Deserialize<JObject>(submitDatas);
            job.Add(new JProperty("ApproveName", LoginStatus.RealName));
            string sBody = string.Format("submitDatas={0}", job.ToString());
            var respone = HttpHelper.HttpPost(Parameter, sBody);
            if (respone.Code == 1)
            {
                result.success = true;
                result.info = "操作成功";
            }
            else
            {
                result.info = "操作失败";
            }
            return Json(result);
        }


        /// <summary>
        ///  获取工单详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(string ID,string repairApplyId)
        {
            Parameter.method = "GetWorkOrderInfoByID";  //获取单条工单信息
            if(!string.IsNullOrEmpty(ID))
            {
                Parameter.ArgsArray.Add("ID", ID);
            }
            if (!string.IsNullOrEmpty(repairApplyId))
            {
                Parameter.ArgsArray.Add("repairApplyId", repairApplyId);
            }
            var respone = HttpHelper.HttpGet(Parameter);
            if (respone.Code == 1)
            {
                ViewBag.ImageUrl = ImageUrl;
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
        public ActionResult GetInfoById(string ID,string repairApplyId)
        {
            Parameter.method = "GetWorkOrderInfoByID";  //获取单条工单信息
            if (!string.IsNullOrEmpty(ID))
                Parameter.ArgsArray.Add("ID", ID);
            if (!string.IsNullOrEmpty(repairApplyId))
                Parameter.ArgsArray.Add("repairApplyId", repairApplyId);   
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

        /// <summary>
        /// 根据ID获取单条保修单信息
        /// </summary>
        /// <returns></returns>
        private HttpResult GetRepairApplyInfoByID(string ID)
        {
            Parameter.method = "GetRepairApplyInfoByID";  //获取单条报修单信息
            Parameter.ArgsArray.Add("ID", ID);
            var respone = HttpHelper.HttpGet(Parameter);
            return respone;
        }            
    }
}