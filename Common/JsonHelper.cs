using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace BitAuto.CarChannel.Common
{
	public class JsonHelper
	{
		public static string Serialize<T>(T obj)
		{
			string strJSON = string.Empty;
			DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(T));
			using (MemoryStream ms = new MemoryStream())
			{
				ds.WriteObject(ms, obj);
				strJSON = Encoding.UTF8.GetString(ms.ToArray());
			}
			return strJSON;
		}

		public static T Deserialize<T>(string sJson) where T : class
		{
			T obj = default(T);
			DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(T));
			using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(sJson)))
			{
				obj = (T)ds.ReadObject(ms);
			}
			return obj;
		}
	}
}
