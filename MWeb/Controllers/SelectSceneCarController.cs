using BitAuto.CarChannel.Common.Cache;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;
using System.Text;
using System.Web.Caching;
using BitAuto.CarChannel.Common;
using System.Web.UI;

namespace MWeb.Controllers
{
    public class SelectSceneCarController : Controller
    {
        //
        // GET: /SelectSceneCar/
        protected string _wordName = string.Empty;
        protected string _wordContent = string.Empty;
        protected Dictionary<string, string> dictCheckListParams = new Dictionary<string, string>();

        protected string allCheckBoxHtml = string.Empty;
        protected string _SearchUrl = string.Empty;
        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index()
        {
            string urlOfMainword = Request.QueryString["word"];
            ViewData["_SearchUrl"] = "/selectscenecar/" + "?word=" + urlOfMainword; //Request.Path+"?word="+urlOfMainword;
            InitMainWordDictItem(urlOfMainword);
            allCheckBoxHtml = getCommonChcekHtml(dictCheckListParams);
            ViewData["_wordName"] = _wordName;
            ViewData["_wordContent"] = _wordContent;
            ViewData["allCheckBoxHtml"] = allCheckBoxHtml;            
            ViewData["GenerateSearchInitScript"] =GenerateSearchInitScript();
            return View();
        }

        private void InitMainWordDictItem(string word)
        {
            string cachekeyOfSceneNode = "Wireless_SelectSceneCar_XmlNode_" + word;
            XmlNode cacheSceneNode = (XmlNode)CacheManager.GetCachedData(cachekeyOfSceneNode);
            if (cacheSceneNode == null)
            {
                string path=HttpContext.Server.MapPath("~/config/SceneCar.xml");
                //string path = HttpContext.Current.Server.MapPath("~/config/SceneCar.xml");
                if (System.IO.File.Exists(path))
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
						Response.Redirect("/error", true);
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
                string[] keyArrStr = key.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
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
        private void checkParamsHtml(StringBuilder htmlItems, Dictionary<string, string> dict, string key, string valueOfKey, string paramValue)
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
