using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace H5Web.xuanche
{
    public partial class SearchCarListV4 : H5PageBase
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
            var res= sb.ToString().Replace(",}", "}");
            return res;
        }
    }
}