using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace WirelessWeb.handlers
{
    /// <summary>
    /// GetMasterNews 的摘要说明
    /// </summary>
    public class GetMasterNews : IHttpHandler
    {
        private int _mabId;
		private string _callBack;
        private int _pageIndex;
		private int _newsCount;
        private int _pageSize;
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.CarChannel.Common.Cache.CacheManager.SetPageCache(30);
            context.Response.ContentType = "application/x-javascript";
            GetParam(context.Request);
            if (_mabId < 1 || string.IsNullOrEmpty(_callBack))
                return;
            context.Response.Write(_callBack + "([");
            context.Response.Write(GetJson());
            context.Response.Write(string.Format("],{0}, {1});", _newsCount.ToString(), CommonFunction.GetTotalPageNumber(_newsCount, _pageSize)));

        }

        private void GetParam(HttpRequest request)
        {
            _mabId = ConvertHelper.GetInteger(request.QueryString["mabId"]);
            if (_mabId < 0)
                return;
            if (!string.IsNullOrEmpty(request.QueryString["call"]))
            {
                _callBack = request.QueryString["call"].Trim();
            }
            if (string.IsNullOrEmpty(_callBack))
                return;
            _pageIndex = ConvertHelper.GetInteger(request.QueryString["i"]);
            if (_pageIndex < 1)
                _pageIndex = 1;
            _pageSize = ConvertHelper.GetInteger(request.QueryString["size"]);
            if (_pageSize < 1)
                _pageSize = 10;
        }

        private string GetJson()
        {
            List<int> carTypeIdList = new List<int>()
                {
                    (int)CarNewsType.daogou,
                    (int)CarNewsType.yongche,
                    (int)CarNewsType.xinwen
                    };
            DataSet ds = new CarNewsBll().GetMasterBrandNews(_mabId,
                        carTypeIdList, _pageSize, _pageIndex, ref _newsCount);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return string.Empty;
            StringBuilder json = new StringBuilder();
            DataRowCollection rows = ds.Tables[0].Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i];
                string picUrl = ConvertHelper.GetString(row["Picture"]);
                string imageUrl = (!string.IsNullOrEmpty(picUrl) && picUrl.IndexOf("/not") < 0) ? picUrl : ConvertHelper.GetString(row["FirstPicUrl"]);
                string filePath = row["filepath"].ToString();
                string title = CommonFunction.NewsTitleDecode(row["title"].ToString());
                DateTime publishTime = Convert.ToDateTime(row["publishtime"]);
                string editorName = row["EditorName"].ToString();
                int commentNum = ConvertHelper.GetInteger(row["CommentNum"]);

                json.Append("{");
                json.AppendFormat("imageUrl:\"{0}\",filePath:\"{1}\",title:\"{2}\",publishTime:\"{3}\",editorName:\"{4}\",commentNum:\"{5}\""
                    , imageUrl
                    , filePath
                    , HttpUtility.UrlEncode(title)
                    , publishTime.ToString("yyyy-MM-dd")
                    , editorName
                    , commentNum
                    );
                json.Append("}");
                if (i < rows.Count - 1)
                    json.Append(",");
            }
            return json.ToString();
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