using Framework.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TraceLogs;

namespace Framework.WeChat.Tool
{
    /// <summary>
    /// 微信公众号帮助类
    /// </summary>
    public class WeChatHelper
    {
        public static ILogger logger = LoggerManager.Instance.GetSLogger("WeChat");

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GetRandomStr()
        {
            Random Ran = new Random();
            string nonce_str = Ran.Next(11111111, 99999999).ToString() + "CNMB";
            return SecurityHelper.md5(nonce_str);
        }

        /// <summary>
        /// 获取时间撮(自1970年以来的秒数)
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static string CreateSha1Sign(Dictionary<string, string> Parameters)
        {
            StringBuilder Sign = new StringBuilder();
            var Keys = new ArrayList(Parameters.Keys);
            Keys.Sort();//字典排序
            foreach (string key in Keys)
            {
                if (!string.IsNullOrEmpty(Parameters[key]))
                {//拼接成键值对字符串
                    Sign.Append(key + "=" + Parameters[key] + "&");
                }
            }
            string sign = SecurityHelper.SHA1(Sign.ToString().TrimEnd('&')).ToLower();
            return sign;
        }
    }
}
