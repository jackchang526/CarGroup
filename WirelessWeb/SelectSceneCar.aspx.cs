using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using System.Xml;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using BitAuto.CarChannel.Common.Cache;
using System.IO;
using System.Web.Caching;
using System.Xml.Linq;

namespace WirelessWeb
{
	public partial class SelectSceneCar:WirelessPageBase
	{
        protected string _wordName = string.Empty;
        protected string _wordContent = string.Empty;
        protected Dictionary<string, string> dictCheckListParams = new Dictionary<string, string>();

        protected string allCheckBoxHtml = string.Empty;
        protected string _SearchUrl = string.Empty;
		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30);
            string urlOfMainword = Request.QueryString["word"];
            _SearchUrl ="/selectscenecar/" +"?word=" + urlOfMainword; //Request.Path+"?word="+urlOfMainword;
            InitMainWordDictItem(urlOfMainword);
            allCheckBoxHtml = getCommonChcekHtml(dictCheckListParams);
		}
        /// <summary>
        /// 验证 字符串参数有效性 1.3-1.6 、200、200_300
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private void InitMainWordDictItem(string word)
        {
            string cachekeyOfSceneNode = "Wireless_SelectSceneCar_XmlNode_"+word;
            XmlNode cacheSceneNode = (XmlNode)CacheManager.GetCachedData(cachekeyOfSceneNode);
            if (cacheSceneNode == null)
            {
                string path = HttpContext.Current.Server.MapPath("~/config/SceneCar.xml");
                if (File.Exists(path))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(path);
                    XmlNode curSceneNode = xmlDoc.SelectSingleNode("SceneList/Scene[@Id='" + word + "']");
                    if (curSceneNode != null)
                    {
                        setCommonWithNode(curSceneNode);
                    }
                    else
                    {
                        Response.Redirect("/404error.aspx");
                        return;
                    }
                    CacheManager.InsertCache(cachekeyOfSceneNode, curSceneNode, new CacheDependency(path), DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            else
            {
                setCommonWithNode(cacheSceneNode);
            }
        }
        private void setCommonWithNode(XmlNode curSceneNode)
        {
            XmlNode subNameNode = curSceneNode.SelectSingleNode("Name");
            XmlNode subContentNode = curSceneNode.SelectSingleNode("Content");
            XmlNode subParamsListNode = curSceneNode.SelectSingleNode("ParamsList");
            if (subNameNode != null)
            {
                _wordName = subNameNode.InnerText;
            }
            if (subContentNode != null)
            {
                _wordContent = subContentNode.InnerText;
            }
            if (subParamsListNode != null)
            {
                XmlNodeList xnlParamList = subParamsListNode.SelectNodes("Param");
                foreach (XmlNode xn in xnlParamList)
                {
                    string key = xn.Attributes["key"].Value;
                    string value = xn.Attributes["value"].Value;
                    dictCheckListParams.Add(key, value);
                }
            }
        }
        private string getCommonChcekHtml(Dictionary<string, string> dict)
        {
            StringBuilder htmlItems = new StringBuilder();
            htmlItems.Append("<ul>");

            string l = Request.QueryString["l"];
            string d = Request.QueryString["d"];
            string g = Request.QueryString["g"];
            string c = Request.QueryString["c"];
            string t = Request.QueryString["t"];
            string dt = Request.QueryString["dt"];
            string f = Request.QueryString["f"];
            string b = Request.QueryString["b"];
            string lv = Request.QueryString["lv"];
            string more = Request.QueryString["more"];
            foreach (string key in dict.Keys)     //key: more=266,l=7,l=8  , more=204_205_206_278  dt=4,8  f=7
            {
                string[] keyArrStr=key.Split(new string[]{"="},StringSplitOptions.RemoveEmptyEntries);
                string nameOfKey = keyArrStr[0];
                string valueOfKey = keyArrStr[1];
                switch (nameOfKey)
                {
                    case "l": checkParamsHtml(htmlItems, dict, key, valueOfKey, l); break;
                    case "d": checkParamsHtml(htmlItems, dict, key, valueOfKey, d); break;
                    case "g": checkParamsHtml(htmlItems, dict, key, valueOfKey, g); break;
                    case "c": checkParamsHtml(htmlItems, dict, key, valueOfKey, c); break;
                    case "t": checkParamsHtml(htmlItems, dict, key, valueOfKey, t); break;
                    case "dt": checkParamsHtml(htmlItems, dict, key, valueOfKey, dt); break;
                    case "f": checkParamsHtml(htmlItems, dict, key, valueOfKey, f); break;
                    case "b": checkParamsHtml(htmlItems, dict, key, valueOfKey, b); break;
                    case "lv": checkParamsHtml(htmlItems, dict, key, valueOfKey, lv); break;
                    case "more": checkParamsHtml(htmlItems, dict, key, valueOfKey, more); break;
                    default: break;
                }
            }
        
            htmlItems.Append("</ul>");
            return htmlItems.ToString();
        }
        private void checkParamsHtml(StringBuilder htmlItems,Dictionary<string,string> dict,string key,string valueOfKey,string paramValue)
        {
            if (!string.IsNullOrEmpty(paramValue))
            {
                if (paramValue.Contains(valueOfKey))
                {
                    htmlItems.Append(string.Format("<li data-param=\"{0}\"><div class=\"radio-box\"><label><div class=\"radio-normal checked\"><input type=\"checkbox\"></div><span>{1}</span></label></div></li>",
                                                            key, dict[key]));
                }
                else
                {
                    htmlItems.Append(string.Format("<li data-param=\"{0}\"><div class=\"radio-box\"><label><div class=\"radio-normal\"><input type=\"checkbox\"></div><span>{1}</span></label></div></li>",
                                 key, dict[key]));
                }
            }
            else
            {
                htmlItems.Append(string.Format("<li data-param=\"{0}\"><div class=\"radio-box\"><label><div class=\"radio-normal\"><input type=\"checkbox\"></div><span>{1}</span></label></div></li>",
                              key, dict[key]));
            }
        }

        protected string GenerateSearchInitScript()
        {
            StringBuilder scriptCode = new StringBuilder();
            string tmpStr = Request.QueryString["p"];
            if (!String.IsNullOrEmpty(tmpStr))
                scriptCode.AppendLine("SelectSceneCarTool.Price='" + tmpStr + "';");
            tmpStr = Request.QueryString["l"];
            if (!String.IsNullOrEmpty(tmpStr))
                scriptCode.AppendLine("SelectSceneCarTool.Level='" + tmpStr + "';");
            tmpStr = Request.QueryString["d"];
            if (!String.IsNullOrEmpty(tmpStr))
                scriptCode.AppendLine("SelectSceneCarTool.Displacement='" + tmpStr + "';");
            tmpStr = Request.QueryString["g"];
            if (!String.IsNullOrEmpty(tmpStr))
                scriptCode.AppendLine("SelectSceneCarTool.Brand='" + tmpStr + "';");
            tmpStr = Request.QueryString["c"];
            if (!String.IsNullOrEmpty(tmpStr))
                scriptCode.AppendLine("SelectSceneCarTool.Country='" + tmpStr + "';");
            tmpStr = Request.QueryString["t"];
            if (!String.IsNullOrEmpty(tmpStr))
                scriptCode.AppendLine("SelectSceneCarTool.TransmissionType='" + tmpStr + "';");
            tmpStr = Request.QueryString["dt"];
            if (!String.IsNullOrEmpty(tmpStr))
                scriptCode.AppendLine("SelectSceneCarTool.DriveType='" + tmpStr + "';");
            tmpStr = Request.QueryString["f"];
            if (!String.IsNullOrEmpty(tmpStr))
                scriptCode.AppendLine("SelectSceneCarTool.Fuel='" + tmpStr + "';");
            tmpStr = Request.QueryString["b"];
            if (!String.IsNullOrEmpty(tmpStr))
                scriptCode.AppendLine("SelectSceneCarTool.Body='" + tmpStr + "';");
            tmpStr = Request.QueryString["lv"];
            if (!String.IsNullOrEmpty(tmpStr))
                scriptCode.AppendLine("SelectSceneCarTool.Wagon='" + tmpStr + "';");
            tmpStr = Request.QueryString["more"];
            if (!String.IsNullOrEmpty(tmpStr))
                scriptCode.AppendLine("SelectSceneCarTool.SetMoreCondition('" + tmpStr + "');");
           
            return scriptCode.ToString();
        }

	}
}