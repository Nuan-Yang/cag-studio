using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class JsonExtension
{
    /// <summary>
    /// 添加扩展方法将对象转json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJson<T>(this T obj) where T : class, new()
    {
        return JsonConvert.SerializeObject(obj);
    }

    /// <summary>
    /// 讲json转化为对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T ToModel<T>(this string json) where T : class, new()
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}

