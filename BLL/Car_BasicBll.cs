using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.IO;
using System.Linq;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using BitAuto.CarChannel.Model.AppModel;
using System.Web.Caching;
using System.Configuration;

namespace BitAuto.CarChannel.BLL
{
    public class Car_BasicBll
    {
        private static readonly Car_BasicDal cbd = new Car_BasicDal();
        private static readonly CarInfoDal cid = new CarInfoDal();
        static string parameterConfigPath = ConfigurationManager.AppSettings["ParameterConfigPath"];

        public Car_BasicBll()
        { }

        /// <summary>
        /// ȡ������Ч����
        /// </summary>
        /// <returns></returns>
        public IList<Car_BasicEntity> Get_Car_BasicAll()
        {
            return cbd.Get_Car_BasicAll();
        }

        /// <summary>
        /// ȡ���г��� ������Ч��
        /// add by chengl Apr.10.2013
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarContainNoValid()
        {
            return cbd.GetAllCarContainNoValid();
        }

        /// <summary>
        /// ���ݳ���IDȡ���ͻ�������Ϣ
        /// </summary>
        /// <param name="carid">����ID</param>
        /// <returns></returns>
        public DataSet GetCarDetailById(int carid)
        {
            return cbd.GetCarDetailById(carid);
        }

        /// <summary>
        /// ȡ������Ч���� ������Ʒ��ID������ID����
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarOrderbyCs(int serialId)
        {
            return cbd.GetAllCarOrderbyCs(serialId);
        }

        /// <summary>
        /// ���ݳ���IDȡ����
        /// </summary>
        /// <param name="carID"></param>
        /// <returns></returns>
        public Car_BasicEntity Get_Car_BasicByCarID(int carID)
        {
            return cbd.Get_Car_BasicByCarID(carID);
        }

        /// <summary>
        /// ȡ��Ʒ�������г���
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public IList<Car_BasicEntity> Get_Car_BasicByCsID(int csID)
        {
            return cbd.Get_Car_BasicByCsID(csID);
        }

        /// <summary>
        /// ��ȡ��������������ĳ�������
        /// </summary>
        /// <returns></returns>
        public DataSet GetCarDataForGoogle()
        {
            string cacheKey = "CarData_For_Google";
            DataSet ds = (DataSet)CacheManager.GetCachedData(cacheKey);
            if (ds == null)
            {
                ds = GetCarDataForGoogleFromDb();
                if (ds != null)
                    CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
            }

            return ds;
        }

        /// <summary>
        /// ��ȡ�����ύ���ͺ�
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public string GetCarNetfriendsFuel(int carId)
        {
            string fuelStr = "��";
            string cacheKey = "all_car_netfriend_fuel";
            Dictionary<int, string> fuelDic = (Dictionary<int, string>)CacheManager.GetCachedData(cacheKey);
            if (fuelDic == null)
            {
                string xmlFile = Path.Combine(WebConfig.DataBlockPath, @"Data/Koubei/AllCarFuelV2.xml");
                try
                {
                    fuelDic = new Dictionary<int, string>();
                    XmlDocument fuelDoc = CommonFunction.ReadXmlFromFile(xmlFile);
                    if (fuelDoc == null) return fuelStr;
                    //XmlDocument fuelDoc = new XmlDocument();
                    //fuelDoc.Load(xmlFile);
                    XmlNodeList fuelList = fuelDoc.SelectNodes("//Trim");
                    foreach (XmlElement fuelNode in fuelList)
                    {
                        int fCarId = Convert.ToInt32(fuelNode.GetAttribute("Id"));
                        // 						double minFuel = Convert.ToDouble(fuelNode.GetAttribute("MinFuel"));
                        // 						double maxFuel = Convert.ToDouble(fuelNode.GetAttribute("MaxFuel"));
                        // 						if (minFuel == 0 && maxFuel == 0)
                        // 							fuelDic[fCarId] = "��";
                        // 						else
                        // 							fuelDic[fCarId] = minFuel.ToString() + "L-" + maxFuel.ToString() + "L";
                        double averageFuel = ConvertHelper.GetDouble(fuelNode.GetAttribute("UserAvgTrimFuel"));
                        if (averageFuel == 0)
                            fuelDic[fCarId] = "��";
                        else
                            fuelDic[fCarId] = averageFuel.ToString();
                    }

                    CacheManager.InsertCache(cacheKey, fuelDic, 60 * 24 * 7);
                }
                catch (Exception ex)
                {
                    CommonFunction.WriteLog(ex.ToString());
                }
            }

            if (fuelDic.ContainsKey(carId))
                fuelStr = fuelDic[carId];

            return fuelStr;
        }

        private DataSet GetCarDataForGoogleFromDb()
        {
            DataSet baseDs = new Car_BasicDal().GetCarData();
            DataSet fuelDs = new PageBase().GetAllCarPerfFuelCostPer100();
            Dictionary<int, XmlElement> imgUrlDic = AutoStorageService.GetImageUrlDic();
            Dictionary<int, string> gotUrlDic = new Dictionary<int, string>();

            DataTable carDataTab = baseDs.Tables[0];
            carDataTab.Columns.Add("PriceUrl");
            carDataTab.Columns.Add("ImageUrl");
            carDataTab.Columns.Add("CarTitle");
            carDataTab.Columns.Add("CarInfo");
            DataTable fuelTable = fuelDs.Tables[0];
            foreach (DataRow row in baseDs.Tables[0].Rows)
            {
                int carId = Convert.ToInt32(row["CarId"]);
                int serialId = ConvertHelper.GetInteger(row["SerialId"]);
                string carName = ConvertHelper.GetString(row["CarName"]).Trim();
                string serialName = ConvertHelper.GetString(row["SerialShowName"]).Trim();
                string producerName = ConvertHelper.GetString(row["ProducerName"]).Trim();
                string carYear = ConvertHelper.GetString(row["CarYear"]).Trim();

                //���ɳ������������
                if (carName.StartsWith(serialName))
                    carName = carName.Substring(serialName.Length);
                carName = serialName + " " + carName;
                if (carYear.Length > 0)
                    carName += " " + carYear + "��";
                row["CarName"] = carName;
                row["CarTitle"] = producerName + " " + carName;

                //���ɱ��۵�ַ
                string serialSpell = ConvertHelper.GetString(row["SeialAllSpell"]);
                row["PriceUrl"] = "http://car.bitauto.com/" + serialSpell + "/m" + carId + "/baojia/";

                //����ͼƬ��ַ
                if (gotUrlDic.ContainsKey(serialId))
                    row["ImageUrl"] = gotUrlDic[serialId];
                else
                {
                    string realUrl = WebConfig.DefaultCarPic;
                    if (imgUrlDic.ContainsKey(serialId))
                    {
                        int imgId = Convert.ToInt32(imgUrlDic[serialId].GetAttribute("ImageId"));
                        string imgUrl = imgUrlDic[serialId].GetAttribute("ImageUrl");
                        if (imgId > 0 && imgUrl.Length > 0)
                            realUrl = new OldPageBase().GetPublishImage(4, imgUrl, imgId);
                    }
                    gotUrlDic[serialId] = realUrl;          //������ʱ�ֵ�
                    row["ImageUrl"] = realUrl;
                }

                //������Ϣ˵��
                //��ȡ�ͺ�
                string perFuel = "";
                DataRow[] rows = fuelTable.Select("carid=" + carId);
                if (rows.Length > 0)
                {
                    DataRow tempRow = rows[0];
                    perFuel = tempRow["pvalue"].ToString().Trim();
                    if (perFuel.Length != 0)
                        perFuel += "L";
                }

                //Ʒ������
                string brandName = ConvertHelper.GetString(row["BrandName"]).Trim();
                string country = ConvertHelper.GetString(row["Cp_Country"]).Trim();
                if (country != "�й�")
                    row["BrandName"] = "����" + brandName;

                string referPrice = ConvertHelper.GetString(row["ReferPrice"]).Trim();
                //string saleState = ConvertHelper.GetString(row["SaleState"]).Trim();
                string exhaust = ConvertHelper.GetString(row["Exhaust"]).Trim();
                string transmissionType = ConvertHelper.GetString(row["TransmissionType"]).Trim();
                string carLevel = ConvertHelper.GetString(row["Carlevel"]).Trim();
                string bodyType = ConvertHelper.GetString(row["BodyType"]).Trim();
                string repairPolicy = ConvertHelper.GetString(row["RepairPolicy"]).Trim();

                string carInfo = "�������ң�" + producerName + "��";
                if (referPrice.Length > 0)
                    carInfo += "����ָ���ۣ�" + referPrice + "��";
                if (exhaust.Length > 0)
                    carInfo += "������" + exhaust + "��";
                if (carLevel.Length > 0)
                    carInfo += "����" + carLevel + "��";
                if (transmissionType.Length > 0)
                    carInfo += "��������" + transmissionType + "��";
                if (bodyType.Length > 0)
                    carInfo += "��ʽ��" + bodyType + "��";
                if (repairPolicy.Length > 0)
                    carInfo += "�ʱ���" + repairPolicy + "��";
                if (perFuel.Length > 0)
                    carInfo += "�ͺģ�" + perFuel;
                else
                    carInfo = carInfo.TrimEnd('��');

                row["CarInfo"] = carInfo;

                //����ָ���۵�λ��ΪԪ
                double refPrice = ConvertHelper.GetDouble(row["ReferPrice"]);
                row["ReferPrice"] = refPrice * 10000;
            }

            //ɾ��������
            carDataTab.Columns.Remove("SaleState");
            carDataTab.Columns.Remove("SerialId");
            carDataTab.Columns.Remove("SerialName");
            carDataTab.Columns.Remove("SeialAllSpell");
            carDataTab.Columns.Remove("SerialShowName");
            carDataTab.Columns.Remove("ProducerName");
            carDataTab.Columns.Remove("CarYear");
            carDataTab.Columns.Remove("Cp_Country");
            //			carDataTab.Columns.Remove("ReferPrice");
            // 			carDataTab.Columns.Remove("Exhaust");
            // 			carDataTab.Columns.Remove("TransmissionType");
            // 			carDataTab.Columns.Remove("Carlevel");
            // 			carDataTab.Columns.Remove("BodyType");
            carDataTab.Columns.Remove("RepairPolicy");
            baseDs.AcceptChanges();

            return baseDs;
        }

        /// <summary>
        /// ȡ����12�ű�׼ͼ �ļ�������
        /// </summary>
        /// <param name="csid"></param>
        /// <param name="carid"></param>
        /// <returns></returns>
        public XmlDocument GetCar12Photo(int csid, int carid)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                //ͼ��ӿڱ��ػ����� by sk 2012.12.21
                string xmlUrl = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.CarStandardImagePath);
                doc.Load(string.Format(xmlUrl, csid, carid));
                //doc.Load(string.Format(WebConfig.CarPhoto12ImageInterface, csid.ToString(), carid.ToString()));
            }
            catch (Exception ex)
            { }
            return doc;
        }

        /// <summary>
        /// ȡ����ÿ������4��ͼƬ
        /// </summary>
        /// <param name="csid"></param>
        /// <param name="carid"></param>
        /// <returns></returns>
        public XmlDocument GetCarSummaryPhoto(int csid, int carid, int subfix)
        {
            XmlDocument doc = new XmlDocument();
            //ͼ��ӿڱ��ػ����� by sk 2012.12.21
            string xmlPicPath = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialDefaultCarImagePath);
            if (File.Exists(string.Format(xmlPicPath, csid, carid)))
                doc.Load(string.Format(xmlPicPath, csid, carid));
            //doc.Load(WebConfig.PhotoCarInterface + "?dataname=caraccountbygroup&serialid=" + csid + "&carid=" + carid + "&showfullurl=true&subfix=" + subfix.ToString());
            return doc;
        }

        /// <summary>
        /// ȡ���ͷ����ֵ����� �����ݻ���
        /// </summary>
        /// <param name="subfix">ͼƬ���</param>
        /// <returns></returns>
        public Dictionary<int, string> GetCarDefaultPhotoDictionary(int subfix)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            string cacheKey = "GetCarDefaultPhotoDictionary_" + subfix.ToString();
            object getCarDefaultPhotoDictionary = CacheManager.GetCachedData(cacheKey);
            if (getCarDefaultPhotoDictionary != null)
            {
                dic = (Dictionary<int, string>)getCarDefaultPhotoDictionary;
            }
            else
            {
                XmlDocument doc = GetCarDefaultPhoto();
                if (doc != null && doc.HasChildNodes)
                {
                    XmlNodeList xnl = doc.SelectNodes("/ImageData/ImageList/ImageInfo");
                    if (xnl != null && xnl.Count > 0)
                    {
                        foreach (XmlNode xn in xnl)
                        {
                            int carid = 0;
                            if (int.TryParse(xn.Attributes["CarId"].Value.ToString(), out carid))
                            {
                                if (carid > 0)
                                {
                                    if (!dic.ContainsKey(carid))
                                    {
                                        string imgUrl = xn.Attributes["ImageUrl"].Value;
                                        if (subfix != 2)
                                            imgUrl = imgUrl.Replace("_2.", string.Format("_{0}.", subfix));
                                        dic.Add(carid, imgUrl);
                                    }
                                }
                            }
                        }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, 60);
            }

            return dic;
        }

        /// <summary>
        /// ȡ���ͷ����ֵ����� �����ݻ���
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, XmlElement> GetCarDefaultPhotoXmlElement()
        {
            Dictionary<int, XmlElement> dic = new Dictionary<int, XmlElement>();

            string cacheKey = "Car_BLL_GetCarDefaultPhotoXmlElement";
            object getCarDefaultPhotoDictionary = CacheManager.GetCachedData(cacheKey);
            if (getCarDefaultPhotoDictionary != null)
            {
                dic = (Dictionary<int, XmlElement>)getCarDefaultPhotoDictionary;
            }
            else
            {
                XmlDocument doc = GetCarDefaultPhoto();
                if (doc != null && doc.HasChildNodes)
                {
                    XmlNodeList itemList = doc.SelectNodes("/ImageData/ImageList/ImageInfo");
                    if (itemList != null && itemList.Count > 0)
                    {
                        foreach (XmlElement item in itemList)
                        {
                            int carID = ConvertHelper.GetInteger(item.Attributes["CarId"].Value);
                            if (!dic.ContainsKey(carID))
                            {
                                dic.Add(carID, item);
                            }
                        }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, 60);
            }
            return dic;
        }
        /// <summary>
        /// ȡ���͵ķ���
        /// </summary>
        /// <param name="subfix">ͼƬ���</param>
        /// <returns></returns>
        public XmlDocument GetCarDefaultPhoto()
        {
            XmlDocument doc = new XmlDocument();
            //ͼ��ӿڱ��ػ����� by sk 2012.12.21
            string xmlFile = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.CarCoverImagePath);
            if (File.Exists(xmlFile))
                doc.Load(xmlFile);
            //doc.Load(WebConfig.PhotoSerialInterface + "?dataname=carcoverimage&showall=false&showfullurl=true&subfix=" + subfix.ToString());
            return doc;
        }

        /// <summary>
        /// ȡ����3�ű�׼ͼ modified by chengl Jun.13.2012 �������
        /// </summary>
        /// <param name="csID">��Ʒ��ID</param>
        /// <param name="carID">����ID</param>
        /// <returns></returns>
        public XmlDocument GetCarDefaultPhoto(int csID, int carID, int year)
        {
            XmlDocument doc = new XmlDocument();
            string xmlFile = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.CarFocusImagePath);
            xmlFile = string.Format(xmlFile, carID);
            if (File.Exists(xmlFile))
                doc.Load(xmlFile);
            return doc;
        }

        public Dictionary<int, string> GetCarPhotoCount()
        {
            var dic = new Dictionary<int, string>();
            const string cacheKey = "GetCarPhotoCountDictionary";
            object getCarDefaultPhotoDictionary = CacheManager.GetCachedData(cacheKey);
            if (getCarDefaultPhotoDictionary != null)
            {
                dic = (Dictionary<int, string>)getCarDefaultPhotoDictionary;
            }
            else
            {
                var doc = new XmlDocument();
                string xmlFile = Path.Combine(PhotoImageConfig.SavePath, "CarImageCount.xml");
                if (File.Exists(xmlFile))
                {
                    doc.Load(xmlFile);
                }
                if (doc.HasChildNodes)
                {
                    XmlNodeList xnl = doc.SelectNodes("/CarModels/Car");
                    if (xnl != null && xnl.Count > 0)
                    {
                        foreach (XmlNode xn in xnl)
                        {
                            int carid = 0;
                            if (int.TryParse(xn.Attributes["CarModelId"].Value, out carid))
                            {
                                if (carid > 0)
                                {
                                    if (!dic.ContainsKey(carid))
                                    {
                                        dic.Add(carid, xn.Attributes["ImageCount"].Value.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
            }
            return dic;
        }

        /// <summary>
        /// ��ȡ����ڱ�����
        /// </summary>
        public XmlDocument GetCarKouBei(int carId)
        {
            //XmlDocument doc = null;
            //string cacheKey = "Car_BasicBll_GetCarKouBei_" + carId.ToString();
            //doc = (XmlDocument)CacheManager.GetCachedData(cacheKey);
            //if (doc == null)
            //{
            XmlDocument doc = new XmlDocument();
            string xmlFile = Path.Combine(WebConfig.DataBlockPath,
                string.Format(@"Data\Koubei\CarTopicList\{0}.xml", carId));
            xmlFile = string.Format(xmlFile, carId);
            if (File.Exists(xmlFile))
                doc.Load(xmlFile);
            //  CacheManager.InsertCache(cacheKey, doc, WebConfig.CachedDuration);
            //doc.Load(WebConfig.PhotoSerialInterface + "?dataname=serialfocusimage&serialid=" + csID + "&cId=" + carID + "&year=" + year + "&showall=false&showfullurl=true");
            //}
            return doc;
        }

        /// <summary>
        /// ȡ���͵ĶԱ��б�
        /// </summary>
        /// <param name="carID"></param>
        /// <returns></returns>
        public DataSet GetCarCompareListByCarID(int carID)
        {
            string cacheKey = "Car_BasicBll_GetCarCompareListByCarID_" + carID.ToString();
            DataSet ds = (DataSet)CacheManager.GetCachedData(cacheKey);
            if (ds == null)
            {
                ds = cbd.GetCarCompareListByCarID(carID);
                CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
            }
            return ds;
        }

        /// <summary>
        /// ָ�������Ƿ��������չ����
        /// </summary>
        /// <param name="carid">����ID</param>
        /// <param name="paramid">����ID</param>
        /// <returns></returns>
        public bool CarHasParamEx(int carid, int paramid)
        {
            bool isHas = false;
            DataSet ds = new DataSet();
            string cacheKey = "GetCarParamEx_" + paramid.ToString();
            object carHasParamEx = CacheManager.GetCachedData(cacheKey);
            if (carHasParamEx != null)
            {
                ds = (DataSet)carHasParamEx;
            }
            else
            {
                ds = GetCarParamEx(paramid);
                CacheManager.InsertCache(cacheKey, ds, 60);
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select("car_id='" + carid.ToString() + "'");
                if (drs != null && drs.Length > 0)
                {
                    if (drs[0]["pvalue"].ToString() == "3000Ԫ")
                    {
                        isHas = true;
                    }
                }
            }
            return isHas;
        }

        /// <summary>
        /// ȡĳ�����͵�ĳ����չ����
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="paramid"></param>
        /// <returns></returns>
        public string GetCarParamEx(int carid, int paramid)
        {
            string carParamEx = "";
            DataSet ds = new DataSet();
            string cacheKey = "GetCarParamEx_" + paramid.ToString();
            object carHasParamEx = CacheManager.GetCachedData(cacheKey);
            if (carHasParamEx != null)
            {
                ds = (DataSet)carHasParamEx;
            }
            else
            {
                ds = GetCarParamEx(paramid);
                CacheManager.InsertCache(cacheKey, ds, 60);
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = ds.Tables[0].Select("car_id='" + carid.ToString() + "'");
                if (drs != null && drs.Length > 0)
                {
                    carParamEx = drs[0]["pvalue"].ToString().Trim();
                }
            }
            return carParamEx;
        }

        /// <summary>
        /// ȡĳ����Ʒ�Ƶ�ĳ����չ����
        /// </summary>
        /// <param name="csid"></param>
        /// <param name="paramid"></param>
        /// <param name="isAllSale"></param>
        /// <param name="orderStr"></param>
        /// <returns></returns>
        public DataRow[] GetCarParamEx(int csid, int paramid, bool isAllSale, string orderStr)
        {
            DataRow[] drs = null;
            DataSet ds = new DataSet();
            string cacheKey = "GetCarParamEx_" + paramid.ToString();
            object carHasParamEx = CacheManager.GetCachedData(cacheKey);
            if (carHasParamEx != null)
            {
                ds = (DataSet)carHasParamEx;
            }
            else
            {
                ds = GetCarParamEx(paramid);
                CacheManager.InsertCache(cacheKey, ds, 60);
            }

            string saleStr = isAllSale ? "" : " and car_SaleState<>'ͣ��' ";

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (orderStr != "")
                { drs = ds.Tables[0].Select("cs_id='" + csid.ToString() + "'" + saleStr, orderStr); }
                else
                { drs = ds.Tables[0].Select("cs_id='" + csid.ToString() + "'" + saleStr); }
            }
            return drs;
        }

        /// <summary>
        /// ���ݲ���IDȡ������չ����
        /// </summary>
        /// <param name="paramid">����ID</param>
        /// <returns></returns>
        public DataSet GetCarParamEx(int paramid)
        {
            return cbd.GetCarParamEx(paramid);
        }

        /// <summary>
        /// ���ݲ���IDȡ������չ���� ���س���ID key ���ֵ�
        /// </summary>
        /// <param name="paramid">����ID</param>
        /// <returns></returns>
        public Dictionary<int, string> GetCarParamExDic(int paramid)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string cacheKey = "Car_BasicBll_GetCarParamExDic_" + paramid.ToString();
            object carHasParamEx = CacheManager.GetCachedData(cacheKey);
            if (carHasParamEx != null)
            {
                dic = (Dictionary<int, string>)carHasParamEx;
            }
            else
            {
                DataSet ds = cbd.GetCarParamEx(paramid);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int carid = int.Parse(dr["car_id"].ToString());
                        string pvalue = dr["pvalue"].ToString().Trim();
                        if (pvalue != "" && !dic.ContainsKey(carid))
                        { dic.Add(carid, pvalue); }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, 60);
            }
            return dic;
        }

        /// <summary>
        ///  ���ݲ���IDȡ������չ���� ���س���ID key ���ֵ� �趨����ʱ��
        /// </summary>
        /// <param name="paramid">����ID</param>
        /// <param name="cacheInterval">����ʱ�� ����</param>
        /// <returns></returns>
        public Dictionary<int, string> GetCarParamExDicByCacheInterval(int paramid, int cacheInterval)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string cacheKey = "Car_BasicBll_GetCarParamExDicByCacheInterval_" + paramid.ToString();
            object carHasParamEx = CacheManager.GetCachedData(cacheKey);
            if (carHasParamEx != null)
            {
                dic = (Dictionary<int, string>)carHasParamEx;
            }
            else
            {
                DataSet ds = cbd.GetCarParamEx(paramid);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int carid = int.Parse(dr["car_id"].ToString());
                        string pvalue = dr["pvalue"].ToString().Trim();
                        if (pvalue != "" && !dic.ContainsKey(carid))
                        { dic.Add(carid, pvalue); }
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, cacheInterval);
            }
            return dic;
        }

        /// <summary>
        /// �õ�CNCAP���������ͺģ� CNCAP�н������ͺģ��׳������ͺĵĲ��Խӿ�,�Դ���
        /// </summary>
        /// <returns></returns>
        public DataSet GetOilMessageByAskInterface()
        {
            string cacheKey = "Car_BasicBll_GetOilMessageByAskInfo";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (DataSet)obj;

            DataSet ds = cbd.GetOilMessageByAskInterface();
            if (ds == null) return null;

            CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
            return ds;
        }

        /// <summary>
        /// ȡ����ȫ��������
        /// </summary>
        /// <param name="carID">����ID</param>
        /// <returns></returns>
        public Dictionary<int, string> GetCarAllParamByCarID(int carID)
        {
            return new Car_BasicDal().GetCarAllParamByCarID(carID);
        }
        /// <summary>
        /// ȡ����ȫ��ѡ�������
        /// </summary>
        /// <param name="carID"></param>
        /// <returns></returns>
        public Dictionary<int, Dictionary<string, double>> GetCarAllParamOptionalByCarID(int carID)
        {
            return new Car_BasicDal().GetCarAllParamOptionalByCarID(carID);
        }
        /// <summary>
        /// ��ȡ�������ֵ
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="paramId"></param>
        /// <returns></returns>
        public string GetCarParamValue(int carId, int paramId)
        {
            string result = string.Empty;
            if (carId <= 0 || paramId <= 0) return result;
            try
            {
                result = cbd.GetCarParamValue(carId, paramId);
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return result;
        }
        /// <summary>
        /// ���ݶ������ ����ֵ
        /// </summary>
        /// <param name="arrCarId">����id����</param>
        /// <param name="paramId"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetCarParamValueByCarIds(int[] arrCarId, int paramId)
        {
            Dictionary<int, string> dictResult = new Dictionary<int, string>();
            if (arrCarId.Length <= 0 || paramId <= 0) return dictResult;
            try
            {
                dictResult = cbd.GetCarParamValueByCarIds(arrCarId, paramId);
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return dictResult;

        }
        /// <summary>
        /// ȡ��Ʒ�Ƴ���
        /// </summary>
        /// <param name="csid">��Ʒ��ID ����0Ϊȡ�ض���Ʒ�� ����0Ϊȡȫ����Ʒ��</param>
        /// <returns></returns>
        public DataSet GetCarListGroupbyYear(int csid, bool isOnlySale)
        {
            return new Car_BasicDal().GetCarListGroupbyYear(csid, isOnlySale);
        }

        /// <summary>
        /// ȡ���͵�ȫ�����о�����
        /// </summary>
        /// <param name="carID"></param>
        /// <returns></returns>
        public DataSet GetCarAllCityPriceDealerByCarID(int carID)
        {
            DataSet ds = new DataSet();
            string cacheName = "GetCarAllCityPriceDealer_" + carID.ToString();
            object carAllCityPriceDealer = null;
            CacheManager.GetCachedData(cacheName, out carAllCityPriceDealer);
            if (carAllCityPriceDealer != null)
            {
                ds = (DataSet)carAllCityPriceDealer;
            }
            else
            {
                com.bitauto.price.DealerPrice dp = new com.bitauto.price.DealerPrice();
                ds = dp.GetDealerPriceList(carID);
                CacheManager.InsertCache(cacheName, ds, 60);
            }
            return ds;
        }

        public List<DealerInfo> GetCarAllCityDealserFromMongoDB(int carId)
        {
            /*
			List<DealerInfo> dealerList = new List<DealerInfo>();
			 */
            string connectionString = WebConfig.MongoDBConnString;
            //string dbName = "DealerPrice";
            MongoServer server = MongoServer.Create(connectionString);
            //MongoCredentials credentials = new MongoCredentials("dealer", "dealer");
            //MongoDatabase database = server.GetDatabase(dbName, credentials);
            MongoDatabase database = server.GetDatabase("DealerPrice");
            /* ���� "dealerlist-" + carId ����
			var dealers = database.GetCollection("dealerlist-" + carId);
			var items = dealers.FindAll().ToArray();
			foreach (BsonDocument d in items)
			{
				DealerInfo dealer = BsonSerializer.Deserialize(d, typeof(DealerInfo)) as DealerInfo;
				dealerList.Add(dealer);
			}
			*/

            var dealers = database.GetCollection("cardealerlist");
            if (dealers != null)
            {
                CarDealerList carDealerList = dealers.FindOneAs(typeof(CarDealerList), MongoDB.Driver.Builders.Query.EQ("CarId", carId)) as CarDealerList;
                if (carDealerList != null)
                {
                    return carDealerList.Dealers;
                }
            }

            return new List<DealerInfo>();

        }

        #region ��������ҳ ���Ͳ���

        /// <summary>
        /// ȡ��������ҳ��������
        /// </summary>
        /// <param name="carID">����</param>
        /// <param name="name">��ʾ������</param>
        /// <param name="allSpell">��ַʹ��</param>
        /// <returns></returns>
        public string GetCarConfigurationForCarSummaey(int carID, string name, string allSpell)
        {
            string result = "";
            StringBuilder sbParameter = new StringBuilder(5000);
            StringBuilder sbConfiguration = new StringBuilder(5000);
            StringBuilder sbTemp = new StringBuilder(500);
            // StringBuilder sbForPage = new StringBuilder();
            // ����XML
            // XmlDocument docCar = new XmlDocument();
            List<int> listValidCarID = new List<int>();
            listValidCarID.Add(carID);
            Dictionary<int, Dictionary<string, string>> dic = GetCarCompareDataByCarIDs(listValidCarID);
            if (!dic.ContainsKey(carID) || dic[carID].Count == 0)
            { return ""; }
            else
            {
                XmlDocument docPC = new XmlDocument();
                string cache = "CarSummary_ParameterConfiguration";
                object parameterConfiguration = null;
                CacheManager.GetCachedData(cache, out parameterConfiguration);
                if (parameterConfiguration != null)
                {
                    docPC = (XmlDocument)parameterConfiguration;
                }
                else
                {
                    if (File.Exists(System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterConfiguration.config"))
                    {
                        docPC.Load(System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterConfiguration.config");
                        CacheManager.InsertCache(cache, docPC, 60);
                    }
                }

                // ��������
                if (docPC != null && docPC.HasChildNodes)
                {
                    XmlNode rootPC = docPC.DocumentElement;
                    // ��ʾ ����
                    if (docPC.ChildNodes.Count > 1)
                    {
                        sbParameter.AppendLine("<div id=\"DicCarParameter\" class=\"line_box car_config\">");
                        sbParameter.AppendLine("<h3><span>" + name + " ����</span><!--<i>ע������� ��ѡ�� -��</i>--></h3>");
                        sbParameter.AppendLine("<table cellspacing=\"0\" cellpadding=\"0\">");
                        XmlNode parameter = rootPC.ChildNodes[0];
                        foreach (XmlNode parameterList in parameter)
                        {
                            if (parameterList.NodeType == XmlNodeType.Element)
                            {
                                if (parameterList.HasChildNodes && parameterList.Attributes.GetNamedItem("Type").Value.ToString() == "2")
                                {
                                    sbTemp.AppendLine("<thead><tr><th colspan=\"4\">" + parameterList.Attributes.GetNamedItem("Name").Value + "</th></tr></thead><tbody>");
                                    bool isHasChild = false;
                                    int loopCount = 0;
                                    XmlNodeList xmlNode = parameterList.ChildNodes;
                                    foreach (XmlNode item in xmlNode)
                                    {
                                        if (item.NodeType != XmlNodeType.Element)
                                        { continue; }
                                        if (dic[carID].ContainsKey(item.Attributes.GetNamedItem("Value").Value)
                                            && dic[carID][item.Attributes.GetNamedItem("Value").Value] != "����")
                                        {
                                            isHasChild = true || isHasChild;
                                            if (loopCount % 2 == 0)
                                            {
                                                if (loopCount != 0)
                                                {
                                                    sbTemp.AppendLine("</tr>");
                                                }
                                                sbTemp.AppendLine("<tr>");
                                            }
                                            string pvalue = dic[carID][item.Attributes.GetNamedItem("Value").Value] + item.Attributes.GetNamedItem("Unit").Value;
                                            // ţB���߼���Ӳ���붼����
                                            // ȼ������ ���͵Ļ�ͬʱ��ʾ ȼ�ͱ��
                                            string pvalueOther;
                                            if (item.Attributes.GetNamedItem("ParamID").Value == "578"
                                                && pvalue == "����")
                                            {
                                                if (dic[carID].ContainsKey("CarParams/Oil_FuelTab")
                                            && dic[carID]["CarParams/Oil_FuelTab"] != "����")
                                                {
                                                    pvalueOther = dic[carID]["CarParams/Oil_FuelTab"];
                                                    switch (pvalueOther)
                                                    {
                                                        case "90��": pvalueOther = pvalueOther + "(����89��)"; break;
                                                        case "93��": pvalueOther = pvalueOther + "(����92��)"; break;
                                                        case "97��": pvalueOther = pvalueOther + "(����95��)"; break;
                                                        default: break;
                                                    }
                                                    pvalue = pvalue + " " + pvalueOther;
                                                }
                                            }
                                            // ������ʽ �����Ȼ����ֱ����ʾ���������ѹ����ʾ��ѹ��ʽ
                                            if (item.Attributes.GetNamedItem("ParamID").Value == "425"
                                                && pvalue == "��ѹ")
                                            {
                                                if (dic[carID].ContainsKey("CarParams/Engine_AddPressType")
                                            && dic[carID]["CarParams/Engine_AddPressType"] != "����")
                                                {
                                                    pvalueOther = dic[carID]["CarParams/Engine_AddPressType"];
                                                    pvalue = pvalue + " " + pvalueOther;
                                                }
                                            }

                                            if (pvalue.IndexOf("��") == 0)
                                            { pvalue = "��"; }
                                            if (pvalue.IndexOf("ѡ��") == 0)
                                            { pvalue = "��"; }
                                            if (pvalue.IndexOf("��") == 0)
                                            { pvalue = "-"; }
                                            sbTemp.AppendLine("<th>" + item.Attributes.GetNamedItem("Name").Value + "</th>");
                                            sbTemp.AppendLine("<td>" + pvalue + "</td>");
                                            loopCount++;
                                        }
                                    }
                                    if (loopCount % 2 == 1)
                                    {
                                        sbTemp.AppendLine("<th></th>");
                                        sbTemp.AppendLine("<td></td>");
                                    }
                                    // ���������
                                    if (isHasChild)
                                    {
                                        sbParameter.AppendLine(sbTemp.ToString() + "</tr></tbody>");
                                    }
                                    if (sbTemp.Length > 0)
                                    { sbTemp.Remove(0, sbTemp.Length); }
                                }
                            }
                        }
                        sbParameter.AppendLine("</table>");
                        sbParameter.AppendLine("<div class=\"more\"><a href=\"/" + allSpell + "/m" + carID.ToString() + "/peizhi/\" target=\"_blank\">�ԱȲ鿴&gt;&gt;</a></div>");
                        sbParameter.AppendLine("</div>");
                    }

                    // ��ʾ����
                    if (docPC.ChildNodes.Count > 1)
                    {
                        sbConfiguration.Append("<div class=\"line_box car_config\">");
                        sbConfiguration.Append("<h3><span>" + name + " ����</span><i>ע������� ��ѡ�� -��</i></h3>");
                        sbConfiguration.Append("<table cellspacing=\"0\" cellpadding=\"0\">");
                        XmlNode parameter = rootPC.ChildNodes[1];
                        foreach (XmlNode parameterList in parameter)
                        {
                            if (parameterList.NodeType == XmlNodeType.Element)
                            {
                                // string block = "";
                                if (parameterList.HasChildNodes && parameterList.Attributes.GetNamedItem("Type").Value.ToString() == "2")
                                {
                                    sbTemp.AppendLine("<thead><tr><th colspan=\"4\">" + parameterList.Attributes.GetNamedItem("Name").Value + "</th></tr></thead><tbody>");
                                    bool isHasChild = false;
                                    int loopCount = 0;
                                    XmlNodeList xmlNode = parameterList.ChildNodes;
                                    foreach (XmlNode item in xmlNode)
                                    {
                                        if (item.NodeType != XmlNodeType.Element)
                                        { continue; }
                                        if (dic[carID].ContainsKey(item.Attributes.GetNamedItem("Value").Value)
                                            && dic[carID][item.Attributes.GetNamedItem("Value").Value] != "����")
                                        {
                                            isHasChild = true || isHasChild;
                                            if (loopCount % 2 == 0)
                                            {
                                                if (loopCount != 0)
                                                {
                                                    sbTemp.AppendLine("</tr>");
                                                }
                                                sbTemp.AppendLine("<tr>");
                                            }

                                            string pvalue = dic[carID][item.Attributes.GetNamedItem("Value").Value] + item.Attributes.GetNamedItem("Unit").Value;
                                            if (pvalue.IndexOf("��") == 0)
                                            { pvalue = "��"; }
                                            if (pvalue.IndexOf("ѡ��") == 0)
                                            { pvalue = "��"; }
                                            if (pvalue.IndexOf("��") == 0)
                                            { pvalue = "-"; }

                                            sbTemp.AppendLine("<th>" + item.Attributes.GetNamedItem("Name").Value + "</th>");
                                            // ������ɫ�������⻯
                                            if (item.Attributes.GetNamedItem("Name").Value == "������ɫ")
                                            {
                                                sbTemp.AppendLine("<td colspan=\"3\"><span class=\"c w530\"><!--������ɫ--></span></td>");
                                                loopCount++;
                                            }
                                            else
                                            {
                                                sbTemp.AppendLine("<td>" + pvalue + "</td>");
                                            }
                                            loopCount++;
                                        }
                                    }
                                    if (loopCount % 2 == 1)
                                    {
                                        sbTemp.AppendLine("<th></th>");
                                        sbTemp.AppendLine("<td></td>");
                                    }
                                    // ���������
                                    if (isHasChild)
                                    {
                                        sbConfiguration.AppendLine(sbTemp.ToString() + "</tr></tbody>");
                                    }
                                    if (sbTemp.Length > 0)
                                    { sbTemp.Remove(0, sbTemp.Length); }
                                }
                            }
                        }
                        sbConfiguration.AppendLine("</table>");
                        sbConfiguration.AppendLine("<div class=\"more\"><a href=\"/" + allSpell + "/m" + carID.ToString() + "/peizhi/\" target=\"_blank\">�ԱȲ鿴&gt;&gt;</a></div>");
                        sbConfiguration.AppendLine("</div>");
                    }

                    result = sbParameter.ToString() + sbConfiguration.ToString();
                }
            }
            return result;
        }

        #endregion

        #region ���ͶԱ�

        /// <summary>
        /// ȡ���Ͳ���
        /// </summary>
        /// <param name="carIDs"></param>
        /// <returns></returns>
        public DataSet GetCarParamForCompare(string carIDs)
        {
            return new Car_BasicDal().GetCarParamForCompare(carIDs);
        }

        /// <summary>
        /// ��ȡ����ѡ�����
        /// </summary>
        /// <param name="carIDs"></param>
        /// <returns></returns>
        public DataSet GetCarOptionalForCompare(string carIDs)
        {
            return cbd.GetCarOptionalForCompare(carIDs);
        }

        /// <summary>
        /// ȡ���в���ID��Ӣ�������ڱ�
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllParamAliasName()
        {
            return new Car_BasicDal().GetAllParamAliasName();
        }

        /// <summary>
        /// ȡ���в���ID��Ӣ�������ڱ�
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllParamAliasNameDictionary()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            DataSet ds = new DataSet();
            string cacheName = "GetAllParamAliasNameDictionary";
            object getAllParamAliasNameDictionary = null;
            CacheManager.GetCachedData(cacheName, out getAllParamAliasNameDictionary);
            if (getAllParamAliasNameDictionary != null)
            {
                dic = (Dictionary<int, string>)getAllParamAliasNameDictionary;
            }
            else
            {
                ds = GetAllParamAliasName();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int pid = int.Parse(dr["ParamId"].ToString());
                        string aliasName = dr["AliasName"].ToString().Trim();
                        if (!dic.ContainsKey(pid))
                        { dic.Add(pid, aliasName); }
                    }
                }
                CacheManager.InsertCache(cacheName, dic, WebConfig.CachedDuration);
            }
            return dic;
        }

        /// <summary>
        /// ȡ���ͻ�����Ϣ
        /// </summary>
        /// <param name="carids"></param>
        /// <returns></returns>
        public DataSet GetCarBaseInfoForCompare(string carIDs)
        {
            return new Car_BasicDal().GetCarBaseInfoForCompare(carIDs);
        }

        /// <summary>
        /// ������Ʒ��ID ȡ���³��ͼ�PV
        /// </summary>
        /// <param name="csIDs"></param>
        /// <returns></returns>
        public DataSet GetCarBaseInfoForCompareByCsIDs(string csIDs)
        {
            return new Car_BasicDal().GetCarBaseInfoForCompareByCsIDs(csIDs);
        }

        /// <summary>
        /// ���ݳ���ID�б�ȡ���ͶԱ�����
        /// </summary>
        /// <param name="listCarID">����ID�б�</param>
        /// <returns></returns>
        public Dictionary<int, Dictionary<string, string>> GetCarCompareDataByCarIDs(List<int> listCarID)
        {
            Dictionary<int, Dictionary<string, string>> dic = new Dictionary<int, Dictionary<string, string>>();
            if (listCarID.Count > 0)
            {
                string keyTemp = "Car_Dictionary_CarCompareData_{0}";
                IList<string> keyForMemCache = new List<string>();
                foreach (int carid in listCarID)
                {
                    if (!keyForMemCache.Contains(string.Format(keyTemp, carid)))
                    { keyForMemCache.Add(string.Format(keyTemp, carid)); }
                }

                IDictionary<string, object> dicMemCache = MemCache.GetMultipleMemCacheByKey(keyForMemCache);
                // Hashtable ht = MemCache.GetMultipleMemCacheByKey(keyForMemCache);
                // ����û��memcache����ĳ���
                foreach (int carid in listCarID)
                {
                    if (dicMemCache.Count > 0
                        && dicMemCache.ContainsKey(string.Format(keyTemp, carid))
                        && dicMemCache[string.Format(keyTemp, carid)] != null
                        )
                    {
                        // ��memcache
                        Dictionary<string, string> dicCar = dicMemCache[string.Format(keyTemp, carid)] as Dictionary<string, string>;
                        if (dicCar != null && !dic.ContainsKey(carid))
                        { dic.Add(carid, dicCar); }
                    }
                    else
                    {
                        // modified Jan.13.2012 by chengl ��û��memcacheʱȡ�����ؽ�memcache ����ʱ��1��
                        Dictionary<string, string> dicCar = new Dictionary<string, string>();
                        GetCarInfoAndParamToDictionary(carid, ref dicCar, false);
                        if (dicCar != null && dicCar.Count > 0)
                        {
                            //modified by sk mem 2Сʱ
                            MemCache.SetMemCacheByKey(string.Format(keyTemp, carid), dicCar, 1000 * 60 * 60 * 2);
                        }
                        if (!dic.ContainsKey(carid) && dicCar.Count > 0)
                        { dic.Add(carid, dicCar); }
                    }
                }
            }
            return dic;
        }

        /// <summary>
		/// ���ݳ���ID�б�ȡ���ͶԱ�����
		/// </summary>
		/// <param name="listCarID">����ID�б�</param>
		/// <returns></returns>
		public Dictionary<int, Dictionary<string, string>> GetCarCompareDataWithOptionalByCarIDs(List<int> listCarID)
        {
            Dictionary<int, Dictionary<string, string>> dic = new Dictionary<int, Dictionary<string, string>>();
            if (listCarID.Count > 0)
            {
                string keyTemp = "Car_Dictionary_CarCompareDataWithOptional_{0}";
                IList<string> keyForMemCache = new List<string>();
                foreach (int carid in listCarID)
                {
                    if (!keyForMemCache.Contains(string.Format(keyTemp, carid)))
                    { keyForMemCache.Add(string.Format(keyTemp, carid)); }
                }

                IDictionary<string, object> dicMemCache = MemCache.GetMultipleMemCacheByKey(keyForMemCache);
                // Hashtable ht = MemCache.GetMultipleMemCacheByKey(keyForMemCache);
                // ����û��memcache����ĳ���
                foreach (int carid in listCarID)
                {
                    if (dicMemCache.Count > 0
                        && dicMemCache.ContainsKey(string.Format(keyTemp, carid))
                        && dicMemCache[string.Format(keyTemp, carid)] != null
                        )
                    {
                        // ��memcache
                        Dictionary<string, string> dicCar = dicMemCache[string.Format(keyTemp, carid)] as Dictionary<string, string>;
                        if (dicCar != null && !dic.ContainsKey(carid))
                        { dic.Add(carid, dicCar); }
                    }
                    else
                    {
                        // modified Jan.13.2012 by chengl ��û��memcacheʱȡ�����ؽ�memcache ����ʱ��1��
                        Dictionary<string, string> dicCar = new Dictionary<string, string>();
                        GetCarInfoAndParamToDictionary(carid, ref dicCar, true);
                        if (dicCar != null && dicCar.Count > 0)
                        {
                            //modified by sk mem 2Сʱ
                            MemCache.SetMemCacheByKey(string.Format(keyTemp, carid), dicCar, 1000 * 60 * 60 * 2);
                        }
                        if (!dic.ContainsKey(carid) && dicCar.Count > 0)
                        { dic.Add(carid, dicCar); }
                    }
                }
            }
            return dic;
        }

        /// <summary>
        /// �������json
        /// </summary>
        /// <returns></returns>
        public string GetValidCarJsObject(List<int> carIdList)
        {
            if (carIdList == null || carIdList.Count == 0) return string.Empty;

            StringBuilder sbForApi = new StringBuilder();
            Dictionary<int, Dictionary<string, string>> dicCarParam = GetCarCompareDataWithOptionalByCarIDs(carIdList);
            Dictionary<int, List<string>> dicTemp = new Common.PageBase().GetCarParameterJsonConfigNewV2();
            if (dicTemp != null && dicTemp.Count > 0)
            {
                int loopCar = 0;
                foreach (KeyValuePair<int, Dictionary<string, string>> kvpCar in dicCarParam)
                {
                    if (loopCar > 0)
                    { sbForApi.Append(","); }

                    sbForApi.Append("[");
                    // ѭ��ģ��
                    foreach (KeyValuePair<int, List<string>> kvpTemp in dicTemp)
                    {
                        if (kvpTemp.Key == 0)
                        {
                            // ��������
                            sbForApi.Append("[\"" + kvpCar.Value["Car_ID"] + "\"");
                            sbForApi.Append(",\"" + kvpCar.Value["Car_Name"].Replace("\"", "'") + "\"");
                            foreach (string param in kvpTemp.Value)
                            {
                                if (kvpCar.Value.ContainsKey(param))
                                { sbForApi.Append(",\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
                                else
                                { sbForApi.Append(",\"\""); }
                            }
                            sbForApi.Append("]");
                        }
                        else
                        {
                            // ��չ����
                            sbForApi.Append(",[");
                            int loop = 0;
                            foreach (string param in kvpTemp.Value)
                            {
                                if (loop > 0)
                                { sbForApi.Append(","); }
                                if (kvpCar.Value.ContainsKey(param))
                                { sbForApi.Append("\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
                                else
                                { sbForApi.Append("\"\""); }
                                loop++;
                            }
                            sbForApi.Append("]");
                        }
                    }
                    sbForApi.Append("]");

                    loopCar++;
                }
            }
            if (sbForApi.Length > 0)
            {
                sbForApi.Insert(0, "[");
                sbForApi.Append("];");
            }
            return sbForApi.ToString();
        }

        ///// <summary>
        ///// ȡ���ͶԱ����� �ֵ�
        ///// </summary>
        ///// <param name="carID"></param>
        ///// <param name="dic"></param>
        //private void GetCarInfoAndParamToDictionary(int carID, ref Dictionary<string, string> dic)
        //{
        //	Dictionary<int, string> dicCarPhoto = GetCarDefaultPhotoDictionary(2);
        //	PageBase page = new PageBase();
        //	Dictionary<int, string> dicCsPhoto = page.GetAllSerialPicURL(false);
        //	Dictionary<int, string> dicCarPrice = page.GetAllCarPriceRange();
        //	// ��������� add by chengl Aug.27.2012
        //	Dictionary<int, string> dicCarHangQingPrice = new HangQingTree().GetAllCarHangQingPrice();
        //	// ��Ʒ�Ƴ�����ɫRGB
        //	Dictionary<int, Dictionary<string, string>> dicSerialColor = new Car_SerialBll().GetAllSerialColorNameRGB();
        //	// ���ͽ���
        //	Dictionary<int, string> dicJiangJia = new CarNewsBll().GetAllCarJiangJia();

        //	#region ���ͻ�������
        //	CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carID);
        //	if (ce == null || ce.Id <= 0)
        //	{ return; }

        //	string carReferPrice = ce.ReferPrice <= 0 ? "��" : (decimal.Parse(ce.ReferPrice.ToString())).ToString("F2") + "��";
        //	string carYearType = ce.CarYear <= 0 ? "" : ce.CarYear.ToString();
        //	string bbsURL = new Car_SerialBll().GetForumUrlBySerialId(ce.SerialId);
        //	// ���������ͺ�
        //	string userFuel = new Car_BasicBll().GetCarNetfriendsFuel(carID);
        //	userFuel = (userFuel == "��" ? "" : userFuel);
        //	// ���ͱ�������
        //	string carPriceRange = dicCarPrice.ContainsKey(carID) ? dicCarPrice[carID] : "��";
        //	// ����ͼƬ �ȼ�鳵���Ƿ��з��棬�ټ����Ʒ�Ʒ���
        //	string carPic = WebConfig.DefaultCarPic;
        //	if (dicCarPhoto.ContainsKey(carID))
        //	{ carPic = dicCarPhoto[carID]; }
        //	else if (dicCsPhoto.ContainsKey(ce.SerialId))
        //	{ carPic = dicCsPhoto[ce.SerialId]; }
        //	else
        //	{ carPic = WebConfig.DefaultCarPic; }
        //	// ���������
        //	string carHangQingPrice = "";
        //	if (dicCarHangQingPrice.ContainsKey(carID))
        //	{ carHangQingPrice = dicCarHangQingPrice[carID]; }
        //	// add by chengl Mar.25.2013
        //	string carJiangJiaPrice = "";
        //	if (dicJiangJia.ContainsKey(carID))
        //	{ carJiangJiaPrice = dicJiangJia[carID]; }

        //	dic.Add("Car_ID", carID.ToString());
        //	dic.Add("Car_Name", ce.Name);
        //	dic.Add("CarImg", carPic);
        //	dic.Add("Cs_ID", ce.SerialId.ToString());
        //	dic.Add("Cs_Name", ce.Serial == null ? "" : ce.Serial.Name);
        //	dic.Add("Cs_ShowName", ce.Serial == null ? "" : ce.Serial.ShowName);
        //	dic.Add("Cs_AllSpell", ce.Serial == null ? "" : ce.Serial.AllSpell);
        //	dic.Add("Car_YearType", ce.CarYear.ToString());
        //	dic.Add("Car_ProduceState", ce.ProduceState);
        //	dic.Add("Car_SaleState", ce.SaleState);
        //	dic.Add("CarReferPrice", carReferPrice);
        //	dic.Add("AveragePrice", carPriceRange);
        //	dic.Add("Car_UserFuel", userFuel);
        //	dic.Add("Cs_BBSUrl", bbsURL);
        //	dic.Add("Cs_CarLevel", (ce.Serial == null || ce.Serial.Level == null) ? "" : ce.Serial.Level.Name);
        //	// ���������
        //	dic.Add("Car_HangQingPrice", carHangQingPrice);
        //	dic.Add("Car_JiangJiaPrice", carJiangJiaPrice);
        //	#endregion

        //	// ���ͳ�����ɫ������
        //	string bodyColor = string.Empty;

        //	#region ������չ����
        //	// ����ID ���� ��
        //	Dictionary<int, string> dicParamIDToName = GetAllParamAliasNameDictionary();

        //	// ������չ����
        //	DataSet dsParam = new Car_BasicBll().GetCarParamForCompare(carID.ToString());
        //	if (dsParam != null && dsParam.Tables.Count > 0 && dsParam.Tables[0].Rows.Count > 0)
        //	{
        //		foreach (DataRow dr in dsParam.Tables[0].Rows)
        //		{
        //			int carid = Convert.ToInt32(dr["CarId"]);
        //			int pid = Convert.ToInt32(dr["Paramid"]);
        //			string aliasName = string.Empty;
        //			if (dicParamIDToName.ContainsKey(pid))
        //			{ aliasName = dicParamIDToName[pid]; }
        //			else { continue; }
        //			string pvalue = dr["Pvalue"].ToString().Trim();

        //			if (pvalue == "")
        //			{ continue; }

        //			if (!dic.ContainsKey(aliasName))
        //			{
        //				dic.Add(aliasName, pvalue);
        //			}
        //			// ����ǳ�����ɫ
        //			if (aliasName == "OutStat_BodyColor")
        //			{ bodyColor = pvalue; }
        //		}
        //	}
        //          #endregion

        //          #region ���ͳ�����ɫRGBֵ

        //          List<string> listBodyColorRGB = new List<string>();
        //	if (!string.IsNullOrEmpty(bodyColor))
        //	{
        //		if (dicSerialColor.ContainsKey(ce.SerialId))
        //		{
        //			// ��ʱ���Ͳ�����ɫ��
        //			List<string> listTemp = new List<string>();
        //			string[] colorNameArray = bodyColor.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //			if (colorNameArray.Length > 0)
        //			{
        //				foreach (string name in colorNameArray)
        //				{
        //					string colorName = name.Trim();
        //					if (colorName != "" && !listTemp.Contains(colorName))
        //					{
        //						listTemp.Add(colorName);
        //					}
        //				}
        //			}
        //			if (listTemp.Count > 0)
        //			{
        //				foreach (KeyValuePair<string, string> kvp in dicSerialColor[ce.SerialId])
        //				{
        //					if (listTemp.Contains(kvp.Key))
        //					{
        //						if (listBodyColorRGB.Count > 0)
        //						{ listBodyColorRGB.Add("|"); }
        //						listBodyColorRGB.Add(kvp.Key + "," + kvp.Value);
        //					}
        //				}
        //			}
        //		}
        //	}

        //	dic.Add("Car_OutStat_BodyColorRGB", string.Concat(listBodyColorRGB.ToArray()));

        //	#endregion
        //}

        /// <summary>
        /// ȡ���ͶԱ����� �ֵ�
        /// </summary>
        /// <param name="carID"></param>
        /// <param name="dic"></param>
        /// <param name="isOptional">�Ƿ����ѡװ</param>
        private void GetCarInfoAndParamToDictionary(int carID, ref Dictionary<string, string> dic, bool isOptional)
        {
            Dictionary<int, string> dicCarPhoto = GetCarDefaultPhotoDictionary(2);
            PageBase page = new PageBase();
            Dictionary<int, string> dicCsPhoto = page.GetAllSerialPicURL(false);
            Dictionary<int, string> dicCarPrice = page.GetAllCarPriceRange();
            // ��������� add by chengl Aug.27.2012
            Dictionary<int, string> dicCarHangQingPrice = new HangQingTree().GetAllCarHangQingPrice();
            // ��Ʒ�Ƴ�����ɫRGB
            Dictionary<int, Dictionary<string, string>> dicSerialColor = new Car_SerialBll().GetAllSerialColorNameRGB();
            // ���ͽ���
            Dictionary<int, string> dicJiangJia = new CarNewsBll().GetAllCarJiangJia();

            #region ���ͻ�������
            CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carID);
            if (ce == null || ce.Id <= 0)
            { return; }

            string carReferPrice = ce.ReferPrice <= 0 ? "��" : (decimal.Parse(ce.ReferPrice.ToString())).ToString("F2") + "��";
            string carYearType = ce.CarYear <= 0 ? "" : ce.CarYear.ToString();
            string bbsURL = new Car_SerialBll().GetForumUrlBySerialId(ce.SerialId);
            // ���������ͺ�
            string userFuel = new Car_BasicBll().GetCarNetfriendsFuel(carID);
            userFuel = (userFuel == "��" ? "" : userFuel);
            // ���ͱ�������
            string carPriceRange = dicCarPrice.ContainsKey(carID) ? dicCarPrice[carID] : "��";
            // ����ͼƬ �ȼ�鳵���Ƿ��з��棬�ټ����Ʒ�Ʒ���
            string carPic = WebConfig.DefaultCarPic;
            if (dicCarPhoto.ContainsKey(carID))
            { carPic = dicCarPhoto[carID]; }
            else if (dicCsPhoto.ContainsKey(ce.SerialId))
            { carPic = dicCsPhoto[ce.SerialId]; }
            else
            { carPic = WebConfig.DefaultCarPic; }
            // ���������
            string carHangQingPrice = "";
            if (dicCarHangQingPrice.ContainsKey(carID))
            { carHangQingPrice = dicCarHangQingPrice[carID]; }
            // add by chengl Mar.25.2013
            string carJiangJiaPrice = "";
            if (dicJiangJia.ContainsKey(carID))
            { carJiangJiaPrice = dicJiangJia[carID]; }

            dic.Add("Car_ID", carID.ToString());
            dic.Add("Car_Name", ce.Name);
            dic.Add("CarImg", carPic);
            dic.Add("Cs_ID", ce.SerialId.ToString());
            dic.Add("Cs_Name", ce.Serial == null ? "" : ce.Serial.Name);
            dic.Add("Cs_ShowName", ce.Serial == null ? "" : ce.Serial.ShowName);
            dic.Add("Cs_AllSpell", ce.Serial == null ? "" : ce.Serial.AllSpell);
            dic.Add("Car_YearType", ce.CarYear.ToString());
            dic.Add("Car_ProduceState", ce.ProduceState);
            dic.Add("Car_SaleState", ce.SaleState);
            dic.Add("CarReferPrice", carReferPrice);
            dic.Add("AveragePrice", carPriceRange);
            dic.Add("Car_UserFuel", userFuel);
            dic.Add("Cs_BBSUrl", bbsURL);
            dic.Add("Cs_CarLevel", (ce.Serial == null || ce.Serial.Level == null) ? "" : ce.Serial.Level.Name);
            // ���������
            dic.Add("Car_HangQingPrice", carHangQingPrice);
            dic.Add("Car_JiangJiaPrice", carJiangJiaPrice);
            #endregion

            // ���ͳ�����ɫ������
            string bodyColor = string.Empty;

            #region ������չ����
            // ����ID ���� ��
            Dictionary<int, string> dicParamIDToName = GetAllParamAliasNameDictionary();

            // ������չ����
            DataSet dsParam = new Car_BasicBll().GetCarParamForCompare(carID.ToString());
            if (dsParam != null && dsParam.Tables.Count > 0 && dsParam.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsParam.Tables[0].Rows)
                {
                    int carid = Convert.ToInt32(dr["CarId"]);
                    int pid = Convert.ToInt32(dr["Paramid"]);
                    string aliasName = string.Empty;
                    if (dicParamIDToName.ContainsKey(pid))
                    { aliasName = dicParamIDToName[pid]; }
                    else { continue; }
                    string pvalue = dr["Pvalue"].ToString().Trim();

                    if (pvalue == "")
                    { continue; }

                    if (!dic.ContainsKey(aliasName))
                    {
                        dic.Add(aliasName, pvalue);
                    }
                    // ����ǳ�����ɫ
                    if (aliasName == "OutStat_BodyColor")
                    { bodyColor = pvalue; }
                }
            }
            if (isOptional)
            {
                DataSet dsOptional = GetCarOptionalForCompare(carID.ToString());
                if (dsOptional != null && dsOptional.Tables.Count > 0 && dsOptional.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsOptional.Tables[0].Rows)
                    {
                        int carid = Convert.ToInt32(dr["CarId"]);
                        int pid = Convert.ToInt32(dr["Paramid"]);
                        string aliasName = string.Empty;
                        if (dicParamIDToName.ContainsKey(pid))
                        { aliasName = dicParamIDToName[pid]; }
                        else { continue; }
                        string pvalue = dr["Pvalue"].ToString().Trim();
                        float price = Convert.ToSingle(dr["Price"]);

                        if (pvalue == "" || price == 0)
                        { continue; }

                        if (!dic.ContainsKey(aliasName))
                        {
                            dic.Add(aliasName, string.Format("{0}|{1}", pvalue, price));
                        }
                        else
                        {
                            if (dic[aliasName] == "ѡ��" && pvalue == "ѡ��")
                            {
                                dic[aliasName] = string.Format("{0}|{1}", pvalue, price);
                            }
                            else
                            {
                                dic[aliasName] = string.Format("{0},{1}|{2}", dic[aliasName], pvalue, price);
                            }
                        }
                    }
                }
            }
            #endregion

            #region ���ͳ�����ɫRGBֵ

            List<string> listBodyColorRGB = new List<string>();
            if (!string.IsNullOrEmpty(bodyColor))
            {
                if (dicSerialColor.ContainsKey(ce.SerialId))
                {
                    // ��ʱ���Ͳ�����ɫ��
                    List<string> listTemp = new List<string>();
                    string[] colorNameArray = bodyColor.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (colorNameArray.Length > 0)
                    {
                        foreach (string name in colorNameArray)
                        {
                            string colorName = name.Trim();
                            if (colorName != "" && !listTemp.Contains(colorName))
                            {
                                listTemp.Add(colorName);
                            }
                        }
                    }
                    if (listTemp.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> kvp in dicSerialColor[ce.SerialId])
                        {
                            if (listTemp.Contains(kvp.Key))
                            {
                                if (listBodyColorRGB.Count > 0)
                                { listBodyColorRGB.Add("|"); }
                                listBodyColorRGB.Add(kvp.Key + "," + kvp.Value);
                            }
                        }
                    }
                }
            }

            dic.Add("Car_OutStat_BodyColorRGB", string.Concat(listBodyColorRGB.ToArray()));

            #endregion

        }

        #endregion

        #region ���ֳ�

        /// <summary>
        /// ȡ���г��͵Ķ��ֳ�����
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllUcarPrice()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string cacheName = "Car_BasicBll_GetAllUcarPrice";
            object getAllUcarPrice = null;
            CacheManager.GetCachedData(cacheName, out getAllUcarPrice);
            if (getAllUcarPrice != null)
            {
                dic = (Dictionary<int, string>)getAllUcarPrice;
            }
            else
            {
                string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\UsedCarInfo\\AllUCarPrice.Xml");
                if (File.Exists(xmlFile))
                {
                    using (XmlReader xmlReader = XmlReader.Create(xmlFile))
                    {
                        // modified by chengl Mar.26.2013 fix bug when file empty
                        try
                        {
                            while (xmlReader.ReadToFollowing("ds"))
                            {
                                XmlReader inner = xmlReader.ReadSubtree();
                                int carid = 0;
                                decimal minP = 0;
                                decimal maxP = 0;
                                while (!inner.EOF)
                                {
                                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "MinPrice")
                                    {
                                        decimal.TryParse(inner.ReadString(), out minP);
                                        if (minP > 0)
                                        { minP = Math.Round(minP, 2); }
                                        if (minP > 100)
                                        {
                                            minP = Math.Round(minP, 0);
                                        }
                                    }
                                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "MaxPrice")
                                    {
                                        decimal.TryParse(inner.ReadString(), out maxP);
                                        if (maxP > 0)
                                        { maxP = Math.Round(maxP, 2); }
                                        if (maxP > 100)
                                        {
                                            maxP = Math.Round(maxP, 0);
                                        }
                                    }
                                    if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "CarId")
                                    {
                                        int.TryParse(inner.ReadString(), out carid);
                                    }
                                    inner.Read();
                                }
                                if (!dic.ContainsKey(carid) && carid > 0 && (minP > 0 || maxP > 0))
                                {
                                    string ucarPrice = "";
                                    if (minP > 0 && maxP > 0)
                                    {
                                        ucarPrice = minP.ToString() + "-" + maxP.ToString() + "��";
                                    }
                                    else
                                    {
                                        ucarPrice = maxP > minP ? maxP.ToString() + "��" : minP + "��";
                                    }
                                    dic.Add(carid, ucarPrice);
                                }
                            }
                        }
                        catch
                        { }
                    }
                }
                CacheManager.InsertCache(cacheName, dic, WebConfig.CachedDuration);
            }
            return dic;
        }

        #endregion

        /// <summary>
        /// ��ȡ�����б� ������Ʒ��Id
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="nocache">�Ƿ�ӻ��������</param>
        /// <returns></returns>
        public List<CarInfoForSerialSummaryEntity> GetCarInfoForSerialSummaryBySerialId(int serialId, bool nocache = false)
        {
            string cacheKey = string.Format("Car_CarInfoForSerialSummary_{0}", serialId);
            if (!nocache)
            {
                object allCarInfoForSerialSummary = CacheManager.GetCachedData(cacheKey);
                if (allCarInfoForSerialSummary != null)
                    return (List<CarInfoForSerialSummaryEntity>)allCarInfoForSerialSummary;
            }
            List<CarInfoForSerialSummaryEntity> carInfoList = new List<CarInfoForSerialSummaryEntity>();
            DataSet ds = cbd.GetAllCarInfoForSerialSummary(serialId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Dictionary<int, string> dictCarPriceRange = new PageBase().GetAllCarPriceRange();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int carId = ConvertHelper.GetInteger(dr["car_id"]);
                    Dictionary<int, string> dictParams = GetCarAllParamByCarID(carId);
                    string saleState = dr["Car_SaleState"].ToString().Trim();
                    string carPriceRange = string.Empty;
                    if (saleState == "ͣ��")
                        carPriceRange = "ͣ��";
                    else
                        carPriceRange = dictCarPriceRange.ContainsKey(carId) ? dictCarPriceRange[carId] : "";
                    //modified by sk 2013.08.07 ������ʽΪnull/����/��Ȼ�����ģ���Ϊһ�ַ���
                    string inhaleType = string.Empty;
                    if (dictParams.ContainsKey(425))
                    {
                        if (dictParams[425] == "" || dictParams[425] == "����" || dictParams[425] == "��Ȼ����") { }
                        else
                            inhaleType = dictParams[425];
                    }
                    DateTime dt = DateTime.MinValue;
                    if (dictParams.ContainsKey(383) && dictParams.ContainsKey(384) && dictParams.ContainsKey(385))
                    {
                        int year = ConvertHelper.GetInteger(dictParams[385]);
                        int month = ConvertHelper.GetInteger(dictParams[384]);
                        int day = ConvertHelper.GetInteger(dictParams[383]);
                        if (year > 0 && month > 0 && day > 0)
                        {
                            dt = new DateTime(year, month, day);
                        }
                    }

                    //add by sk 2014.3.31 ��ѹ��ʽ
                    //add by sk 2014.3.31 ��ѹ��ʽ
                    string addPressType = string.Empty;
                    //if (dictParams.ContainsKey(425))
                    //{
                    //	if (dictParams[425] == "" || dictParams[425] == "����" || dictParams[425] == "��") { }
                    //	else
                    //		addPressType = dictParams[425];
                    //}
                    ////�������� �������� ���û��ֵ ����ǧ��ʱ�������� ��û�������
                    //int maxPower = 0;
                    //if (dictParams.ContainsKey(791))
                    //{
                    //    maxPower = ConvertHelper.GetInteger(dictParams[791]);
                    //}
                    //if (maxPower == 0)
                    //{
                    //    if (dictParams.ContainsKey(430))
                    //        maxPower = (int)(Convert.ToDouble(dictParams[430]) * 1.36);
                    //}
                    //maxPower = maxPower == 0 ? 9999 : maxPower;//������� Ϊ0 �����õ� ��ͬ����������
                    var fuelType = dictParams.ContainsKey(578) ? dictParams[578] : string.Empty;
                    int kw = 0;
                    int electrickW = 0;
                    if (fuelType == "����")
                    {
                        kw = dictParams.ContainsKey(870) ? ConvertHelper.GetInteger(dictParams[870]) : 0;
                    }
                    else if (fuelType == "�͵���" || fuelType == "�����")
                    {
                        double tempDiankW;
                        if (dictParams.ContainsKey(870) && double.TryParse(dictParams[870], out tempDiankW))
                        {
                            electrickW = Convert.ToInt32(tempDiankW);
                        }

                        double tempYoukW;
                        if (dictParams.ContainsKey(430) && double.TryParse(dictParams[430], out tempYoukW))
                        {
                            kw = ConvertHelper.GetInteger(tempYoukW);
                        }

                        //double tempYoukW;
                        //double.TryParse(dictParams[430], out tempYoukW);

                        //int diankW = dictParams.ContainsKey(870) ? ConvertHelper.GetInteger(tempDiankW) : 0;
                        //int youkW = dictParams.ContainsKey(430) ? ConvertHelper.GetInteger(tempYoukW) : 0;
                        //kw = youkW;
                        //electrickW = diankW;
                    }
                    else
                    {
                        if (dictParams.ContainsKey(430))
                        {
                            double tempkW;
                            double.TryParse(dictParams[430], out tempkW);
                            kw = (int)Math.Round(tempkW);
                        }
                    }
                    kw = kw == 0 ? 9999 : kw;
                    string exhaust = dr["Engine_Exhaust"].ToString();
                    if (string.IsNullOrEmpty(exhaust) || ConvertHelper.GetDouble(exhaust.Replace("L", "")) <= 0)
                    {
                        exhaust = "����";
                        if (fuelType == "����")
                            exhaust = "�綯��";
                    }
                    //�Ƿ���ƽ�н���
                    int isImport = (dictParams.ContainsKey(382) && dictParams[382] == "ƽ�н���") ? 1 : 0;

                    carInfoList.Add(new CarInfoForSerialSummaryEntity()
                    {
                        CarID = carId,
                        CarName = dr["car_name"].ToString(),
                        SaleState = saleState,
                        CarPriceRange = carPriceRange,
                        CarPV = dr["Pv_SumNum"].ToString() == "" ? 0 : int.Parse(dr["Pv_SumNum"].ToString()),
                        ReferPrice = dr["car_ReferPrice"].ToString(),
                        TransmissionType = dr["UnderPan_TransmissionType"].ToString(),//������
                        Engine_Exhaust = exhaust,//����
                        CarYear = dr["Car_YearType"] == DBNull.Value ? "" : dr["Car_YearType"].ToString(),
                        ProduceState = dr["Car_ProduceState"].ToString(),
                        UnderPan_ForwardGearNum = dictParams.ContainsKey(724) ? dictParams[724] : "",//��λ����
                        Engine_MaxPower = kw,//dictParams.ContainsKey(430) ? (int)(Convert.ToDouble(dictParams[430]) * 1.36) : 0,//�������
                        Electric_Peakpower = electrickW,
                        Engine_InhaleType = inhaleType,//������ʽ
                        Engine_AddPressType = addPressType,//��ѹ��ʽ
                        Oil_FuelType = dictParams.ContainsKey(578) ? dictParams[578] : "",//ȼ������
                        IsImport = isImport,
                        BodyForm = dr["Body_Type"].ToString(),
                        MarketDateTime = dt
                    });
                }
            }
            CacheManager.InsertCache(cacheKey, carInfoList, 10);
            return carInfoList;
        }

        /// <summary>
        /// ��ȡͬ�������ų��� ��������ĳ��Ʒ�ƣ�
        /// </summary>
        /// <param name="carLevel"></param>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public DataSet GetHotCarForCompare(string carLevel, int serialId)
        {
            return cbd.GetHotCarForCompare(carLevel, serialId);
        }

        /// <summary>
        /// ��ȡ��Ʒ�� �� ���ų��� ������� �����ų�
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public Dictionary<int, int> GetHotCarForPhotoCompareBySerialId(IEnumerable<int> serialIdArray)
        {
            var dict = new CommonService().GetPhotoCompareSerialAndCarList();
            Dictionary<int, int> dictSerialCarId = new Dictionary<int, int>();
            int carId = 0;
            foreach (var serialId in serialIdArray)
            {
                if (dict.ContainsKey(serialId))
                {
                    List<int> carList = dict[serialId];
                    DataTable dt = cbd.GetHotCarBySerialId(serialId).Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        carId = ConvertHelper.GetInteger(dr["car_id"]);
                        if (carList.Contains(carId))
                        {
                            dictSerialCarId.Add(serialId, carId);
                            break;
                        }
                    }
                }
            }
            return dictSerialCarId;
        }

        /// <summary>
        /// ��ȡ��Ʒ�� �� ���ų��� ������� �����ų�
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public int GetHotCarForPhotoCompareBySerialId(int serialId)
        {
            int carId = 0;
            var dict = new CommonService().GetPhotoCompareSerialAndCarList();
            if (!dict.ContainsKey(serialId)) return carId;
            List<int> carList = dict[serialId];
            DataTable dt = cbd.GetHotCarBySerialId(serialId).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                var tempCarId = ConvertHelper.GetInteger(dr["car_id"]);
                if (carList.Contains(tempCarId))
                {
                    carId = tempCarId;
                    break;
                }
            }
            return carId;
        }

        public DataSet GetCarBaseDataBySerialId(int serialId, bool isAll = false)
        {
            return cbd.GetCarBaseDataBySerialId(serialId, isAll);
        }

        /// <summary>
        /// ��ȡ ��Ʒ�� ���� List ������
        /// ������� ���� ������ ָ����
        /// ���۳�ϵ����ȡȫ��δ����+���۳��
        /// ͣ�۳�ϵ����ȡȫ�����
        /// δ���г�ϵ����ȡȫ�����
        /// ����״̬���鳵ϵ����ȡȫ������
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="isZaiShou"></param>
        public List<CarInfoForSerialSummaryEntity> GetCarBaseListBySerialId(int serialId, bool isAll = false)
        {
            string cacheKey = "Car_BasicBll_GetCarBaseDataBySerialId_" + serialId + "_" + (isAll ? 1 : 0);
            var carList = CacheManager.GetCachedData(cacheKey);
            if (carList != null)
                return (List<CarInfoForSerialSummaryEntity>)carList;

            DataSet ds = cbd.GetCarBaseDataBySerialId(serialId, isAll);
            var carListTemp = new List<CarInfoForSerialSummaryEntity>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int carId = ConvertHelper.GetInteger(dr["car_id"]);
                    carListTemp.Add(new CarInfoForSerialSummaryEntity()
                    {
                        CarID = carId,
                        CarName = ConvertHelper.GetString(dr["Car_Name"]),
                        ReferPrice = ConvertHelper.GetString(dr["car_ReferPrice"]),
                        CarYear = ConvertHelper.GetString(dr["Car_YearType"]),
                        Engine_Exhaust = ConvertHelper.GetString(dr["Engine_Exhaust"]),
                        TransmissionType = ConvertHelper.GetString(dr["UnderPan_TransmissionType"])
                    });
                }
                carListTemp.Sort(NodeCompare.CompareCarByYear);
                CacheManager.InsertCache(cacheKey, carListTemp, WebConfig.CachedDuration);
            }
            return carListTemp;
        }

        public DataSet GetAllCarInfoForSerialSummary(int serialId)
        {
            string cacheKey = "Car_BasicBll_GetAllCarInfoForSerialSummary_" + serialId;
            var carDataSet = CacheManager.GetCachedData(cacheKey);
            if (carDataSet != null)
            {
                return (DataSet)carDataSet;
            }
            DataSet ds = cbd.GetAllCarInfoForSerialSummary(serialId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                CacheManager.InsertCache(cacheKey, ds, 30);
            }
            return ds;
        }
        #region CarInfoDal

        /// <summary>
        /// ȡ��Ч����
        /// </summary>
        /// <returns></returns>
        public DataSet GetCarInfoByParams()
        {
            return cid.GetCarInfoByParams();
        }

        public DataSet GetCarPVData(int serialId)
        {
            try
            {
                if (serialId > 0)
                    return cid.GetCarPVDataBySerialId(serialId);
                else
                    return cid.GetCarPVData();
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
                return null;
            }
        }
        #endregion


        /// <summary>
        /// ���ݳ����ж��Ƿ��˰������˰
        /// </summary>
        /// <param name="carIdList"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetSubTaxByCarIds(List<int> carIdList)
        {
            if (carIdList == null || carIdList.Count == 0)
            {
                return null;
            }
            //����˰��������
            var dictPurchaseTaxParamN = this.GetCarParamValueByCarIds(carIdList.ToArray(), 987);
            //����˰����
            var dictPurchaseTaxParam = this.GetCarParamValueByCarIds(carIdList.ToArray(), 986);
            //����
            var dictEngine_ExhaustParam = this.GetCarParamValueByCarIds(carIdList.ToArray(), 785);

            Dictionary<int, string> dictCarTaxTag = new Dictionary<int, string>();
            foreach (int carId in carIdList)
            {
                double exhaust = 0;
                if (dictEngine_ExhaustParam.ContainsKey(carId))
                {
                    exhaust = ConvertHelper.GetDouble(dictEngine_ExhaustParam[carId]);
                }
                if (dictPurchaseTaxParamN.ContainsKey(carId) && (dictPurchaseTaxParamN[carId] == "��1��" || dictPurchaseTaxParamN[carId] == "��2��" || dictPurchaseTaxParamN[carId] == "��3��" || dictPurchaseTaxParamN[carId] == "��4��" || dictPurchaseTaxParamN[carId] == "��5��" || dictPurchaseTaxParamN[carId] == "��6��") && dictPurchaseTaxParam.ContainsKey(carId))
                {
                    if (dictPurchaseTaxParam[carId] == "����")
                    {
                        if (!dictCarTaxTag.ContainsKey(carId))
                        {
                            dictCarTaxTag.Add(carId, "��˰");
                        }
                    }
                    else if (dictPurchaseTaxParam[carId] == "����")
                    {
                        if (!dictCarTaxTag.ContainsKey(carId))
                        {
                            dictCarTaxTag.Add(carId, "��˰");
                        }
                    }
                }
                else if (exhaust > 0 && exhaust <= 1.6)
                {
                    if (!dictCarTaxTag.ContainsKey(carId))
                    {
                        dictCarTaxTag.Add(carId, "����˰75��");
                    }
                }
            }
            return dictCarTaxTag;
        }


        public int GetSerialCarRellyPicCount(int carId)
        {
            int count = 0;
            try
            {
                var xmlFile =
                string.Format(Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialCarReallyImagePath),
                    carId);

                XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);
                if (xmlDoc.HasChildNodes)
                {
                    //XmlDocument xmlDoc = new XmlDocument();
                    //xmlDoc.Load(xmlFile);
                    XmlNode node = xmlDoc.SelectSingleNode("//Data//Total");
                    var countStr = node.InnerText;
                    int.TryParse(countStr, out count);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return count;
        }
        /// <summary>
        /// ���Ͳ���ģ��
        /// </summary>
        /// <returns></returns>
        public List<ParameterGroupEntity> GetCarParameterJsonConfig(bool isVersion86)
        {
            List<ParameterGroupEntity> parameterGroup = new List<ParameterGroupEntity>();
            try
            {


                string cacheKey = string.Format(DataCacheKeys.CarParameterJson, isVersion86);
                object getCarParameterJsonConfig = CacheManager.GetCachedData(cacheKey);
                if (getCarParameterJsonConfig != null)
                {
                    parameterGroup = getCarParameterJsonConfig as List<ParameterGroupEntity>;
                }
                else
                {
                    if (isVersion86)
                    {
                        string fileName = parameterConfigPath;
                        if (File.Exists(fileName))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(fileName);
                            if (doc != null && doc.HasChildNodes)
                            {
                                ParameterGroupEntity parameter;
                                XmlNodeList xnl = doc.SelectNodes("/ParameterConfigurationList/Parameter/ParameterList");
                                if (xnl != null && xnl.Count > 0)
                                {
                                    int i = 0;
                                    foreach (XmlNode xnCate in xnl)
                                    {

                                        parameter = new ParameterGroupEntity();
                                        parameter.GroupID = i;
                                        parameter.Name = xnCate.Attributes["Name"].Value;


                                        // �����
                                        if (xnCate.ChildNodes.Count > 0)
                                        {
                                            parameter.Fields = new List<ParameterGroupFieldEntity>();
                                            ParameterGroupFieldEntity field;
                                            // ��������
                                            foreach (XmlNode xn in xnCate.ChildNodes)
                                            {
                                                if (xn.NodeType == XmlNodeType.Element)
                                                {
                                                    field = new ParameterGroupFieldEntity();
                                                    field.Key = xn.Attributes["Value"].Value;
                                                    field.ParamID = TypeParse.StrToInt(xn.Attributes["ParamID"].Value, 0);
                                                    field.Unit = xn.Attributes["Unit"].Value;
                                                    field.Title = xn.Attributes["Name"].Value;
                                                    parameter.Fields.Add(field);
                                                }
                                            }
                                        }
                                        i++;
                                        parameterGroup.Add(parameter);
                                    }
                                }
                            }
                           
                        }
                    }
                    else
                    {
                        string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterForJsonNewV2.xml";
                        if (File.Exists(fileName))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(fileName);
                            if (doc != null && doc.HasChildNodes)
                            {
                                ParameterGroupEntity parameter;
                                XmlNodeList xnl = doc.SelectNodes("/Param/Group");
                                if (xnl != null && xnl.Count > 0)
                                {
                                    int i = 0;
                                    foreach (XmlNode xnCate in xnl)
                                    {

                                        parameter = new ParameterGroupEntity();
                                        parameter.GroupID = i;
                                        parameter.Name = xnCate.Attributes["Desc"].Value.ToString();


                                        // �����
                                        if (xnCate.ChildNodes.Count > 0)
                                        {
                                            parameter.Fields = new List<ParameterGroupFieldEntity>();
                                            ParameterGroupFieldEntity field;
                                            // ��������
                                            foreach (XmlNode xn in xnCate.ChildNodes)
                                            {
                                                if (xn.NodeType == XmlNodeType.Element)
                                                {
                                                    field = new ParameterGroupFieldEntity();
                                                    field.Key = xn.Attributes["Value"].Value.ToString();
                                                    field.ParamID = TypeParse.StrToInt(xn.Attributes["ParamID"].Value, 0);
                                                    field.Unit = xn.Attributes["Unit"].Value.ToString();
                                                    field.Title = xn.Attributes["Desc"].Value.ToString();
                                                    parameter.Fields.Add(field);
                                                }
                                            }
                                        }
                                        i++;
                                        parameterGroup.Add(parameter);
                                    }
                                }
                            }
                        }
                    }
                    CacheManager.InsertCache(cacheKey, parameterGroup, 10);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(string.Format("��������ParameterConfigurationNewV2.config����,Message:{0},StackTrace:{1}", ex.Message, ex.StackTrace));
            }
            return parameterGroup;
        }

        /// <summary>
        /// ���ɲ���ģ���ֵ�
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ParameterGroupFieldEntity> GetParamDic(bool isVersion86)
        {
            Dictionary<string, ParameterGroupFieldEntity> result = new Dictionary<string, ParameterGroupFieldEntity>();
            var parameterList = GetCarParameterJsonConfig(isVersion86);
            foreach (var item in parameterList)
            {
                foreach (var field in item.Fields)
                {
                    if (!result.ContainsKey(field.Key))
                    {
                        result.Add(field.Key, field);
                    }
                }
            }
            return result;
        }

        public List<CarParameterListEntity> GetCarParamterListWithWebCacheByCarIds(List<int> carIds, bool isVersion86)
        {
            //return GetCarParamterListByCarIds(carIds);
            string carParamterKey = string.Format(DataCacheKeys.CarParameterListKey, isVersion86, string.Join("_", carIds));
            var carParamterList = CacheManager.GetCachedData(carParamterKey);
            if (carParamterList == null)
            {
                List<CarParameterListEntity> newCarParamterList = GetCarParamterListByCarIds(carIds, isVersion86);

                if (newCarParamterList != null && newCarParamterList.Count > 0)
                {
                    CacheManager.InsertCache(carParamterKey, newCarParamterList, 5);
                }
                return newCarParamterList;
            }

            return (List<CarParameterListEntity>)carParamterList;
        }

        public List<CarParameterListEntity> GetCarParamterListByCarIds(List<int> carIds, bool isVersion86)
        {
            /*
            ��  �ڵ�
            ��  ��
            ѡ��  ����Բ
            ѡ��|1000   ����Բ ѡ��  �۸�
            ����       ����
            ����,����    
            ����,���£�ǰ��|1000,ǰ��|1000  ����  �۸�
             */
            List<CarParameterListEntity> result = new List<CarParameterListEntity>();
            Dictionary<int, Dictionary<string, string>> parameterDic = GetCarCompareDataWithOptionalByCarIDs(carIds);
            var fieldDic = GetParamDic(isVersion86);
            List<CarParameterEntity> carParameterList;
            foreach (var group in parameterDic)
            {
                carParameterList = new List<CarParameterEntity>();
                CarParameterEntity p;
                ParameterGroupFieldEntity field;
                foreach (var item in group.Value)
                {

                    if (fieldDic.ContainsKey(item.Key))
                    {
                        field = fieldDic[item.Key];
                        p = new CarParameterEntity();
                        p.ItemList = new List<ParamItemEntity>();
                        p.ParamKey = item.Key;
                        var itemValue = item.Value;
                        bool isColor = false;
                        try
                        {
                            if (item.Key == "Car_OutStat_BodyColorRGB")
                            {
                                isColor = true;
                                itemValue = itemValue.Replace(",", "$");
                                itemValue = itemValue.Replace("|", ",");
                                itemValue = itemValue.Replace("$", "|");
                            }
                            #region �����
                            switch (itemValue)
                            {
                                case "��":
                                    p.ItemList.Add(new ParamItemEntity
                                    {
                                        Icon = "��"
                                    });
                                    break;
                                case "��":
                                    p.ItemList.Add(new ParamItemEntity
                                    {
                                        Icon = "-"
                                    });
                                    break;
                                case "ѡ��":
                                    p.ItemList.Add(new ParamItemEntity
                                    {
                                        Icon = "��"
                                    });
                                    break;

                                default:
                                    if (itemValue.Contains("|") && itemValue.Contains(","))
                                    {
                                        string[] itemArr = itemValue.Split(new char[] { ',' });
                                        string[] filedArr;
                                        foreach (var itemStr in itemArr)
                                        {
                                            if (!string.IsNullOrWhiteSpace(itemStr))
                                            {
                                                if (itemStr.Contains("|"))
                                                {
                                                    filedArr = itemStr.Split(new char[] { '|' });
                                                    if (filedArr.Length > 1)
                                                    {
                                                        p.ItemList.Add(new ParamItemEntity
                                                        {
                                                            Icon = "��",
                                                            Title = filedArr[0],
                                                            Des = filedArr[1] + (isColor ? "" : "Ԫ")
                                                        });
                                                    }
                                                    else
                                                    {
                                                        if (itemStr != "��")
                                                        {
                                                            p.ItemList.Add(new ParamItemEntity
                                                            {
                                                                Icon = "��",
                                                                Title = itemStr
                                                            });
                                                        }
                                                    }


                                                }
                                                else
                                                {
                                                    if (itemStr != "��")
                                                    {
                                                        p.ItemList.Add(new ParamItemEntity
                                                        {
                                                            Icon = "��",
                                                            Title = itemStr
                                                        });
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else if (itemValue.Contains(","))
                                    {
                                        string[] itemArr = itemValue.Split(new char[] { ',' });
                                        foreach (var itemStr in itemArr)
                                        {
                                            if (!string.IsNullOrWhiteSpace(itemStr))
                                            {
                                                if (itemStr != "��")
                                                {
                                                    p.ItemList.Add(new ParamItemEntity
                                                    {
                                                        Icon = "��",
                                                        Title = itemStr
                                                    });
                                                }
                                            }
                                        }
                                    }
                                    else if (itemValue.Contains("|"))
                                    {
                                        string[] itemArr = itemValue.Split(new char[] { '|' });

                                        if (itemArr.Length > 1)
                                        {
                                            p.ItemList.Add(new ParamItemEntity
                                            {
                                                Icon = "��",
                                                Title = itemArr[0],
                                                Des = itemArr[1] + (isColor ? "" : "Ԫ")
                                            });
                                        }
                                        else
                                        {
                                            if (itemValue != "��")
                                            {
                                                p.ItemList.Add(new ParamItemEntity
                                                {
                                                    Icon = "��",
                                                    Title = itemValue
                                                });
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(itemValue))
                                        {
                                            p.ItemList.Add(new ParamItemEntity
                                            {
                                                Title = itemValue
                                            });
                                        }
                                    }
                                    break;
                            }
                            #endregion
                            carParameterList.Add(p);
                        }
                        catch (Exception ex)
                        {
                            var message = string.Format("���������,carIds:{0},itemValue:{1}", string.Join(",", carIds), itemValue);
                            CommonFunction.WriteLog(string.Format("[message]:{0},[StackTrace]:{1},[carMessage]:{2}", ex.Message, ex.StackTrace, message));
                        }
                    }
                }

                result.Add(new CarParameterListEntity { CarParameterList = carParameterList, CarId = group.Key });
            }
            return result;
        }

        /// <summary>
        /// ��ȡ�����������ʾ�ַ���
        /// ���ݵ�λ�����ͱ����������ж�
        /// </summary>
        /// <returns></returns>
        public string GetCarTransmissionType(string forwardGearNum, string transmissionType)
        {
            if (string.IsNullOrWhiteSpace(transmissionType))
                return string.Empty;
            if (transmissionType == "CVT�޼�����" || transmissionType == "E-CVT�޼�����" || transmissionType == "���ٱ�����")
            {
                return transmissionType;
            }
            if (!string.IsNullOrWhiteSpace(forwardGearNum))
            {
                return string.Format("{0}�� {1}", forwardGearNum, transmissionType);
            }
            return string.Empty;
        }
        /// <summary>
        /// ͨ������ҳ�����б�  Ŀǰ΢��С��������ҳ��ʹ��
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public List<CarInfoForSerialSummaryEntity> GetCarListForSerialSummaryBySerialId(int serialId, bool includeStopSale)
        {
            List<CarInfoForSerialSummaryEntity> result = new List<CarInfoForSerialSummaryEntity>();
            List<CarInfoForSerialSummaryEntity> carinfoList = GetCarInfoForSerialSummaryBySerialId(serialId);
            List<CarInfoForSerialSummaryEntity> carList = new List<CarInfoForSerialSummaryEntity>();
            if (includeStopSale == false)
            {
                carList = carinfoList.FindAll(p => p.SaleState != "ͣ��");
            }
            else
            {
                carList = carinfoList;
            }

            carList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);

            var listGroupNew = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
            var listGroupOff = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
            var listGroupImport = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
            var importGroup = carList.GroupBy(p => new { p.IsImport }, p => p);
            foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in importGroup)
            {
                var key = CommonFunction.Cast(info.Key, new { IsImport = 0 });
                if (key.IsImport == 1)
                {
                    listGroupImport.Add(info);
                }
                else
                {
                    var querySale = info.ToList().GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower }, p => p);
                    foreach (IGrouping<object, CarInfoForSerialSummaryEntity> subInfo in querySale)
                    {
                        var isStopState = subInfo.FirstOrDefault(p => p.ProduceState != "ͣ��");
                        if (isStopState != null)
                            listGroupNew.Add(subInfo);
                        else
                            listGroupOff.Add(subInfo);
                    }
                }
            }
            listGroupNew.AddRange(listGroupOff);
            listGroupNew.AddRange(listGroupImport);
            foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in listGroupNew)
            {
                List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList<CarInfoForSerialSummaryEntity>();//�����ļ���                
                foreach (CarInfoForSerialSummaryEntity entity in carGroupList)
                {
                    result.Add(entity);
                }
            }
            return result;
        }

        public List<CarGroupEntity> GetCarGroupBySerialIdAndCSID(int cityId, int serialId, bool includeStopSale)
        {

            var cacheKey = string.Format(DataCacheKeys.CarGroupListByserialIdAndCityId, serialId, cityId, includeStopSale);
            List<CarGroupEntity> carGroupList = CacheManager.GetCachedData<List<CarGroupEntity>>(cacheKey);
            if (carGroupList == null)
            {
                carGroupList = new List<CarGroupEntity>();
                Dictionary<string, CarGroupEntity> carGroupDic = new Dictionary<string, CarGroupEntity>();
                var carList = GetCarListForSerialSummaryBySerialId(serialId, includeStopSale);
                var serialBll = new Car_SerialBll();
                List<int> carIds = new List<int>();
                foreach (var item in carList)
                {
                    string groupKey = item.Engine_Exhaust + "/" + item.Engine_MaxPower;
                    if (!carGroupDic.ContainsKey(groupKey))
                    {
                        carGroupDic.Add(groupKey, new CarGroupEntity
                        {
                            Name = groupKey + "kw " + item.Engine_InhaleType,
                            CarList = new List<CarInfoEntity>()
                        });
                    }
                    carIds.Add(item.CarID);
                    var taxreief = GetTax(item.CarID, item.SaleState, item.Engine_Exhaust);
                    var newStatus = serialBll.GetCarMarketText(item.CarID, item.SaleState, item.MarketDateTime, item.ReferPrice);
                    carGroupDic[groupKey].CarList.Add(new CarInfoEntity
                    {
                        CarId = item.CarID,
                        Name = item.CarName,
                        Year = TypeParse.StrToInt(item.CarYear, 2000),
                        IsSupport = item.IsImport == 1,
                        MinPrice = item.CarPriceRange,
                        ReferPrice = item.ReferPrice,
                        Trans = item.TransmissionType,
                        SaleState = item.SaleState,
                        NewSaleStatus = string.IsNullOrWhiteSpace(newStatus) ? item.SaleState : newStatus,
                        SupportType = 0,
                        ImportType = item.IsImport == 1 ? "ƽ�н��ڳ�" : "",
                        CarPV = item.CarPV,
                        IsTax = (!string.IsNullOrWhiteSpace(taxreief)),//
                        TaxRelief = taxreief,//
                        TimeToMarket = item.MarketDateTime.ToString(),
                        IsHaveImage = false///
                    });
                }
                foreach (var carGroup in carGroupDic.Values)
                {
                    carGroupList.Add(carGroup);
                }
                if (carGroupList != null)
                {
                    CacheManager.InsertCache(cacheKey, carGroupList, 5);
                }
            }
            return carGroupList;
        }

        /// <summary>
        /// ��ȡ�����˰
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="saleState"></param>
        /// <param name="exhaust"></param>
        /// <returns></returns>
        public string GetTax(int carId, string saleState, string exhaust)
        {
            Dictionary<int, string> dictCarParams = GetCarAllParamByCarID(carId);

            string strTravelTax = string.Empty;
            var dex = TypeParse.StrToFloat(exhaust.ToUpper().Replace("L", ""), 0);

            if (saleState == "����")
            {
                if (dictCarParams.ContainsKey(987) && (dictCarParams[987] == "��1��" || dictCarParams[987] == "��2��" || dictCarParams[987] == "��3��" || dictCarParams[987] == "��4��" || dictCarParams[987] == "��5��" || dictCarParams[987] == "��6��") && dictCarParams.ContainsKey(986))
                {
                    if (dictCarParams[986].ToString() == "����")
                    {
                        strTravelTax = "����˰����";

                    }
                    else if (dictCarParams[986].ToString() == "����")
                    {
                        strTravelTax = "��������˰";

                    }
                }
                else if (dex > 0 && dex <= 1.6)
                {
                    strTravelTax = "����˰75��";

                }
            }
            return strTravelTax;
        }
    }
}
