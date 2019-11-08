using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace School.Common.JosnTransformation
{
    public class JsonUtil<T>
    {
        /// <summary>  
        /// 获取将实体类转换为json数据（目的是为了更快在网页上传递数据）  
        /// </summary>  
        /// <returns></returns>  
        public static string getJsonInfo(T personInfo)
        {
            //将对象序列化json  
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            //创建存储区为内存流  
            System.IO.MemoryStream ms = new MemoryStream();
            //将json字符串写入内存流中  
            serializer.WriteObject(ms, personInfo);
            System.IO.StreamReader reader = new StreamReader(ms);
            ms.Position = 0;
            string strRes = reader.ReadToEnd();
            reader.Close();
            ms.Close();
            return strRes;

        }

        /// <summary>
        /// 将JSON数据转成实体数据（用例 personJsonList.Add(JsonUtil<PersonVM>.GetObjectByJson((JsonUtil<PersonVM>.getJsonInfo(personCollection))));）
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T GetObjectByJson(string jsonString)
        {
            // 实例化DataContractJsonSerializer对象，需要待序列化的对象类型  
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<T>));
            //把Json传入内存流中保存  
            jsonString = "[" + jsonString + "]";
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            // 使用ReadObject方法反序列化成对象  
            object ob = serializer.ReadObject(stream);
            List<T> ls = (List<T>)ob;
            return ls[0];
        }
    }
}
