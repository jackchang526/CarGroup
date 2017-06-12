using BitAuto.CarChannel.BLL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using System.IO;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;

namespace H5Web.V2
{
	public partial class Page2 : H5PageBase
	{
		#region Params
		protected int SerialBrandId;
		protected SerialEntity BaseSerialEntity = null;

		protected List<SerialPositionImagesEntity> SerialImageList = new List<SerialPositionImagesEntity>();

		protected SerialPositionImagesEntity TujieImageEntity;
		#endregion
		protected void Page_Load(object sender, EventArgs e)
		{
			GetParams();
			InitData();
			InitImageList();
		}

		private void GetParams()
		{
			if (!string.IsNullOrEmpty(Request["serialbrandid"]))
			{
				SerialBrandId = Convert.ToInt32(Request["serialbrandid"]);
			}
		}

		/// <summary>
		/// 初始化数据
		/// </summary>
		private void InitData()
		{
			BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialBrandId);
		}

		private void InitImageList()
		{
			int[] positionIds = { 6, 7, 8 };
			Dictionary<int, List<SerialPositionImagesEntity>> dic = GetImageDic();
			int index = 0;
			while (SerialImageList.Count < 3)
			{
				foreach (var positionId in positionIds)
				{
					if (!dic.ContainsKey(positionId))
						continue;
					var list = dic[positionId];
					if (list.Count > index)
					{
						SerialImageList.Add(list[index]);
					}
					index++;
				}
			}
			//初始化图解列表
			if (dic.ContainsKey(12))
				TujieImageEntity = dic[12][0];
		}
		protected string getTag(int positionId)
		{
			switch (positionId)
			{
				case 6:
					return "外观";
				case 7:
					return "内饰";
				case 8:
					return "空间";
				case 12:
					return "图解";
			}
			return string.Empty;
		}

		private Dictionary<int, List<SerialPositionImagesEntity>> GetImageDic()
		{
			string xmlPath = string.Format(Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialFourthStageImagePath),
				SerialBrandId);
			XDocument document = XDocument.Load(xmlPath);
			var imageEles = document.Descendants("Images");
			Dictionary<int, List<SerialPositionImagesEntity>> dic = new Dictionary<int, List<SerialPositionImagesEntity>>();
			foreach (var ele in imageEles)
			{
				int groupId = Convert.ToInt32(ele.Attribute("GroupId").Value);
				var imgEles = ele.Descendants("Image");
				List<SerialPositionImagesEntity> imageList = new List<SerialPositionImagesEntity>();
				foreach (var imgEle in imgEles)
				{
					imageList.Add(
						new SerialPositionImagesEntity()
						{
							ImageId = Convert.ToInt32(imgEle.Attribute("ImageID").Value),
							ImageName = imgEle.Attribute("ImageName").Value,
							ImageUrl = imgEle.Attribute("ImageUrl").Value,
							PositionId = groupId
						}
						);
				}
				if (imageList.Count > 0)
				{
					dic[groupId] = imageList;
				}

			}
			return dic;
		}
	}
}