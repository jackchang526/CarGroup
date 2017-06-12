using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Web;
using System.Net;
using System.Security.Cryptography;

using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.Common
{
    public class CommonFunction
    {
        private int tickNum = 0;
        public string szConnString = WebConfig.DefaultConnectionString;
        public static string[] CharList = new string[]{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q",
			"R","S","T","U","V","W","X","Y","Z"};

        public CommonFunction()
        { }

        #region
        public string GetCarInfoForCompare(int carid)
        {
            com.bitauto.carser.Service se = new BitAuto.CarChannel.Common.com.bitauto.carser.Service();
            return se.GetCarParameterConfigurationXMLContent(carid);
        }
        #endregion

        /// <summary>
        /// �������������õõ�ͼƬ������Url
        /// </summary>
        /// <param name="absolut">ͼƬ���·��</param>
        /// <returns>ͼƬ����URI</returns>
        private string GetImageUrl(string relativePath)
        {
            string[] imageDomains = WebConfig.ImageDomain.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            if (tickNum == imageDomains.Length)
            {
                tickNum = 0;
            }

            string imageUrl = string.Empty;

            if (relativePath != "")
            {
                imageUrl = string.Format("http://{0}/{1}", imageDomains[tickNum], relativePath);
            }

            tickNum++;

            return imageUrl;
        }

        /// <summary>
        /// �������ͻ�ȡ������·��
        /// </summary>
        /// <param name="publishType">��������</param>
        /// <param name="imgUrl">ͼƬ·��</param>
        /// <returns></returns>
        public string GetPublishImage(int publishType, string imgUrl, int imgId)
        {
            if (imgUrl.Trim().LastIndexOf(".") < 0)
            {
                string tempDefaultCarPic = WebConfig.DefaultCarPic;
                return tempDefaultCarPic.Insert(tempDefaultCarPic.LastIndexOf("."), "_" + publishType.ToString());
            }
            else
            {
                string sourceImgPath = imgUrl;
                sourceImgPath = sourceImgPath.Insert(sourceImgPath.LastIndexOf("."), "_" + imgId.ToString());
                sourceImgPath = sourceImgPath.Insert(sourceImgPath.LastIndexOf("."), "_" + publishType.ToString());
                return GetImageUrl(sourceImgPath);
            }
        }

        /// <summary>
        /// ��ͼƬ·��������ɢ�к�ʹ��
        /// </summary>
        /// <param name="publishType"></param>
        /// <param name="imgUrl"></param>
        /// <param name="imgId"></param>
        /// <returns></returns>
        public static string GetPublishHashImgUrl(int publishType, string imgUrl, int imgId)
        {
            if (imgUrl.Trim().LastIndexOf(".") < 0)
            {
                imgUrl = WebConfig.DefaultCarPic;
                imgUrl = imgUrl.Insert(imgUrl.LastIndexOf("."), "_" + publishType.ToString());
            }
            else
            {
                string domainName = "http://img" + (imgId % 4 + 1).ToString() + ".bitautoimg.com/autoalbum/";
                imgUrl = domainName + imgUrl;
                imgUrl = imgUrl.Insert(imgUrl.LastIndexOf("."), "_" + imgId.ToString());
                imgUrl = imgUrl.Insert(imgUrl.LastIndexOf("."), "_" + publishType.ToString());
            }
            return imgUrl;
        }
        /// <summary>
        /// �õ�Hash���ͼƬ����
        /// </summary>
        /// <param name="imgId"></param>
        /// <returns></returns>
        public static string GetPublishHashImageDomain(int imgId)
        {
            if (imgId < 1) return "";
            string domainName = "http://img" + (imgId % 4 + 1).ToString() + ".bitautoimg.com/autoalbum/";
            return domainName;
        }

        /// <summary>
        /// ��ȡָ���������������ݲ�����
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="nLength"></param>
        /// <returns></returns>
        public string GetShortString(string strSource, int nLength)
        {
            if (string.IsNullOrEmpty(strSource))
            {
                return strSource;
            }

            if (strSource.Length > nLength)
            {
                strSource = strSource.Substring(0, nLength) + "...";
            }

            return strSource;
        }

        /// <summary>
        /// ��ȡָ���������������ݲ�����
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public string GetShortString(string strSource)
        {
            return GetShortString(strSource, 150);
        }

        /// <summary>
        /// ����������������еı��ת���ɶ�Ӧ��html��ʽ���
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public string ToHtml(string strSource)
        {
            if (strSource.Length == 0)
            {
                return strSource;
            }

            strSource = strSource.Replace("&", "&amp;");
            strSource = strSource.Replace("<", "&lt;");
            strSource = strSource.Replace(">", "&gt;");
            strSource = strSource.Replace("\r\n", "<br>");
            strSource = strSource.Replace("\n", "<br>");
            strSource = strSource.Replace(" ", "&nbsp;");

            return (strSource);
        }
        /// <summary>
        /// �����������еı��ת���ɶ�Ӧ��xml��ʽ���
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public string ToXml(string strSource)
        {
            if (string.IsNullOrEmpty(strSource)) return strSource;
            strSource = strSource.Replace("&amp;", "&");
            strSource = strSource.Replace("&lt;", "<");
            strSource = strSource.Replace("&gt;", ">");
            strSource = strSource.Replace("&apos;", "'");
            strSource = strSource.Replace("&quot;", "\"");
            strSource = strSource.Replace("&mdash;", "-");

            return (strSource);

        }

        /// <summary>
        /// ȥ�������а�����html���
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public string DropHTML(string strHtml)
        {
            string[] aryReg ={
              @"<script[^>]*?>.*?</script>",

              @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""''])(\\[""''tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
              @"([\r\n])[\s]+",
              @"&(quot|#34);",
              @"&(amp|#38);",
              @"&(lt|#60);",
              @"&(gt|#62);", 
              @"&(nbsp|#160);", 
              @"&(iexcl|#161);",
              @"&(cent|#162);",
              @"&(pound|#163);",
              @"&(copy|#169);",
              @"&#(\d+);",
              @"-->",
              @"<!--.*\n"

             };

            string[] aryRep = {
               "",
               "",
               "",
               "\"",
               "&",
               "<",
               ">",
               " ",
               "\xa1",//chr(161),
               "\xa2",//chr(162),
               "\xa3",//chr(163),
               "\xa9",//chr(169),
               "",
               "\r\n",
               ""
              };

            string newReg = aryReg[0];
            string strOutput = strHtml;

            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");

            return strOutput;
        }

        /// <summary>
        /// ��ȡxml�ļ� 
        /// </summary>
        /// <param name="xmlUrl">xml��ַ</param>
        /// <returns></returns>
        public static XmlDocument ReadXml(string xmlUrl)
        {
            XmlDocument xmlDoc = new XmlDocument();
            int readTimes = 0;		//���ļ�����

            while (true)
            {
                readTimes++;
                try
                {
                    xmlDoc.Load(xmlUrl);
                    break;
                }
                catch (Exception ex)
                {
                    //modified by sk 2013.04.26 �����쳣��־��¼
                    if (readTimes == 2)
                        WriteLog("��ȡ�ļ�ʧ��2�Σ�·����" + xmlUrl);
                    //�ȴ�һ��
                    Thread.Sleep(500);
                }

                //���ʧ�ܣ��ظ�������
                if (readTimes == 2)
                    break;
            }
            return xmlDoc;
        }

        /// <summary>
        /// ��Xml�ļ��������ȡʧ�ܣ��ض�һ��
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <returns></returns>
        public static XmlDocument ReadXmlFromFile(string xmlFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(xmlFile))
            {
                int readTimes = 0;		//���ļ�����
                while (true)
                {
                    readTimes++;
                    FileStream fs = null;
                    try
                    {
                        fs = new FileStream(xmlFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        xmlDoc.Load(fs);
                        // xmlDoc.Load(xmlFile);
                        break;
                    }
                    catch (Exception ex)
                    {
                        //modified by sk 2013.04.26 �����쳣��־��¼
                        if (readTimes == 2)
                        {
                            xmlDoc = GetMemcacheByFile(xmlFile);
                            if (!xmlDoc.HasChildNodes)
                            {
                                WriteLog(string.Format("��ȡ�ļ�ʧ��2����memcache����ԴҲΪ��({0})��·����{1}", ex.Message, xmlFile));
                            }
                            break;
                        }
                        //�ȴ�һ��
                        Thread.Sleep(500);
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                        }
                    }
                    //���ʧ�ܣ��ظ�������
                    if (readTimes == 2)
                        break;
                }
            }
            return xmlDoc;
        }
        /// <summary>
        /// ��Xml�ļ��������ȡʧ�ܣ��ض�һ��
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <returns></returns>
        public static XmlReader ReadXmlReaderFromFile(string xmlFile)
        {
            XmlReader xmlReader = null;
            if (File.Exists(xmlFile))
            {
                int readTimes = 0;		//���ļ�����
                while (true)
                {
                    readTimes++;
                    try
                    {
                        xmlReader = XmlReader.Create(xmlFile);
                        break;
                    }
                    catch (Exception ex)
                    {
                        //modified by sk 2013.04.26 �����쳣��־��¼
                        if (readTimes == 2)
                        {
                            xmlReader = GetXmlReaderFromMemcacheByFile(xmlFile);
                            if (xmlReader == null)
                            {
                                WriteLog(string.Format("��ȡ�ļ�ʧ��2����memcache����ԴҲΪ��({0})��·����{1}", ex.Message, xmlFile));
                            }
                            break;
                        }
                        //�ȴ�һ��
                        Thread.Sleep(500);
                    }
                    //���ʧ�ܣ��ظ�������
                    if (readTimes == 2)
                        break;
                }
            }
            return xmlReader;
        }
        /// <summary>
        /// �����ļ���ȡmemcache����
        /// </summary>
        /// <param name="xmlFile">xml�ļ�·��</param>
        /// <returns></returns>
        public static XmlReader GetXmlReaderFromMemcacheByFile(string xmlFile)
        {
            XmlReader xmlReader = null;
            try
            {
                string memCacheKey = BitAuto.CarChannel.Common.Config.FileToMemcacheConfig.GetValue(xmlFile);
                if (string.IsNullOrEmpty(memCacheKey))
                    return xmlReader;
                string xmlStr = (string)MemCache.GetMemCacheByKey(memCacheKey);
                if (!string.IsNullOrEmpty(xmlStr))
                {
                    xmlReader = XmlReader.Create(new StringReader(xmlStr));
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
            return xmlReader;
        }
        /// <summary>
        /// �����ļ���ȡmemcache����
        /// </summary>
        /// <param name="xmlFile">xml�ļ�·��</param>
        /// <returns></returns>
        public static XmlDocument GetMemcacheByFile(string xmlFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                string memCacheKey = BitAuto.CarChannel.Common.Config.FileToMemcacheConfig.GetValue(xmlFile);
                if (!string.IsNullOrEmpty(memCacheKey))
                {
                    string xmlStr = (string)MemCache.GetMemCacheByKey(memCacheKey);
                    if (!string.IsNullOrEmpty(xmlStr))
                    {
                        xmlDoc.LoadXml(xmlStr);
                    }
                    else
                    {
                        WriteLog("Memcache��keyֵΪ��,Key��" + memCacheKey);
                    }
                }
                else
                {
                    WriteLog("δ�ҵ��ļ���ӦMemcache��key,�ļ�·����" + xmlFile);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
            return xmlDoc;
        }
        /// <summary>
        /// ��ȡ�ļ�����
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileContent(string filePath)
        {
            string content = string.Empty;
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
                    {
                        content = sr.ReadToEnd();
                    } 
                }
            }
            catch (Exception ex)
            {
                WriteLog("��ȡ�쳣URL��" + filePath + ex.ToString());
            } 
            return content;
        }

        /// <summary>
        /// ���ַ�������Html����
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public static string HtmlEncode(string htmlStr)
        {
            htmlStr = HttpUtility.HtmlEncode(htmlStr);
            htmlStr = htmlStr.Replace("\r", "").Replace("\n", "<br>").Replace(" ", "&nbsp;");

            return htmlStr;
        }

        /// <summary>
        /// ������ĸΪ���ֵ�תΪ��ĸ
        /// </summary>
        /// <param name="firstChar"></param>
        /// <returns></returns>
        public static string ConvertNumToChar(string firstChar)
        {
            string fChar = firstChar;

            switch (firstChar)
            {
                case "1":
                    fChar = "Y";
                    break;
                case "2":
                    fChar = "E";
                    break;
                case "3":
                case "4":
                    fChar = "S";
                    break;
                case "5":
                    fChar = "W";
                    break;
                case "0":
                case "6":
                    fChar = "L";
                    break;
                case "7":
                    fChar = "Q";
                    break;
                case "8":
                    fChar = "B";
                    break;
                case "9":
                    fChar = "J";
                    break;
            }

            return fChar;
        }

        /// <summary>
        /// ������ĸ����
        /// </summary>
        /// <param name="charDic"></param>
        /// <returns></returns>
        public static string RenderCharNav<T>(Dictionary<string, T> charDic)
        {

            StringBuilder charNav = new StringBuilder();
            charNav.AppendLine("<div class=\"car_top_tit a2zletters\">\r\n<ul>");

            foreach (string firstChar in CharList)
            {
                if (charDic.ContainsKey(firstChar))
                {
                    charNav.AppendLine("<li><a href=\"#" + firstChar + "\">" + firstChar + "</a></li>");
                }
                else
                {
                    charNav.AppendLine("<li class=\"nolink\">" + firstChar + "</li>");
                }
            }

            charNav.AppendLine("</ul>\r\n</div>");

            return charNav.ToString();
        }

        /// <summary>
        /// ������ĸ����
        /// </summary>
        /// <param name="charDic"></param>
        /// <returns></returns>
        public static string RenderCharNav<T>(Dictionary<string, T> charDic, string id, bool IsShow, string preChar)
        {
            string displayDefine = IsShow ? "block" : "none";
            List<string> charNavHtmlList = new List<string>();
            charNavHtmlList.Add("<div id=\"" + id + "\" class=\"car_top_tit a2zletters\" style=\"display:" + displayDefine + "\"><ul>");

            foreach (string firstChar in CharList)
            {
                if (charDic.ContainsKey(firstChar))
                {
                    charNavHtmlList.Add("<li><a href=\"#" + preChar + firstChar + "\">" + firstChar + "</a></li>");
                }
                else
                {
                    charNavHtmlList.Add("<li class=\"nolink\">" + firstChar + "</li>");
                }
            }

            charNavHtmlList.Add("</ul></div>");

            return String.Concat(charNavHtmlList.ToArray());
        }

        /// <summary>
        /// ������ĸ����(��ҳ)
        /// </summary>
        /// <param name="charDic"></param>
        /// <returns></returns>
        public static string RenderCharNavForDefaultPage<T>(Dictionary<string, T> charDic)
        {
            var charNav = new StringBuilder();
            charNav.Append("<div style=\"width:100%; height: 45px; overflow:hidden;\">");
            charNav.Append("<div id=\"theid\">");
             charNav.AppendLine("<ul class=\"list list-gapline sm a-z\">");
            foreach (string firstChar in CharList)
            {
                if (charDic.ContainsKey(firstChar))
                {
                    //charNav.AppendLine("<li class=\" \"><a href=\"#" + firstChar + "\">" + firstChar + "<span>|</span></a></li>");
                    var liHtml =
                        string.Format("<li class=\" \"><a href=\"#{0}\">{1}</a></li>", firstChar, firstChar);
                    charNav.AppendLine(liHtml);
                }
                else
                {
                    charNav.AppendLine(string.Format("<li class=\" \">{0}</li>", firstChar));
                }
            }
            charNav.AppendLine("</ul>");
            charNav.AppendLine("</div></div>");
            return charNav.ToString();
        }

        /// <summary>
        /// ���ɰ�����ID���ɲ�ѯ��Xpath
        /// </summary>
        /// <param name="cateIds"></param>
        /// <returns></returns>
        public static string GetCategoryXmlPath(int[] cateIds)
        {
            string xmlPath = string.Empty;

            foreach (int cateId in cateIds)
            {
                if (xmlPath.Length == 0)
                    xmlPath = "contains(CategoryPath,\"," + cateId + ",\")";
                else
                    xmlPath += "or contains(CategoryPath,\"," + cateId + ",\")";
            }
            return xmlPath;
        }
        /// <summary>
        /// ͨ��Url��ַ�õ�������Ϣ
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <returns>�����ַ���:"":��ʾ�����쳣��δȡ������</returns>
        public static string GetContentByUrl(string url)
        {
            string returnString = string.Empty;

            if (url == null || url == "")
            {
                return returnString;
            }

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (Stream receiveStream = httpWebResponse.GetResponseStream())
                {
                    Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    using (StreamReader readStream = new StreamReader(receiveStream, encode))
                    {
                        StringBuilder tempStr = new StringBuilder();
                        while (!readStream.EndOfStream)
                        {
                            tempStr.Append(readStream.ReadLine());
                        }
                        returnString = tempStr.ToString();
                    }
                }
                httpWebResponse.Close();

            }
            catch (Exception ex)
            {
                returnString = "";
            }
            return returnString;
        }

        /// <summary>
        /// ͨ��Url��ַ�õ�������Ϣ
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <returns>�����ַ���:"":��ʾ�����쳣��δȡ������</returns>
        public static string GetContentByUrl(string url, string encodetype)
        {
            string returnString = string.Empty;

            if (url == null || url == "")
            {
                return returnString;
            }

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (Stream receiveStream = httpWebResponse.GetResponseStream())
                {
                    Encoding encode = System.Text.Encoding.GetEncoding(encodetype);
                    using (StreamReader readStream = new StreamReader(receiveStream, encode))
                    {
                        StringBuilder tempStr = new StringBuilder();
                        while (!readStream.EndOfStream)
                        {
                            tempStr.Append(readStream.ReadLine());
                        }
                        returnString = tempStr.ToString();
                    }
                }
                httpWebResponse.Close();

            }
            catch (Exception ex)
            {
                returnString = "";
            }
            return returnString;
        }


        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="oldExhaust"></param>
        /// <returns></returns>
        public static string GetExhaust(string oldExhaust)
        {
            string exhaust = "";
            string allExhaust = "";
            List<double> exhaustList = new List<double>();
            string[] exhaustArray = oldExhaust.Split('��');
            foreach (string tmpExhaust in exhaustArray)
            {
                double dEx = 0.0;
                Double.TryParse(tmpExhaust.ToUpper().Replace("L", ""), out dEx);
                if (!exhaustList.Contains(dEx) && dEx != 0.0)
                    exhaustList.Add(dEx);
            }
            exhaustList.Sort();

            for (int i = 0; i < exhaustList.Count; i++)
            {
                // string tmpExhaust = Math.Round(exhaustList[i], 1).ToString();
                string tmpExhaust = Math.Round(exhaustList[i], 1).ToString("n1");
                if (allExhaust.Length > 0)
                    allExhaust += "��";
                allExhaust += tmpExhaust + "L";
            }
            if (exhaustList.Count <= 5)
                exhaust = allExhaust;
            else
                exhaust = exhaustList[0] + "L��" + exhaustList[1] + "L��" + exhaustList[2] + "L������" + exhaustList[exhaustList.Count - 1] + "L";
            return exhaust;
        }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="oldExhaust"></param>
        /// <returns></returns>
        public static string GetExhaust(string oldExhaust, string splitString, bool isShow)
        {
            string exhaust = "";
            string allExhaust = "";
            List<double> exhaustList = new List<double>();
            string[] exhaustArray = oldExhaust.Split('��');
            foreach (string tmpExhaust in exhaustArray)
            {
                double dEx = 0.0;
                Double.TryParse(tmpExhaust.ToUpper().Replace("L", ""), out dEx);
                if (!exhaustList.Contains(dEx) && dEx != 0.0)
                    exhaustList.Add(dEx);
            }
            exhaustList.Sort();

            for (int i = 0; i < exhaustList.Count; i++)
            {
                string tmpExhaust = Math.Round(exhaustList[i], 1).ToString("n1");
                if (allExhaust.Length > 0)
                    allExhaust += splitString;
                allExhaust += tmpExhaust + "L";
            }
            if (exhaustList.Count <= 5 || !isShow)
                exhaust = allExhaust;
            else if (isShow)
                exhaust = exhaustList[0] + "L��" + exhaustList[1] + "L��" + exhaustList[2] + "L������" + exhaustList[exhaustList.Count - 1] + "L";
            return exhaust;
        }

        /// <summary>
        /// ����ȥ����λ�ı��������� 
        /// </summary>
        /// <param name="oldTrans"></param>
        /// <returns></returns>
        public static string GetTransmission(string oldTrans)
        {
            string trans = "";
            List<string> tranList = new List<string>();
            string[] tranArray = oldTrans.Split('��');
            foreach (string tmpTran in tranArray)
            {
                string newTran = tmpTran;
                int pos = newTran.IndexOf("��");
                if (pos >= 0)
                    newTran = newTran.Substring(pos + 1).Trim();
                if (newTran.ToUpper().IndexOf("˫���") >= 0)
                    newTran = "˫���";
                if (newTran.Length > 0 && !tranList.Contains(newTran))
                    tranList.Add(newTran);
            }
            tranList.Sort(NodeCompare.CompareTransmissionType);
            foreach (string tmpTran in tranList)
            {
                if (trans.Length > 0)
                    trans += "��";
                trans += tmpTran;
            }
            if (tranList.Count > 3)
                trans = tranList[0] + "��" + tranList[1] + "������" + tranList[tranList.Count - 1];

            return trans;
        }
        /// <summary>
        /// ����ȥ����λ�ı��������� 
        /// </summary>
        /// <param name="oldTrans"></param>
        /// <returns></returns>
        public static string GetTransmission(string oldTrans, string splitString, bool isShow)
        {
            string trans = "";
            List<string> tranList = new List<string>();
            string[] tranArray = oldTrans.Split('��');
            foreach (string tmpTran in tranArray)
            {
                string newTran = tmpTran;
                int pos = newTran.IndexOf("��");
                if (pos >= 0)
                    newTran = newTran.Substring(pos + 1).Trim();
                if (newTran.ToUpper().IndexOf("˫���") >= 0)
                    newTran = "˫���";
                if (newTran.Length > 0 && !tranList.Contains(newTran))
                    tranList.Add(newTran);
            }
            tranList.Sort(NodeCompare.CompareTransmissionType);
            foreach (string tmpTran in tranList)
            {
                if (trans.Length > 0)
                    trans += splitString;
                trans += tmpTran;
            }
            if (tranList.Count > 3 && isShow)
                trans = tranList[0] + "��" + tranList[1] + "������" + tranList[tranList.Count - 1];

            return trans;
        }

        /// <summary>
        /// ��ȡ��;������
        /// </summary>
        /// <param name="purId"></param>
        /// <returns></returns>
        public static string GetPurposeById(int purId)
        {
            string purposeName = "";
            try
            {
                purId = purId - 459;
                EnumCollection.SerialPurpose purposeEnum = (EnumCollection.SerialPurpose)purId;
                purposeName = purposeEnum.ToString();
            }
            catch
            {

            }
            return purposeName;
        }

        /// <summary>
        /// ��ȡ��;�ı�ǩ����
        /// </summary>
        /// <param name="purName"></param>
        /// <returns></returns>
        public static string GetPurposeTagName(string purName)
        {
            string tagName = "";
            switch (purName)
            {
                case "ԽҰ": tagName = "yy"; break;
                case "ʱ��": tagName = "ss"; break;
                case "����": tagName = "jy"; break;
                case "����": tagName = "db"; break;
                case "����": tagName = "xx"; break;
                case "�˶�": tagName = "yd"; break;
                case "����": tagName = "sw"; break;
                case "cross": tagName = "cr"; break;
                case "�๦��": tagName = "dgn"; break;
            }
            return tagName;
        }
        /// <summary>
        /// �õ�SQLINID�ַ���
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public static string GetSqlInString(List<int> idList)
        {
            if (idList == null || idList.Count < 1)
            {
                return "";
            }
            StringBuilder sqlWhere = new StringBuilder("");
            int index = 0;
            foreach (int i in idList)
            {
                sqlWhere.Append(i.ToString());
                if (index + 1 < idList.Count)
                {
                    sqlWhere.Append(",");
                }
                index++;
            }
            return sqlWhere.ToString();
        }

        /// <summary>
        /// ��ȡָ���������͵����һ����,���ȣ���
        /// </summary>
        /// <param name="indexType">��������(Dealer|Media|Sale|UV)</param>
        /// <param name="dateType">��������(Season|Month|Week)</param>
        /// <returns></returns>
        public static DateObj GetLastDate(IndexType indexType, DateType dateType)
        {
            string keyName = "Last_indexType" + indexType.ToString() + "_DateType" + dateType.ToString();

            if (CacheManager.GetCachedData(keyName) == null)
            {
                DateObj dateObj = new DateObj();
                string basePath = Path.Combine(WebConfig.IndexDataBlockPath, indexType.ToString() + "\\" + dateType.ToString());
                dateObj.Year = GetMaxNumInPath(basePath);

                if (dateObj.Year > 0)
                {
                    basePath = Path.Combine(basePath, dateObj.Year.ToString());
                    dateObj.DateNum = GetMaxNumInPath(basePath);
                }

                CacheManager.InsertCache(keyName, dateObj, 60);
            }

            return (DateObj)CacheManager.GetCachedData(keyName);
        }

        /// <summary>
        /// ��ȡĿ¼������һ����
        /// </summary>
        /// <param name="basePath"></param>
        /// <returns></returns>
        private static int GetMaxNumInPath(string basePath)
        {
            string[] dirs = Directory.GetDirectories(basePath);
            int maxNum = 0;

            foreach (string dir in dirs)
            {
                int pos = dir.LastIndexOf("\\");

                if (pos > 0)
                {
                    int dateNum = 0;
                    Int32.TryParse(dir.Substring(pos + 1), out dateNum);

                    if (dateNum > maxNum)
                    {
                        maxNum = dateNum;
                    }
                }
            }

            return maxNum;
        }

        /// <summary>
        /// ����ָ��
        /// </summary>
        /// <param name="indexNum">ָ����ԭʼ����</param>
        /// <returns></returns>
        public static int ComputeIndex(int indexNum, IndexType indexType)
        {
            //����ָ��=����/3.76*2.79
            //ý��ָ��=����/4.66
            //if (indexType == IndexType.UV || indexType == IndexType.Compare)
            //    indexNum = (int)Math.Round(indexNum * 2.23, MidpointRounding.AwayFromZero);
            if (indexType == IndexType.Dealer)
                indexNum = (int)Math.Round(indexNum * 0.74, MidpointRounding.AwayFromZero);
            else if (indexType == IndexType.Media)
                indexNum = (int)Math.Round(indexNum / 13.98, MidpointRounding.AwayFromZero);
            return indexNum;
        }

        /// <summary>
        /// ����ٷֱȵ��ַ���
        /// </summary>
        /// <param name="partNum"></param>
        /// <param name="bigNum"></param>
        /// <returns></returns>
        public static string ComputePercent(int partNum, int bigNum)
        {
            double per = (double)partNum / (double)bigNum;
            per = Math.Round(per * 100, MidpointRounding.AwayFromZero);
            if (per < 1)
                per = 1;
            return per + "%";
        }

        /// <summary>
        /// ���ݼ���ƴдȡ�����ļ�����׺
        /// </summary>
        /// <param name="levelSpell"></param>
        /// <returns></returns>
        public static string GetLevelFilePostfixBySpell(string levelSpell)
        {
            string filePostfix = "0";
            switch (levelSpell)
            {
                case "weixingche": filePostfix = "1";
                    break;
                case "xiaoxingche": filePostfix = "2";
                    break;
                case "jincouxingche": filePostfix = "3";
                    break;
                case "zhongdaxingche": filePostfix = "4";
                    break;
                case "zhongxingche": filePostfix = "5";
                    break;
                case "haohuaxingche": filePostfix = "6";
                    break;
                case "mpv": filePostfix = "7";
                    break;
                case "suv": filePostfix = "8";
                    break;
                case "paoche": filePostfix = "9";
                    break;
                case "qita": filePostfix = "10";
                    break;
                case "mianbaoche": filePostfix = "11";
                    break;
                case "pika": filePostfix = "12";
                    break;
                default:
                    filePostfix = "0";
                    break;
            }
            return filePostfix;
        }

        /// <summary>
        /// ����ٷֱȣ�������λС��
        /// </summary>
        /// <param name="partNum"></param>
        /// <param name="bigNum"></param>
        /// <returns></returns>
        public static double ComputeDoublePercent(int partNum, int bigNum)
        {
            double per = (double)partNum / (double)bigNum;
            per = Math.Round(per * 10000, MidpointRounding.AwayFromZero);
            per /= 100;
            return per;
        }
        /// <summary>
        /// �õ������б�
        /// </summary>
        /// <param name="pagesize">ȡ������</param>
        /// <param name="startindex">��ʼλ��</param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XmlDocument GetNewsList(int pagesize, int startindex, string path)
        {
            try
            {
                StringBuilder xmlContent = new StringBuilder("");
                XmlReader xmlReader = XmlReader.Create(path);
                int index = 0;
                int endIndex = startindex + pagesize;
                //����XML�ĵ�
                while (xmlReader.Read())
                {
                    if (index >= endIndex) break;

                    if (xmlReader.NodeType == XmlNodeType.Element
                        && xmlReader.LocalName.ToLower() == "listnews")
                    {
                        if (index >= startindex)
                        {
                            xmlContent.AppendFormat("<listnews>{0}</listnews>", xmlReader.ReadInnerXml());
                        }
                        index++;
                    }
                }

                if (string.IsNullOrEmpty(xmlContent.ToString())) return null;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml("<root>" + xmlContent.ToString() + "</root>");
                return xmlDoc;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// �õ���ҳ��
        /// </summary>
        /// <param name="dataCount">��ǰ����������</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <returns></returns>
        public static int GetTotalPageNumber(int dataCount, int pageSize)
        {
            int pageCount = dataCount / pageSize + (dataCount % pageSize == 0 ? 0 : 1);
            return pageCount;
        }

        /// <summary>
        /// ��ʼ������ĸ�����ǩ ����ƥ�����
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, EnumCollection.PingCeTag> IntiPingCeTagInfo()
        {
            Dictionary<int, EnumCollection.PingCeTag> dic = new Dictionary<int, EnumCollection.PingCeTag>();
            // ����
            EnumCollection.PingCeTag pct1 = new EnumCollection.PingCeTag();
            pct1.tagName = "����";
            pct1.tagRegularExpressions = "(���|����:)";
            dic.Add(1, pct1);
            // ���
            EnumCollection.PingCeTag pct2 = new EnumCollection.PingCeTag();
            pct2.tagName = "���";
            pct2.tagRegularExpressions = "(��ۣ�|���:)";
            dic.Add(2, pct2);
            // ����
            EnumCollection.PingCeTag pct3 = new EnumCollection.PingCeTag();
            pct3.tagName = "����";
            pct3.tagRegularExpressions = "(���Σ�|����:)";
            dic.Add(3, pct3);
            // �ռ�
            EnumCollection.PingCeTag pct4 = new EnumCollection.PingCeTag();
            pct4.tagName = "�ռ�";
            pct4.tagRegularExpressions = "(�ռ䣺|�ռ�:)";
            dic.Add(4, pct4);
            // ��Ұ
            EnumCollection.PingCeTag pct5 = new EnumCollection.PingCeTag();
            pct5.tagName = "��Ұ";
            pct5.tagRegularExpressions = "(��Ұ��|��Ұ:)";
            dic.Add(5, pct5);
            // �ƹ�
            EnumCollection.PingCeTag pct6 = new EnumCollection.PingCeTag();
            pct6.tagName = "�ƹ�";
            pct6.tagRegularExpressions = "(�ƹ⣺|�ƹ�:)";
            dic.Add(6, pct6);
            // ����
            EnumCollection.PingCeTag pct7 = new EnumCollection.PingCeTag();
            pct7.tagName = "����";
            pct7.tagRegularExpressions = "(������|����:)";
            dic.Add(7, pct7);
            // �ٿ�
            EnumCollection.PingCeTag pct8 = new EnumCollection.PingCeTag();
            pct8.tagName = "�ٿ�";
            pct8.tagRegularExpressions = "(�ٿأ�|�ٿ�:)";
            dic.Add(8, pct8);
            // ������
            EnumCollection.PingCeTag pct9 = new EnumCollection.PingCeTag();
            pct9.tagName = "������";
            pct9.tagRegularExpressions = "(�����ԣ�|���ʣ�|������:|����:)";
            dic.Add(9, pct9);
            // �ͺ�
            EnumCollection.PingCeTag pct10 = new EnumCollection.PingCeTag();
            pct10.tagName = "�ͺ�";
            pct10.tagRegularExpressions = "(�ͺģ�|�ͺ�:)";
            dic.Add(10, pct10);
            // ����
            EnumCollection.PingCeTag pct11 = new EnumCollection.PingCeTag();
            pct11.tagName = "����";
            pct11.tagRegularExpressions = "(�����밲ȫ��|���ã�|�����밲ȫ:|����:)";
            dic.Add(11, pct11);
            // �ܽ�
            EnumCollection.PingCeTag pct12 = new EnumCollection.PingCeTag();
            pct12.tagName = "�ܽ�";
            pct12.tagRegularExpressions = "(�ܽ᣺|�ܽ�:)";
            dic.Add(12, pct12);
            return dic;
        }

		/// <summary>
		/// �����ǩ
		/// </summary>
		/// <returns></returns>
		public static List<EnumCollection.PingCeTag> IntiPingCeTagListInfo()
		{
			List<EnumCollection.PingCeTag> list = new List<EnumCollection.PingCeTag>();
			// ����
			EnumCollection.PingCeTag pct1 = new EnumCollection.PingCeTag();
			pct1.tagName = "����";
			pct1.tagRegularExpressions = "(���|����:)";
			pct1.tagId = 1;
			list.Add(pct1);
			// ���
			EnumCollection.PingCeTag pct2 = new EnumCollection.PingCeTag();
			pct2.tagName = "���";
			pct2.tagRegularExpressions = "(��ۣ�|���:)";
			pct2.tagId = 2;
			list.Add( pct2);
			// ����
			EnumCollection.PingCeTag pct3 = new EnumCollection.PingCeTag();
			pct3.tagName = "����";
			pct3.tagRegularExpressions = "(���Σ�|����:)";
			pct3.tagId = 3;
			list.Add( pct3);
			// �ռ�
			EnumCollection.PingCeTag pct4 = new EnumCollection.PingCeTag();
			pct4.tagName = "�ռ�";
			pct4.tagRegularExpressions = "(�ռ䣺|�ռ�:)";
			pct4.tagId = 4;
			list.Add( pct4);
			// ��Ұ
			EnumCollection.PingCeTag pct5 = new EnumCollection.PingCeTag();
			pct5.tagName = "��Ұ";
			pct5.tagRegularExpressions = "(��Ұ��|��Ұ:)";
			pct5.tagId = 5;
			list.Add( pct5);
			// �ƹ�
			EnumCollection.PingCeTag pct6 = new EnumCollection.PingCeTag();
			pct6.tagName = "�ƹ�";
			pct6.tagRegularExpressions = "(�ƹ⣺|�ƹ�:)";
			pct6.tagId = 6;
			list.Add(pct6);
			// ����
			EnumCollection.PingCeTag pct7 = new EnumCollection.PingCeTag();
			pct7.tagName = "����";
			pct7.tagRegularExpressions = "(������|����:)";
			pct7.tagId = 7;
			list.Add(pct7);
			// �ٿ�
			EnumCollection.PingCeTag pct8 = new EnumCollection.PingCeTag();
			pct8.tagName = "�ٿ�";
			pct8.tagRegularExpressions = "(�ٿأ�|�ٿ�:)";
			pct8.tagId = 8;
			list.Add( pct8);
			// ������
			EnumCollection.PingCeTag pct9 = new EnumCollection.PingCeTag();
			pct9.tagName = "������";
			pct9.tagRegularExpressions = "(�����ԣ�|���ʣ�|������:|����:)";
			pct9.tagId = 9;
			list.Add( pct9);
			// �ͺ�
			EnumCollection.PingCeTag pct10 = new EnumCollection.PingCeTag();
			pct10.tagName = "�ͺ�";
			pct10.tagRegularExpressions = "(�ͺģ�|�ͺ�:)";
			pct10.tagId = 10;
			list.Add( pct10);
			// ����
			EnumCollection.PingCeTag pct11 = new EnumCollection.PingCeTag();
			pct11.tagName = "����";
			pct11.tagRegularExpressions = "(�����밲ȫ��|���ã�|�����밲ȫ:|����:)";
			pct11.tagId = 11;
			list.Add( pct11);
			// �ܽ�
			EnumCollection.PingCeTag pct12 = new EnumCollection.PingCeTag();
			pct12.tagName = "�ܽ�";
			pct12.tagRegularExpressions = "(�ܽ᣺|�ܽ�:)";
			pct12.tagId = 12;
			list.Add( pct12);
			return list;
		}

        public static void WriteLog(string errStr)
        {
            errStr = "\r\nIP: " + BitAuto.Utils.WebUtil.GetClientIP() +
            "\r\nError in: " + HttpContext.Current.Request.Url.ToString() +
            "\r\nStack Trace: " + errStr;

            string subDir = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + "-" + DateTime.Now.Hour.ToString();
            string sDir = AppDomain.CurrentDomain.BaseDirectory + "GlobalLog\\" + subDir + "\\";
            try
            {
                if (!System.IO.Directory.Exists(sDir))
                {
                    System.IO.Directory.CreateDirectory(sDir);
                }
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sDir + "" + fileName + ".txt", true))
                {
                    sw.Write("[" + DateTime.Now.ToString() + "]  " + errStr + "\r\n\r\n");
                    sw.Close();
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contetn"></param>
        public static void WriteInvokeLog(string contetn)
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd");
            string sDir = AppDomain.CurrentDomain.BaseDirectory + "InvokeLog\\";
            try
            {
                if (!System.IO.Directory.Exists(sDir))
                {
                    System.IO.Directory.CreateDirectory(sDir);
                }
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sDir + "" + fileName + ".txt", true))
                {
                    sw.Write("[" + DateTime.Now.ToString() + "]  " + contetn + "\r\n");
                    sw.Close();
                }
            }
            catch
            { }
        }

        /// <summary>
        /// ������־
        /// </summary>
        /// <param name="subDir">InvokeLog ����Ŀ¼��</param>
        /// <param name="contetn">����</param>
        public static void WriteInvokeLog(string subDir, string contetn)
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd");
            string sDir = AppDomain.CurrentDomain.BaseDirectory + "InvokeLog\\" + subDir + "\\";
            try
            {
                if (!System.IO.Directory.Exists(sDir))
                {
                    System.IO.Directory.CreateDirectory(sDir);
                }
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sDir + "" + fileName + ".txt", true))
                {
                    sw.Write("[" + DateTime.Now.ToString() + "]  " + contetn + "\r\n");
                    sw.Close();
                }
            }
            catch
            { }
        }

        /// <summary>
        /// �õ��ӳ��ж�Ӧ�ĸ�����
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, int> GetCityRelationParentDic()
        {
            string cacheKey = "CommonFunction_GetCityRelationParentDic";
            Dictionary<int, int> cityParentDic = (Dictionary<int, int>)CacheManager.GetCachedData(cacheKey);
            if (cityParentDic == null)
            {
                cityParentDic = new Dictionary<int, int>();
                Dictionary<int, int> orderDic = new Dictionary<int, int>();
                string filePath = Path.Combine(WebConfig.DataBlockPath, "Data\\City\\needrelationmap.xml");
                if (File.Exists(filePath))
                {
                    //cityParentDic���ȴ洢ÿ�����еĸ����е�order,Ȼ���ٽ���order��ָ�ĳ���ID������
                    XmlReader reader = XmlReader.Create(filePath);
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element)
                            continue;
                        if (reader.LocalName == "City")
                        {
                            reader.MoveToAttribute("ID");
                            int cityId = BitAuto.Utils.ConvertHelper.GetInteger(reader.Value);
                            if (reader.MoveToAttribute("CityOrder"))
                            {
                                int cityOrder = BitAuto.Utils.ConvertHelper.GetInteger(reader.Value);
                                orderDic[cityOrder] = cityId;
                            }
                            else if (reader.MoveToAttribute("ParentCityOrder"))
                            {
                                int parentCityOrder = BitAuto.Utils.ConvertHelper.GetInteger(reader.Value);
                                cityParentDic[cityId] = parentCityOrder;
                            }
                        }
                    }
                    reader.Close();
                    //������ָ��������order��ָ�ĳ���ID������
                    int[] cityIds = new int[cityParentDic.Count];
                    cityParentDic.Keys.CopyTo(cityIds, 0);
                    foreach (int cityId in cityIds)
                    {
                        if (orderDic.ContainsKey(cityParentDic[cityId]))
                            cityParentDic[cityId] = orderDic[cityParentDic[cityId]];
                        else
                            cityParentDic[cityId] = 0;
                    }

                }
                System.Web.Caching.CacheDependency cd = new System.Web.Caching.CacheDependency(filePath);
                CacheManager.InsertCache(cacheKey, cityParentDic, cd, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
            }

            return cityParentDic;
        }

        /// <summary>
        /// �������ű���
        /// </summary>
        public static string NewsTitleDecode(string title)
        {
            if (string.IsNullOrEmpty(title))
                return title;
            return BitAuto.Utils.StringHelper.RemoveHtmlTag(HttpUtility.HtmlDecode(title));
        }

        /// <summary>
        /// ȡ�ַ�����unicode����
        /// modified by chengl 2015-8-26 fix bug
        /// </summary>
        /// <param name="srcString"></param>
        /// <returns></returns>
        public static string GetUnicodeByString(string srcString)
        {
            if (!string.IsNullOrEmpty(srcString) && srcString.Length > 0)
            {
                List<string> listTemp = new List<string>(srcString.Length);
                char[] src = srcString.ToCharArray();
                for (int i = 0; i < src.Length; i++)
                {
                    byte[] bytes = Encoding.Unicode.GetBytes(src[i].ToString());
                    string str = @"\u" + bytes[1].ToString("X2") + bytes[0].ToString("X2");
                    listTemp.Add(str);
                }
                return string.Join("", listTemp.ToArray());
            }
            else
            {
                return "";
            }

            //string temp = "";
            //char[] src = srcString.ToCharArray();
            //for (int i = 0; i < src.Length; i++)
            //{
            //    byte[] bytes = Encoding.Unicode.GetBytes(src[i].ToString());
            //    string str = @"\u" + bytes[1].ToString("X2") + bytes[0].ToString("X2");
            //    temp += str;
            //}
            //return temp;
        }
        /// <summary>
        /// ��������ʱ�����͵�ʱ�������Ϊ��
        /// </summary>
        /// <param name="dateInterval"></param>
        /// <param name="dateTime1"></param>
        /// <param name="dateTime2"></param>
        /// <returns></returns>
        public static int DateDiff(string dateInterval, DateTime dateTime1, DateTime dateTime2)
        {
            int dateDiff = 0;
            try
            {
                TimeSpan timeSpan = new TimeSpan(dateTime2.Ticks - dateTime1.Ticks);

                switch (dateInterval.ToLower())
                {
                    case "year":
                    case "y":
                        dateDiff = dateTime2.Year - dateTime1.Year;
                        break;
                    case "month":
                    case "m":
                        dateDiff = (dateTime2.Year * 12 + dateTime2.Month) - (dateTime1.Year * 12 + dateTime1.Month);
                        break;
                    case "day":
                    case "d":
                        dateDiff = (int)timeSpan.TotalDays;
                        break;
                    case "hour":
                    case "h":
                        dateDiff = (int)timeSpan.TotalHours;
                        break;
                    case "minute":
                    case "n":
                        dateDiff = (int)timeSpan.TotalMinutes;
                        break;
                    case "second":
                    case "s":
                        dateDiff = (int)timeSpan.TotalSeconds;
                        break;
                    case "milliseconds":
                    case "ms":
                        dateDiff = (int)timeSpan.TotalMilliseconds;
                        break;
                    default:
                        dateDiff = (int)timeSpan.TotalMinutes;
                        break;
                }
            }
            catch
            {

            }
            return dateDiff;
        }
        public static string GetUserIp()
        {
            string ip = String.Empty;
            ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }
            if (!IsValidIp(ip))
                ip = "0.0.0.0";
            return ip;
        }
        public static bool IsValidIp(string strIP)
        {
            if (string.IsNullOrEmpty(strIP))
                return false;
            Regex rx = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
            return rx.IsMatch(strIP);
        }

        /// <summary>
        /// �ж���T����L
        /// </summary>
        /// <param name="p425">������ʽ</param>
        /// <param name="p408">��ѹ��ʽ</param>
        /// <returns></returns>
        public static string GetInhaleType(string p425, string p408)
        {
            string inhaleType = "L";
            if (!string.IsNullOrEmpty(p425) && p425.IndexOf("��ѹ") >= 0)
            {
                inhaleType = "T";
            }
            return inhaleType;
        }

        /// <summary>
        /// �������ʽ���
        /// </summary>
        /// <param name="cpCountry">���̹���</param>
        /// <param name="bsCountry">��Ʒ�ƹ���</param>
        /// <returns></returns>
        public static string GetImport(string cpCountry, string bsCountry)
        {
            string import = "";
            if (!string.IsNullOrEmpty(cpCountry) && !string.IsNullOrEmpty(bsCountry))
            {
                if (cpCountry == "�й�" && bsCountry == "�й�")
                { import = "����"; }
                else if (cpCountry != "�й�" && bsCountry != "�й�")
                { import = "����"; }
                else
                { import = "����"; }
            }
            return import;
        }

        /// <summary>
        /// ���ݿ��еı��������� ת��Ϊ�ֶ�|�Զ�
        /// </summary>
        /// <param name="allTrans"></param>
        /// <param name="oldSplitChar">�ϵı������ָ���</param>
        /// <param name="newSplitChar">������ı������ָ��</param>
        /// <returns></returns>
        public static string GetTransbyDataBase(string allTrans, char oldSplitChar, string newSplitChar)
        {
            string tempTran = "";
            if (!string.IsNullOrEmpty(allTrans) && !string.IsNullOrEmpty(newSplitChar))
            {
                List<string> transList = new List<string>(2);
                if (allTrans.IndexOf(oldSplitChar) >= 0)
                {
                    string[] arrTra = allTrans.Split(new char[] { oldSplitChar }, StringSplitOptions.RemoveEmptyEntries);
                    if (arrTra.Length > 0)
                    {
                        for (int i = 0; i < arrTra.Length; i++)
                        {
                            string tmpTrans = "�ֶ�";
                            if (arrTra[i].IndexOf("�ֶ�") == -1)
                                tmpTrans = "�Զ�";
                            if (!transList.Contains(tmpTrans))
                                transList.Add(tmpTrans);
                            if (transList.Count >= 2)
                                break;
                        }
                    }
                    if (transList.Count > 0)
                    { tempTran = string.Join(newSplitChar, transList.ToArray()); }
                }
            }
            return tempTran;
        }

        /// <summary>
        /// ����ͼƬurl���¿�� ȡ�³ߴ�ͼƬ
        /// </summary>
        /// <param name="imgURL">ͼƬ��ַ</param>
        /// <param name="width">�¿��</param>
        /// <returns></returns>
        public static string GetPicOtherSize(string imgURL, int width)
        {
            string otherSize = "";
            if (imgURL.IndexOf("autoalbum") > 0)
            {
                string tempFile = imgURL.Substring(imgURL.IndexOf("autoalbum"));
                otherSize = string.Format("http://img4.bitautoimg.com/wapimg-{0}-9999/{1}", width, tempFile);
            }
            return otherSize;
        }

        /// <summary>
        /// ͳһ���XML
        /// </summary>
        /// <param name="hr">HttpResponse</param>
        /// <param name="xmlContent">xml����</param>
        /// <param name="rootName">���ڵ� ��Ϊ����Ӹ��ڵ�</param>
        public static void EchoXml(HttpResponse hr, string xmlContent, string rootName)
        {
            hr.ContentType = "text/xml";
            hr.ContentEncoding = Encoding.UTF8;
            hr.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            if (rootName != "")
            {
                sb.AppendFormat("<{0}>", rootName);
            }
            sb.Append(xmlContent);
            if (rootName != "")
            {
                sb.AppendFormat("</{0}>", rootName);
            }
            hr.Write(sb.ToString());
        }
        /// <summary>
        /// ��������ת��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T Cast<T>(object o, T type)
        {
            return (T)o;
        }

        /// <summary>
        /// ��url�л�ȡ���� XML
        /// </summary>
        /// <param name="url">����url</param>
        /// <param name="interval">��ʱʱ�� (����)</param>
        /// <returns></returns>
        public static XmlDocument ReadXmlFromUrl(string url, int interval = 60000)
        {
            XmlDocument xmlDoc = new XmlDocument();
            HttpWebRequest req = null;
            HttpWebResponse response = null;
            Stream responseStream = null;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Timeout = interval;
                using (response = req.GetResponse() as HttpWebResponse)
                using (responseStream = response.GetResponseStream())
                {
                    using (XmlTextReader reader = new XmlTextReader(responseStream))
                    {
                        xmlDoc.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
				WriteLog("url : " + url + "\r\n" + ex.ToString());
            }
            finally
            {
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (req != null)
                {
                    req.Abort();
                }
            }
            return xmlDoc;
        }

        /// <summary>
        /// DateTimeʱ���ʽת��ΪUnixʱ�����ʽ
        /// </summary>
        /// <param name="time"> DateTimeʱ���ʽ</param>
        /// <returns>Unixʱ�����ʽ</returns>
        public static int ConvertDateTimeInt(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// �����ַ�������SHA1
        /// </summary>
        /// <param name="stringSor"></param>
        /// <returns></returns>
        public static string GenerateSHA1Hash(string stringSor)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = UTF8Encoding.Default.GetBytes(stringSor);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            return BitConverter.ToString(bytes_sha1_out).Replace("-", ""); ;
        }

    }
}
