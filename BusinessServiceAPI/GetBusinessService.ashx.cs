using BitAuto.Utils;
using Newtonsoft.Json;
using Noah.Opportunity.Service;
using Noah.Opportunity.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessServiceAPI
{
	/// <summary>
	/// GetBusinessService 的摘要说明
	/// </summary>
	public class GetBusinessService : PageBaseForBusiness, IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;

		int serialId = 0;
		int carId = 0;
		int cityId = 0;
		string callback = string.Empty;
		public void ProcessRequest(HttpContext context)
		{
			base.SetPageCache(60);

			context.Response.ContentType = "application/x-javascript";
			response = context.Response;
			request = context.Request;

			string action = ConvertHelper.GetString(request.QueryString["action"]);

			GetParameters();

			switch (action.ToLower())
			{
				case "pcserial": GetPCOpportunityBySerial(); break;
				case "mserial": GetMOpportunityBySerial(); break;
				case "pccar": GetPCOpportunityByCar(); break;
				case "mcar": GetMOpportunityByCar(); break;
				default: break;
			}
		}

		private void GetParameters()
		{
			serialId = ConvertHelper.GetInteger(request.QueryString["serialid"]);
			carId = ConvertHelper.GetInteger(request.QueryString["carid"]);
			cityId = ConvertHelper.GetInteger(request.QueryString["cityid"]);
			callback = request.QueryString["callback"];
		}

		/// <summary>
		/// PC 车系 商机信息
		/// </summary>
		private void GetPCOpportunityBySerial()
		{
			ServiceClient sc = new ServiceClient();
			ButtonModel btnModel = sc.GetPcOpportunityBySerial(cityId, serialId);
 			RenderContent(btnModel);
		}
 	
		/// <summary>
		/// M站 车系 商机信息
		/// </summary>
		private void GetMOpportunityBySerial()
		{
			ServiceClient sc = new ServiceClient();
			ButtonModel btnModel = sc.GetMobileOpportunityBySerial(cityId, serialId);
			RenderContent(btnModel);
		}
		/// <summary>
		/// PC 车款 商机信系
		/// </summary>
		private void GetPCOpportunityByCar()
		{
			ServiceClient sc = new ServiceClient();
			ButtonModel btnModel = sc.GetPcOpportunityByCar(cityId, serialId, carId);
			RenderContent(btnModel);
		}
		/// <summary>
		/// M 车款 商机信系
		/// </summary>
		private void GetMOpportunityByCar()
		{
			ServiceClient sc = new ServiceClient();
			ButtonModel btnModel = sc.GetMobileOpportunityByCar(cityId, serialId, carId);
			RenderContent(btnModel);
		}

		private void RenderContent(ButtonModel btnModel)
		{
			string json = JsonConvert.SerializeObject(btnModel);

			if (string.IsNullOrEmpty(callback))
				response.Write(string.Format("{0}", json));
			else
				response.Write(string.Format("{1}({0})", json, callback));
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