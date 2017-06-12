using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.DAL.Data;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL.Data
{
	public class CarEntity:BaseEntity
	{
		protected int m_serialId;
		protected int m_carYear;
		protected int m_pvNum;
		protected WeakReference m_serialEntity;
		protected string m_produceState;
		protected string m_saleSate;
		protected string m_priceRange;
		protected double m_referPrice;
		protected Dictionary<string, string> m_paraNameDic;
		protected Dictionary<int, string> m_paraIdDic;


		public CarEntity()
		{
			m_paraIdDic = new Dictionary<int, string>();
			m_paraNameDic = new Dictionary<string, string>();
		}

		/// <summary>
		/// 根据行数据初始化车型信息
		/// </summary>
		/// <param name="carRow"></param>
		public CarEntity(DataRow carRow):this()
		{
			InitCarData(carRow);
		}


		/// <summary>
		/// 所属子品牌ID
		/// </summary>
		public int SerialId
		{
			get { return m_serialId; }
		}

		/// <summary>
		/// 所属子品牌
		/// </summary>
		public SerialEntity Serial
		{
			get
			{
				SerialEntity serial = null;
				if (m_serialEntity != null)
					serial = m_serialEntity.Target as SerialEntity;
				if (serial == null)
				{
					serial = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, m_serialId);
					if (m_serialEntity == null)
						m_serialEntity = new WeakReference(serial);
					else
						m_serialEntity.Target = serial;
				}
				return serial;
			}
		}

		/// <summary>
		/// 车型年款
		/// </summary>
		public int CarYear
		{
			get { return m_carYear; }
		}

		/// <summary>
		/// 生产状态
		/// </summary>
		public string ProduceState
		{
			get { return m_produceState; }
		}

		/// <summary>
		/// 销售状态
		/// </summary>
		public string SaleState
		{
			get { return m_saleSate; }
		}

		/// <summary>
		/// 车型指导价
		/// </summary>
		public double ReferPrice
		{
			get { return m_referPrice; }
		}

		/// <summary>
		/// 车型向前两天当天的PV值
		/// </summary>
		public int CarPV
		{
			get { return m_pvNum; }
		}

		/// <summary>
		/// 按参数名称取参数
		/// </summary>
		/// <param name="paraName"></param>
		/// <returns></returns>
		public string this[string aliasName]
		{
			get
			{
				if (m_paraNameDic.ContainsKey(aliasName))
					return m_paraNameDic[aliasName];
				else
					return GetParameterValue(aliasName);
			}
		}

		/// <summary>
		/// 按参数ID取参数
		/// </summary>
		/// <param name="paraId"></param>
		/// <returns></returns>
		public string this[int paraId]
		{
			get
			{
				if (m_paraIdDic.ContainsKey(paraId))
					return m_paraIdDic[paraId];
				else
					return GetParameterValue(paraId);
			}
		}

		/// <summary>
		/// 车型的报价区间
		/// </summary>
		public string PriceRange
		{
			get
			{
				if(m_priceRange == null)
				{
					if (m_saleSate == "停销")
						m_priceRange = "";
					else
						m_priceRange = new PageBase().GetCarPriceRangeByID(m_id);
				}
				return m_priceRange;
			}
		}

		/// <summary>
		/// 初始化车型数据
		/// </summary>
		/// <param name="id"></param>
		public override void InitData(int id)
		{
			DataSet ds = new TCarDAL().GetCarDataById(id);
			if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow carRow = ds.Tables[0].Rows[0];
				InitCarData(carRow);
			}
		}

		/// <summary>
		/// 使用一行数据初始化车型信息
		/// </summary>
		/// <param name="row"></param>
		private void InitCarData(DataRow carRow)
		{
			m_id = ConvertHelper.GetInteger(carRow["Car_Id"]);
			m_name = carRow["Car_Name"].ToString().Trim();
			m_isState = ConvertHelper.GetInteger(carRow["IsState"]);
			m_produceState = carRow["Car_ProduceState"].ToString().Trim();
			m_saleSate = carRow["Car_SaleState"].ToString().Trim();
			m_referPrice = ConvertHelper.GetDouble(carRow["car_ReferPrice"]);
			m_serialId = ConvertHelper.GetInteger(carRow["Cs_Id"]);
			m_carYear = ConvertHelper.GetInteger(carRow["Car_YearType"]);
			m_pvNum = ConvertHelper.GetInteger(carRow["Pv_SumNum"]);
		}

		/// <summary>
		/// 加载车型的全部参数
		/// </summary>
		public void InitParameterValue()
		{
			InitParameterValue("");
		}

		/// <summary>
		/// 按照分类加载车型参数
		/// </summary>
		/// <param name="paraClass"></param>
		public void InitParameterValue(string paraClass)
		{
			DataSet ds = new TCarDAL().GetCarParameter(m_id, paraClass);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow row = ds.Tables[0].Rows[0];
				int paraId = ConvertHelper.GetInteger(row["ParamId"]);
				string aliasName = row["AliasName"].ToString().Trim();
				string paraValue = row["Pvalue"].ToString().Trim();

				//加入参数值字典
				m_paraNameDic[aliasName] = paraValue;
				m_paraIdDic[paraId] = paraValue;
			}
		}


		/// <summary>
		/// 加载参数
		/// </summary>
		/// <param name="enName"></param>
		/// <returns></returns>
		private string GetParameterValue(string aliasName)
		{
			string paraValue = String.Empty;
			DataSet ds = new TCarDAL().GetCarParameter(m_id, aliasName);
			if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow row = ds.Tables[0].Rows[0];
				int paraId = ConvertHelper.GetInteger(row["ParamId"]);
				paraValue = row["Pvalue"].ToString().Trim();

				//加入参数值字典
				m_paraIdDic[paraId] = paraValue;
			}
			m_paraNameDic[aliasName] = paraValue;
			return paraValue;
		}

		/// <summary>
		/// 加载参数
		/// </summary>
		/// <param name="enName"></param>
		/// <returns></returns>
		private string GetParameterValue(int paraId)
		{
			string paraValue = String.Empty;
			DataSet ds = new TCarDAL().GetCarParameter(m_id, paraId);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow row = ds.Tables[0].Rows[0];
				string aliasName = row["AliasName"].ToString().Trim();
				paraValue = row["Pvalue"].ToString().Trim();

				//加入参数值字典
				m_paraNameDic[aliasName] = paraValue;
			}
			m_paraIdDic[paraId] = paraValue;
			return paraValue;
		}


		/// <summary>
		/// 根据获取数据字段名称取数据
		/// </summary>
		/// <param name="dataName"></param>
		/// <returns></returns>
		public override string GetDataValue(EntityType eType, string dataName )
		{
			string dataValue = String.Empty;
			if (eType != EntityType.Car)
				dataValue = this.Serial.GetDataValue(eType, dataName);
			else
			{
				dataValue = base.GetDataValue(dataName);
				if (dataValue.Length == 0)
				{
					switch (dataName)
					{
						case "CarYear":
							dataValue = m_carYear.ToString();
							break;
						case "PVNum":
							dataValue = m_pvNum.ToString();
							break;
						case "SaleState":
							dataValue = m_saleSate;
							break;
					}
				}
			}
			return dataValue;
		}
	}
}
