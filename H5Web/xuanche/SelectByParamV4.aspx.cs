using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;

namespace H5Web.xuanche
{
    public partial class SelectByParam : Page
    {
        protected string ParamJson = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            ParamJson = ConvertParaToJson();
        }

        private string ConvertParaToJson()
        {
            NameValueCollection nvcQuery = Request.QueryString;
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (var key in nvcQuery.AllKeys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    sb.AppendFormat("'{0}':'{1}',", key, nvcQuery[key]);
                }
            }
            sb.Append("}");
            var res = sb.ToString().Replace(",}", "}");
            return res;
        }
    }
}