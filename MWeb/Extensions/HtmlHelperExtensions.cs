using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BitAuto.CarChannel.Common;

namespace MWeb.Extensions
{
    public static class HtmlHelperExtensions
    {

        public static IHtmlString ServerSideInclude(this HtmlHelper helper, string serverPath)
        {
            var filePath = HttpContext.Current.Server.MapPath(serverPath);
            if (!File.Exists(filePath))
                return new HtmlString("");
            try
            {
                using (var streamReader = new StreamReader(filePath, Encoding.Default))
                {
                    var markup = streamReader.ReadToEnd();
                    return new HtmlString(markup);
                }
            }
            catch (Exception ex)
            {
                return new HtmlString("");
            }
        }

        public static IHtmlString ServerSideDoubleInclude(this HtmlHelper helper, string serverPath)
        {

            try
            {
                string content = CommonFunction.GetFileContent(serverPath);


                string pattern = @"<!--#include\s+?file=""(?<src>.+?)""\s*?-->";

                MatchCollection mc = Regex.Matches(content, pattern);

                foreach (Match m in mc)
                {
                    var subFilePath = m.Groups["src"].Value;
                    var subContent = CommonFunction.GetFileContent(subFilePath);
                    content = content.Replace(m.Value, subContent);
                }
                return new HtmlString(content);

            }
            catch (Exception ex)
            {
                return new HtmlString("");
            }
        }
    }
}