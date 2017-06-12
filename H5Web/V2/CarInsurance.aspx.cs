using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace H5Web.V2
{
    public partial class CarInsurance : H5PageBase
    {
        //第四级
        private SerialFourthStageBll serialFourthStageBll = new SerialFourthStageBll();
        //子品牌id
        protected int serialId = 0;
        //子品牌实体
        protected SerialEntity BaseSerialEntity;
        //子品牌下车款的最低厂商指导价
        protected decimal carMinReferPrice = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);

            GetParamter();
            if (serialId < 1)
            {
                Response.Redirect("http://car.h5.yiche.com/");
            }
            InitData();
            //List<int> listAllCsID = serialFourthStageBll.GetAllSerialInH5();
            //if (listAllCsID == null || BaseSerialEntity == null)
            //{
            //    Response.Redirect("http://car.h5.yiche.com/");
            //}
            //if (!listAllCsID.Contains(serialId) && serialId > 0)
            //{
            //    Response.Redirect("http://car.m.yiche.com/" + BaseSerialEntity.AllSpell);
            //}
            
            GetReferPrice();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetParamter()
        {
            serialId = ConvertHelper.GetInteger(Request.QueryString["csID"]);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
        }

        /// <summary>
        /// 获得子品牌下车款的最低厂商指导价
        /// </summary>
        private void GetReferPrice()
        {
            DataSet ds = base.GetCarIDAndNameForCS(serialId, WebConfig.CachedDuration);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataView carView = ds.Tables[0].DefaultView;
                //LowerReferPriceBySerial = ds.Tables[0].AsEnumerable().Where(x => ConvertHelper.GetDecimal(x["car_ReferPrice"]) > 0).Select(x => x.Field<decimal>("car_ReferPrice")).Min();
                DataRow dr= ds.Tables[0].AsEnumerable().Where(x => ConvertHelper.GetDecimal(x["car_ReferPrice"]) > 0).OrderBy(x => ConvertHelper.GetDecimal(x["car_ReferPrice"])).FirstOrDefault();
                if (dr != null)
                {
                    carMinReferPrice = ConvertHelper.GetDecimal(dr["car_ReferPrice"]);
                }
            }
        }
    }
}