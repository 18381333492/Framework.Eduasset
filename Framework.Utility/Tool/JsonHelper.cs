using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utility
{
    public class JsonHelper
    {
        /// <summary>
        /// 将对象序列化成JSON字符串
        /// </summary>
        /// <param name="o">序列化的对象</param>
        /// <returns></returns>
        public static string ToJsonString(object o)
        {
            return JsonConvert.SerializeObject(o, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }

        /// <summary>
        /// 将JSON字符串反序列化成泛型T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sJsonString"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string sJsonString)
        {
            var result = default(T);
            result = JsonConvert.DeserializeObject<T>(sJsonString);
            return result;
        }
    }
}
