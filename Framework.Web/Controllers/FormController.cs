﻿using System;
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
        /// 添加报修单
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
        /// 根据机构编码获取门牌号列表
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public ActionResult GetHouseListByOrgCode(string OrgCode)
        {
            Parameter.method = "GetHouseListByOrgCode";
            Parameter.ArgsArray.Add("OrgCode", OrgCode);
            var respone = HttpHelper.HttpGet(Parameter);
            if (respone.Code == 1)
            {
                result.success = true;
            }
            else
                result.info = "获取数据失败";
            return Json(result,JsonRequestBehavior.AllowGet);
        }               
    }
}