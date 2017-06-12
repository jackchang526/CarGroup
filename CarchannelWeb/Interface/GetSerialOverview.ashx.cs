using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using BitAuto.Utils;
using PieceDataProviderLib;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface
{
	/// <summary>
	/// GetSerialOverview 的摘要说明
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class GetSerialOverview : InterfacePageBase, IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			PieceClient pc = new PieceClient(context, "123456");
			pc.ReadConfig += new ReadConfigHandle(ReadConfig);
			pc.GetDataSet += new GetDataSetHandle(GetDataSet);

			pc.UpdateTime = System.DateTime.Now;
			pc.RunCommand();
		}

		/// <summary>
		/// 返回用户需要的配置文件
		/// </summary>
		public string ReadConfig()
		{
			string cfgFile = Path.Combine(WebConfig.WebRootPath, "Interface\\GetSerialOverview.xml");
			string config = File.ReadAllText(cfgFile, Encoding.UTF8);
			return config;
		}

		/// <summary>
		/// 返回用户需要的DATASET的XML文件
		/// </summary>
		/// <param name="sc">搜索条件对象</param>
		/// <returns>DATASET的XML结果</returns>
		public System.Data.DataSet GetDataSet(SearchCondition sc)
		{
			Condition c = (Condition)sc.ConditionFields[0];
			foreach (MultCondition mc in sc.ConditionFields)
			{
				if (mc is Condition)
				{
					if (((Condition)mc).FieldName == "PushBrandId")
					{
						c = (Condition)mc;
						break;
					}
				}
			}
			string[] ids = c.DefaultValue.ToString().Split(',');
			List<int> serialIdList = new List<int>();
			foreach (string sId in ids)
			{
				int serialId = ConvertHelper.GetInteger(sId);
				if (!serialIdList.Contains(serialId))
					serialIdList.Add(serialId);
			}
			return new Car_SerialBll().GetSerialOverview(serialIdList.ToArray());
		}

		public bool IsReusable
		{
			get { throw new Exception("The method or operation is not implemented."); }
			//get
			//{
			//    return false;
			//}
		}
	}
}