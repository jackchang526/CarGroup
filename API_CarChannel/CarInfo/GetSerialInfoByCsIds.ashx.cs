using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// GetSerialInfoByCsIds 的摘要说明
    /// </summary>
    public class GetSerialInfoByCsIds : PageBase, IHttpHandler
    {
        /// <summary>
        /// 子品牌id
        /// </summary>
        private string serialIds = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        private string callback = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            GetParams(context);
            RendSerialInfo(context);
        }

        private void RendSerialInfo(HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(serialIds))
            {
                context.Response.Write(string.IsNullOrWhiteSpace(callback) ? "{}" : string.Format("{0}({})", callback));
            }
            else
            {
                string[] csIdArr = serialIds.Split(',');
                StringBuilder sb = new StringBuilder();
                // 白底图
                Dictionary<int, string> dicWhitePhoto = base.GetAllSerialPicURLWhiteBackground();
                foreach (string csId in csIdArr)
                {
                    int csIdInt = ConvertHelper.GetInteger(csId);
                    SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csIdInt);
                    if (serialEntity == null)
                    {
                        continue;
                    }
                    string saleState = string.Empty;
                    if (string.IsNullOrWhiteSpace(serialEntity.Price))
                    {
                        if (!string.IsNullOrEmpty(serialEntity.SaleState) && "待销" == serialEntity.SaleState)
                        {
                            saleState = "未上市";
                        }
                        else
                        {
                            saleState = "暂无报价";
                        }
                    }
                    else
                    {
                        saleState = serialEntity.Price;//StringHelper.SubString(serialToSerial.ToCsPriceRange.ToString(), 14, false);
                    }

                    sb.AppendFormat("\"{4}\":{{\"showName\":\"{0}\",\"price\":\"{1}\",\"allSpell\":\"{2}\",\"img\":\"{3}\"}},"
                        , serialEntity.ShowName
                        , saleState
                        , serialEntity.AllSpell
                        , dicWhitePhoto.ContainsKey(csIdInt) ? dicWhitePhoto[csIdInt].Replace("_2.", "_5.") : WebConfig.DefaultCarPic
                        , serialEntity.Id);
                }
                context.Response.Write(string.IsNullOrWhiteSpace(callback) ? string.Format("{{{0}}}", sb.ToString().TrimEnd(',')) : string.Format("{0}({{{1}}})", callback, sb.ToString().TrimEnd(',')));
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="context"></param>
        private void GetParams(HttpContext context)
        {
            serialIds = context.Request["csids"];
            callback = context.Request["callback"];
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