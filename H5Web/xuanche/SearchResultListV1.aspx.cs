using System;
using System.Web.UI;
using Newtonsoft.Json;

namespace H5Web.xuanche
{
    public partial class SearchResultListV1 : Page
    {
        protected int CurrentPage = 1;
        protected string Keyword = string.Empty;
        protected int PageSize = 10;
        protected string Para = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Keyword = Request.QueryString["keyword"];

            if (Request.QueryString["page"] != null)

                int.TryParse(Request.QueryString["page"], out CurrentPage);

            var nvcQuery = Request.QueryString;

            Para = JsonConvert.SerializeObject(nvcQuery);
        }
    }
}