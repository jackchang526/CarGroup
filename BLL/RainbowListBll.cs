using System;
using System.Web;
using BitAuto.CarChannel.Common;
using System.Text;
using System.Data;
using BitAuto.CarChannel.BLL;
using System.Collections.Generic;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using System.Collections;
using BitAuto.Utils;

namespace BitAuto.CarChannel.Common.Interface
{
    public class RainbowListBll
    {
        private StringBuilder sbXML;

        private string[] NDomesticCarRBItemIDs = WebConfig.NDomesticCarRBItemIDs.Split(',');
        private string[] DomesticCarRBItemIDs = WebConfig.DomesticCarRBItemIDs.Split(',');

        public string GetRainbowListXML_CSID(int nCSID)
        {
            string xml = string.Empty;
            try
            {
                if (-1 != nCSID)
                {
                    xml = BuildRainbowXMLByCSID(nCSID);
                }
            }
            catch (Exception err)
            {
            }
            return xml;
        }

        public string GetRainbowListXML_All()
        {
            string xml = string.Empty;

            xml = BuildRainbowXML();
           
            return xml;
        }

        private string BuildRainbowXML()
        {
            string strXML = string.Empty;

            string cacheKey = "Interface_Rainbow_All";
            if (null == CacheManager.GetCachedData(cacheKey))
            {
                sbXML = new StringBuilder();

                sbXML.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");

                int nDisplayStatus = (int)EnumRainbowDisplayStatus.Show;
                Dictionary<string, DataSet> dicRainbows = new Car_SerialBll().GetCSRainbowsList(nDisplayStatus);
                if (null != dicRainbows && dicRainbows.Count > 0)
                {
                    sbXML.AppendFormat("<RainbowRoot Time= '{0}'>", DateTime.Now.ToString());

                    if (dicRainbows.ContainsKey("国产车"))
                    {
                        sbXML.Append(GetSerialsXML("国产车", dicRainbows["国产车"], WebConfig.DomesticCarRBItemIDs, 5));
                    }

                    if (dicRainbows.ContainsKey("进口车"))
                    {
                        sbXML.Append(GetSerialsXML("进口车", dicRainbows["进口车"], WebConfig.NDomesticCarRBItemIDs, 5));
                    }

                    sbXML.AppendLine("</RainbowRoot>");
                }

                strXML = sbXML.ToString();
                CacheManager.InsertCache(cacheKey, strXML, WebConfig.CachedDuration);
            }
            else
            {
                strXML = Convert.ToString(CacheManager.GetCachedData(cacheKey));
            }

            return strXML;
        }

        private string GetSerialsXML(string key, DataSet dsRainbows, string CSRBItemIDs, int nNum)
        {
            // modified by chengl Oct.21.2009
            string[] tempRIs = CSRBItemIDs.Split(',');

            DataSet dsCSRBItem = new Car_SerialBll().GetCSRBItemByIDs(CSRBItemIDs);

            StringBuilder sb = new StringBuilder();
            if (dsRainbows.Tables.Count > 0 && dsCSRBItem != null && dsCSRBItem.Tables[0] != null && dsCSRBItem.Tables[0].Rows.Count > 0)
            {
                sb.AppendFormat("<Serials Type='{0}'>", key);

                int nCount = 0;

                foreach (DataTable dtRainbows in dsRainbows.Tables)
                {
                    nCount++;
                    if (nCount > nNum)
                        break;

                    ArrayList alYes = new ArrayList();

                    sb.AppendFormat("<Serial ID='{0}' Name='{1}' ShowName='{2}' AllSpell='{3}'>",
                                        dtRainbows.Rows[0]["cs_id"].ToString().Trim(),
                                        dtRainbows.Rows[0]["cs_name"].ToString().Trim(),
                                        dtRainbows.Rows[0]["cs_showname"].ToString().Trim(),
                                        dtRainbows.Rows[0]["allspell"].ToString().Trim());
                    //有数据的
                    // foreach (DataRow dr in dsCSRBItem.Tables[0].Rows)
                    foreach (string riID in tempRIs)
                    {
                        // 口碑URL
                        if (riID == "36")
                        {
                            sb.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\" URL=\"http://car.bitauto.com/{2}/koubei/\" Time=\"{3}\"/>",
                                                    riID,
                                                    "口碑",
                                                    dtRainbows.Rows[0]["allspell"].ToString().Trim(),
                                                    DateTime.Now.ToString());
                        }
                        else
                        {
                            int nRaindowItemID = int.Parse(riID);
                            // int nRaindowItemID = Convert.ToInt32(dr["RID"]);
                            // modifed by chengl Oct.21.2009
                            DataRow[] drs = dtRainbows.Select(" RainbowItemID='" + nRaindowItemID.ToString() + "' ");
                            if (drs != null && drs.Length > 0)
                            {
                                sb.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\" URL=\"{2}\" Time=\"{3}\"/>",
                                                    nRaindowItemID.ToString(),
                                                    drs[0]["RName"].ToString(),
                                                    drs[0]["URL"].ToString(),
                                                    drs[0]["UrlTime"].ToString());
                            }
                            else
                            {
                                if (!alYes.Contains(nRaindowItemID))
                                {
                                    DataRow[] drsRI = dsCSRBItem.Tables[0].Select(" RID='" + nRaindowItemID.ToString() + "' ");
                                    if (drsRI != null && drsRI.Length > 0)
                                    {
                                        sb.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\" URL=\"{2}\" Time=\"{3}\"/>", nRaindowItemID, drsRI[0]["RName"].ToString(), string.Empty, string.Empty);
                                    }
                                    else
                                    {
                                        sb.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\" URL=\"{2}\" Time=\"{3}\"/>", nRaindowItemID, string.Empty, string.Empty, string.Empty);
                                    }
                                }
                            }
                        }
                        //foreach (DataRow drRainbow in dtRainbows.Rows)
                        //{
                        //    int nCRainbowItemID = int.Parse(drRainbow["RainbowItemID"].ToString());
                            
                            //if (nRaindowItemID == nCRainbowItemID)
                            //{
                            //    sb.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\" URL=\"{2}\" Time=\"{3}\"/>",
                            //                        nCRainbowItemID,
                            //                        drRainbow["RName"].ToString(),
                            //                        drRainbow["URL"].ToString(),
                            //                        drRainbow["UrlTime"].ToString());

                            //    alYes.Add(nRaindowItemID);
                            //}
                        //}
                    }

                    //没有对应数据的
                    //foreach (DataRow dr in dsCSRBItem.Tables[0].Rows)
                    //{
                    //    int nRaindowItemID = Convert.ToInt32(dr["RID"]);
                    //    if (!alYes.Contains(nRaindowItemID))
                    //    {
                    //        sb.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\" URL=\"{2}\" Time=\"{3}\"/>", nRaindowItemID, dr["RName"].ToString(), string.Empty, string.Empty);
                    //    }
                    //}

                    sb.AppendLine("</Serial>");
                }

                sb.AppendLine("</Serials>");
            }
            return sb.ToString();
        }

        private string BuildRainbowXMLByCSID(int nCSID)
        {
            string strXML = string.Empty;

            string[] CSRBItemIDs = DomesticCarRBItemIDs;
            string strCSRBItemIDs = WebConfig.DomesticCarRBItemIDs;
			string carCountry = "国产车";
            if (!IsDomesticCar(nCSID))
            {
                CSRBItemIDs = NDomesticCarRBItemIDs;
                strCSRBItemIDs = WebConfig.NDomesticCarRBItemIDs;
				carCountry = "进口车";
            }

            string cacheKey = "Interface_Rainbow_CSID_" + nCSID;
			strXML = (string)CacheManager.GetCachedData(cacheKey);
            if (null == strXML)
            {
                sbXML = new StringBuilder();

                sbXML.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");

                // modified by chengl Aug.28.2009
                DataSet dsRainbows = new Car_SerialBll().GetCSRainbowsListByID(nCSID, strCSRBItemIDs);

                Car_SerialEntity serial = new Car_SerialBll().Get_Car_SerialByCsID(nCSID);
                // modified by chengl Aug.28.2009 end
                if (null != dsRainbows && null != dsRainbows.Tables[0] && dsRainbows.Tables[0].Rows.Count > 0)
                {
					bool isShow = ConvertHelper.GetInteger(dsRainbows.Tables[0].Rows[0]["IsShow"]) == 1 ? true : false;
                    foreach (DataRow drRainbow in dsRainbows.Tables[0].Rows)
                    {
                        if (drRainbow["IsShow"].ToString() == "1")
                        { 
                            isShow = true;
                            break;
                        }
                    }
                    sbXML.AppendFormat("<RainbowRoot Time= '{0}'>", DateTime.Now.ToString());
                    //dsRainbows.Tables[0].Rows[0]["cs_id"].ToString().Trim(),
                    //dsRainbows.Tables[0].Rows[0]["cs_name"].ToString().Trim(),
                    //dsRainbows.Tables[0].Rows[0]["cs_showname"].ToString().Trim(),
                    //dsRainbows.Tables[0].Rows[0]["allspell"].ToString().Trim());
                    sbXML.AppendFormat("<Serial ID='{0}' Name='{1}' ShowName='{2}' AllSpell='{3}' IsShow='{4}' CarCountry='{5}'>",
                                        serial.Cs_Id.ToString(),
                                        serial.Cs_Name,
                                        serial.Cs_ShowName,
                                        serial.Cs_AllSpell,
										isShow.ToString(),
										carCountry);

                    foreach (string strItemID in CSRBItemIDs)
                    {
                        int nRaindowItemID = Convert.ToInt32(strItemID);
                        foreach (DataRow drRainbow in dsRainbows.Tables[0].Rows)
                        {
                            int nCRainbowItemID = int.Parse(drRainbow["RainbowItemID"].ToString());
                            if (nRaindowItemID == nCRainbowItemID)
                            {
                                if (nRaindowItemID == 36)
                                {
                                    sbXML.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\" URL=\"{2}\" Time=\"{3}\"/>",
                                                        nRaindowItemID.ToString(),
                                                        drRainbow["RName"].ToString(),
                                                        "http://car.bitauto.com/" + serial.Cs_AllSpell.ToLower()+"/koubei/",
                                                        DateTime.Now.ToString());
                                }
                                else
                                {
                                    sbXML.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\" URL=\"{2}\" Time=\"{3}\"/>",
                                                        nRaindowItemID.ToString(),
                                                        drRainbow["RName"].ToString(),
                                                        drRainbow["URL"].ToString(),
                                                        drRainbow["UrlTime"].ToString());
                                }
                            }
                        }
                    }

                    sbXML.AppendLine("</Serial>");

                    sbXML.AppendLine("</RainbowRoot>");
                }

                strXML = sbXML.ToString();
                CacheManager.InsertCache(cacheKey, strXML, WebConfig.CachedDuration);
            }
            else
            {
                strXML = Convert.ToString(CacheManager.GetCachedData(cacheKey));
            }

            return strXML;
        }

        private bool IsDomesticCar(int nCSID)
        {
            return new Car_SerialBll().IsDomesticCar(nCSID);
        }

    }
}
