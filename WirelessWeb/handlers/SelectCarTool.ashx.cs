using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using BitAuto.CarChannel.Common.Enum;

namespace WirelessWeb
{
	/// <summary>
	/// SelectCarTool 的摘要说明
	/// </summary>
	public class SelectCarTool : WirelessTreeBase, IHttpHandler
	{
		HttpRequest request;
		HttpResponse response;
		int pageNum = 0;
		int pageSize = 20;
		int SerialNum = 0;
		int CarNum = 0;
        private string sortMode;
		private SelectCarParameters selectParas;
		public new void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/html";
			request = context.Request;
			response = context.Response;
			pageNum = ConvertHelper.GetInteger(request.QueryString["page"]);
			//if (pageNum <= 1) return;
			selectParas = base.GetSelectCarParas();
            sortMode = request.QueryString["s"];
			MakeSelectCar();
		}
		private void MakeSelectCar()
		{
			List<BitAuto.CarChannel.BLL.SerialInfo> tmpList = new SelectCarToolBll().SelectCarByParameters(selectParas);
			List<BitAuto.CarChannel.BLL.SerialInfo> serialList = tmpList.GetRange(0, tmpList.Count);
            //排序
           
            if (sortMode == "2")
            {
                serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinPrice);
            }
            else if (sortMode == "3")
            {
                serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinPriceDesc);
            }
            else
            {
                serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByPVDesc);
            }
			SerialNum = serialList.Count;
			CarNum = 0;
			StringBuilder sb = new StringBuilder();
			if (SerialNum > 0)
			{
				response.Write(MakeImageModeHtml(serialList));
			}
			else
			{
				//无数据
			}
		}

		/// <summary>
		/// 以大图方式显示选车结果
		/// </summary>
		/// <param name="serilaList"></param>
        private string MakeImageModeHtml(List<BitAuto.CarChannel.BLL.SerialInfo> serilaList)
        {
            int startIndex = (pageNum - 1) * pageSize + 1;
            int endIndex = startIndex + pageSize - 1;
            StringBuilder sbCarList = new StringBuilder();
            int counter = 0;
            foreach (BitAuto.CarChannel.BLL.SerialInfo info in serilaList)
            {
                counter++;
                CarNum += info.CarNum;
                if (counter < startIndex || counter > endIndex)
                    continue;
                string serialUrl = "/" + info.AllSpell + "/";
                string shortName = info.ShowName.Replace("(进口)", "");
                if (info.SerialId == 1568)
                {
                    shortName = "索纳塔八";
                }
                sbCarList.Append("<li>");
                sbCarList.AppendFormat("<a href=\"{0}\" class=\"car\"><div class=\"img-box\"><img src=\"{1}\" />", serialUrl, info.ImageUrl.Replace("_2.","_6."));
                if (info.IsAdvertise)   //设置“特价”标签
                    sbCarList.Append("<i class=\"sale\"></i>");
                sbCarList.AppendFormat("</div><strong>{0}</strong>", shortName);
                sbCarList.AppendFormat("<p>报价：<em>{0}</em></p>", info.PriceRange);
                sbCarList.Append("</a></li>");
            }
            return sbCarList.ToString();
        }

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}