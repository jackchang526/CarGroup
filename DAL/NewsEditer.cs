using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.DAL
{
    public class NewsEditerDal
    {
        /// <summary>
        /// 得到编辑的列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<int,Editer> GetEditerList()
        {
            string url = WebConfig.NewsEditerMessageUrl;
            if (string.IsNullOrEmpty(url)) return null;

            DataSet ds = new DataSet();
            try
            {
                ds.ReadXml(url);
                if(ds == null || ds.Tables.Count < 1|| ds.Tables[0].Rows.Count < 1) return null;
                Dictionary<int, Editer> editerlist = new Dictionary<int, Editer>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int userId = ConvertHelper.GetInteger(dr.IsNull("UserId") ? "0" : dr["UserId"].ToString());
                    string userName = dr.IsNull("UserName") ? string.Empty : dr["UserName"].ToString();
                    string userLogin = dr.IsNull("UserLoginName") ? string.Empty : dr["UserLoginName"].ToString();
                    string userBlogUrl = dr.IsNull("UserBlogUrl") ? string.Empty : dr["UserBlogUrl"].ToString();

					int cityId = ConvertHelper.GetInteger(dr.IsNull("CityId") ? "0" : dr["CityId"].ToString());
					string cityName = dr.IsNull("CityName") ? string.Empty : dr["CityName"].ToString();

                    if (userId < 1) continue;
                    Editer editer = new Editer();
                    editer.UserId = userId;
                    editer.UserName = userName;
                    editer.UserBlogUrl = userBlogUrl;
                    editer.UserLoginName = userLogin;

					editer.CityId = cityId;
					editer.CityName = cityName;

                    if (!editerlist.ContainsKey(userId))
                        editerlist.Add(userId, editer);
                }
                return editerlist;
            }
            catch
            {
                return null;
            }
        }
    }
}
