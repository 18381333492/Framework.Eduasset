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
        public ActionResult Insert(string deviceCode)
        {
            if (!Request.IsAjaxRequest())
            {
                Parameter.method = "GetDeviceInfoByCode";//通过二维码获取设备信息
                Parameter.ArgsArray.Add("deviceCode", deviceCode);
                var respone=HttpHelper.HttpGet(Parameter);
                if (respone.Code == 1)
                {
                    dynamic deviceInfo=JsonHelper.Deserialize<dynamic>(JsonHelper.ToJsonString(respone.Data));
                    return View(deviceInfo);
                }
                else
                {
                    result.info = "设备信息错误";
                    return View(result);
                }
            }
            else
            {
                Parameter.method = "SaveRepairApply";//提交保修单
                Parameter.ArgsArray.Add("DeviceCode", Request["deviceCode"]);
                Parameter.ArgsArray.Add("ClassName", Request["ClassName"]);
                Parameter.ArgsArray.Add("HouseNo", Request["HouseNo"]);
                Parameter.ArgsArray.Add("StorePlace", Request["StorePlace"]);
                Parameter.ArgsArray.Add("Linkman", Request["Linkman"]);
                Parameter.ArgsArray.Add("ContactNumber", Request["ContactNumber"]);
                Parameter.ArgsArray.Add("FaultDate", Request["FaultDate"]);
                Parameter.ArgsArray.Add("FaultName", Request["FaultName"]);
                Parameter.ArgsArray.Add("FaultDesc", Request["FaultDesc"]);
                Parameter.ArgsArray.Add("ApplyerName", LoginStatus.RealName);
                //Parameter.ArgsArray.Add("FaultPicPath1", Request["FaultPicPath1"]);
                var respone= HttpHelper.HttpPost(Parameter);
                if(respone.Code == 1)
                {
                    result.success = true;
                    result.info = "保修成功";
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