using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Utility;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Framework.Web.Controllers
{
    public class FormController : BaseController
    {
        // GET: Form
        private static readonly string ImageUrl = ConfigHelper.GetAppSetting("ImageUrl");
        /// <summary>
        /// 老师保修单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Teacher()
        {
            return View();
        }

        /// <summary>
        /// 维修单位报修单列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public ActionResult Repair()
        {
            return View();
        }

        /// <summary>
        /// 维修单位的维修任务列表
        /// </summary>
        /// <returns></returns>
        public ActionResult RepairTask()
        {
            return View();
        }

        /// <summary>
        /// 维修人员的任务
        /// </summary>
        /// <returns></returns>
        public ActionResult RepairMan()
        {
            return View();
        }

        /// <summary>
        /// 分页获取报修单列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult GetRepairApplyList(PageInfo pageInfo, string startDate, string endDate, string repairApplyState, string state)
        {
            Parameter.method = "GetRepairApplyList";//分页获取保修单
            Parameter.ArgsArray.Add("page", pageInfo.page);
            Parameter.ArgsArray.Add("rows", pageInfo.rows);
            if (!string.IsNullOrEmpty(state))
                Parameter.ArgsArray.Add("state", state);
            if (!string.IsNullOrEmpty(repairApplyState))
                Parameter.ArgsArray.Add("repairApplyState", repairApplyState);
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
        /// 添加报修单
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert(string houseCode,string submitDatas)
        {
            if (!Request.IsAjaxRequest())
            {
                ViewBag.houseCode = houseCode;
                return View(LoginStatus);
            }
            else
            {
                if (string.IsNullOrEmpty(submitDatas))
                {
                    result.info = "参数错误";
                    return Json(result);
                }
                Parameter.method = "SaveRepairApply";//提交保修单
                submitDatas = submitDatas.Replace(ImageUrl, string.Empty);
                string sBody = string.Format("submitDatas={0}", submitDatas);
                var respone = HttpHelper.HttpPost(Parameter, sBody);
                if (respone.Code == 1)
                {
                    result.success = true;
                    result.info = "保修成功";
                }
                else
                {
                    result.info = "保修失败";
                }
                return Json(result);
            }
        }

        /// <summary>
        /// 报修单撤销
        /// </summary>
        /// <param name="submitDatas"></param>
        /// <returns></returns>
        public ActionResult CancelRepairApply(string submitDatas)
        {
            Parameter.method = "CancelRepairApply";//提交保修单
            JObject job = JsonHelper.Deserialize<JObject>(submitDatas);
            job.Add(new JProperty("CancelerName", LoginStatus.RealName));
            string sBody = string.Format("submitDatas={0}", job.ToString());
            var respone = HttpHelper.HttpPost(Parameter, sBody);
            if (respone.Code == 1)
            {
                result.success = true;
                result.info = "撤销成功";
            }
            else
            {
                result.info = "撤销失败";
            }
            return Json(result);
        }

        /// <summary>
        /// 响应保修单
        /// </summary>
        /// <param name="submitDatas"></param>
        /// <returns></returns>
        public ActionResult ResponseRepairApply(string submitDatas)
        {
            Parameter.method = "ResponseRepairApply";//提交保修单
            JObject job = JsonHelper.Deserialize<JObject>(submitDatas);
            job.Add(new JProperty("ResponserName", LoginStatus.RealName));
            string sBody = string.Format("submitDatas={0}", job.ToString());
            var respone = HttpHelper.HttpPost(Parameter, sBody);
            if (respone.Code == 1)
            {
                result.success = true;
                result.info = "响应成功";
            }
            else
            {
                result.info = "响应失败";
            }
            return Json(result);
        }

        /// <summary>
        /// 报修单确认（建议措施类型）
        /// </summary>
        /// <param name="submitDatas"></param>
        /// <returns></returns>
        public ActionResult ConfirmRepairApply(string submitDatas)
        {
            Parameter.method = "ConfirmRepairApply";//提交保修单
            JObject job = JsonHelper.Deserialize<JObject>(submitDatas);
            job.Add(new JProperty("MeasureConfirmerName", LoginStatus.RealName));
            string sBody = string.Format("submitDatas={0}", job.ToString());
            var respone = HttpHelper.HttpPost(Parameter, sBody);
            if (respone.Code == 1)
            {
                result.success = true;
                result.info = "确认成功";
            }
            else
            {
                result.info = "确认失败";
            }
            return Json(result);
        }

        /// <summary>
        /// 查看保修单详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Detail(string ID)
        {
            Parameter.method = "GetRepairApplyInfoByID";  //获取单条报修单信息
            Parameter.ArgsArray.Add("ID", ID);
            var respone = HttpHelper.HttpGet(Parameter);
            if (respone.Code == 1)
            {
               ViewBag.ImageUrl = ImageUrl;
               ViewBag.RoleType = LoginStatus.RoleType;
               dynamic FormInfo=(dynamic)respone.Data;
               return View(FormInfo);
            }
            else
            {
                return Json(result);
            }
        }

        /// <summary>
        /// 根据ID获取保修单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetInfoById(string ID)
        {
            Parameter.method = "GetRepairApplyInfoByID";  //获取单条工单信息
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
        /// 根据设备编号获取设备信息
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public ActionResult GetDeviceInfoByCode(string deviceCode = "0207111010144007")
        {
            Parameter.method = "GetDeviceInfoByCode";//通过二维码获取设备信息
            Parameter.ArgsArray.Add("deviceCode", deviceCode);
            var respone = HttpHelper.HttpGet(Parameter);
            if (respone.Code == 1)
            {
                result.success = true;
                result.data = new
                {
                    deviceInfo =JsonHelper.ToJsonString(respone.Data),
                    RealName = LoginStatus.RealName               
                };
            }
            else
            {
                result.success = false;
                result.info = "设备信息获取失败";     
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据门牌号二维码获取教室信息
        /// </summary>
        /// <param name="houseCode"></param>
        /// <returns></returns>
        public ActionResult GetHouseInfoByHouseCode(string houseCode)
        {
            Parameter.method = "GetHouseInfoByHouseCode";
            Parameter.ArgsArray.Add("houseCode", houseCode);
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
        /// 根据机构编码获取门牌号列表
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public ActionResult GetHouseListByOrgCode(string OrgCode)
        {
            Parameter.method = "GetHouseListByOrgCode";
            Parameter.ArgsArray.Add("orgCode", OrgCode);
            var respone = HttpHelper.HttpGet(Parameter);
            if (respone.Code == 1)
            {
                result.data =JsonHelper.ToJsonString(respone.Data);
                result.success = true;
            }
            else
                result.info = "获取数据失败";
            return Json(result,JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 根据学校代码和门牌号获取设备列表
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="HouseNo"></param>
        /// <returns></returns>
        public ActionResult GetDeviceListByOrgCodeAndHouseNo(string OrgCode,string HouseNo)
        {
            Parameter.method = "GetDeviceListByOrgCodeAndHouseNo";
            Parameter.ArgsArray.Add("orgCode", OrgCode);
            Parameter.ArgsArray.Add("houseNo", HouseNo);
            var respone = HttpHelper.HttpGet(Parameter);
            if (respone.Code == 1)
            {
                result.data = JsonHelper.ToJsonString(respone.Data);
                result.success = true;
            }
            else
                result.info = "获取设备失败";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据维修单位代码获取维修人员列表
        /// </summary>
        /// <param name="integratorCode"></param>
        /// <returns></returns>
        public ActionResult GetServicePersonListByIntegratorCode()
        {
            Parameter.method = "GetServicePersonListByIntegratorCode";
            Parameter.ArgsArray.Add("integratorCode", LoginStatus.OrgCode);
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
        /// 获取维修单位下拉列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRepairOrgList()
        {
            Parameter.method = "GetRepairOrgList";
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
        /// 获取抄送人列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCopyPersonList(string OrgCode)
        {
            Parameter.method = "GetCopyPersonList";
            Parameter.ArgsArray.Add("orgCode", OrgCode);
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