using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Linq;

using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;

namespace BitAuto.CarChannel.Common
{
    public class NodeCompare
    {
        #region �°� ����ҳ �����б� ���� add by sk 2013.08.05
        /// <summary>
        /// ����������С��������������ͬ�İ���Ȼ��������ǰ��������ѹ�ں󣻽�����ʽ��ͬ�İ��������С��������
        /// add by sk ���� ����״̬���� �ڲ� ���� ͣ�� gux
        /// �Ȱ�������µ�������ͬ���İ������������ֶ������Զ����Զ�������һ�壬�޼����٣�˫��ϣ���ͬ������İ�ָ�����ɵ͵�����ʾ
        /// </summary>
        /// <param name="car1"></param>
        /// <param name="car2"></param>
        /// <returns></returns>
        public static int CompareCarByExhaustAndPowerAndInhaleType(CarInfoForSerialSummaryEntity car1, CarInfoForSerialSummaryEntity car2)
        {
            double exhaust1 = 0;
            if (!double.TryParse(car1.Engine_Exhaust.TrimEnd('L'), out exhaust1))
            {
                exhaust1 = 9999;
            }

            double exhaust2 = 0;
            if (!double.TryParse(car2.Engine_Exhaust.TrimEnd('L'), out exhaust2))
            {
                exhaust2 = 9999;
            }

            //int result = String.Compare(car1.Engine_Exhaust, car2.Engine_Exhaust);
            int result = 0;
            if (exhaust1 > exhaust2)
                result = 1;
            else if (exhaust1 < exhaust2)
                result = -1;
            if (result == 0)
            {
                result = CompareInhaleType(car1.Engine_InhaleType, car2.Engine_InhaleType);
                if (result == 0)
                {
                    result = CompareMaxPower(car1.Engine_MaxPower, car2.Engine_MaxPower);
                    if (result == 0)
                    {
                        result = CompareProduceState(car1.ProduceState, car2.ProduceState);
                        if (result == 0)
                        {
                            result = CompareCarByYear(car1, car2);
                        }
                    }
                }
            }
            if (result == 0)
            {
                car2.CarID.CompareTo(car1.CarID);
            }
            return result;
        }
        /// <summary>
        /// ����״̬
        /// </summary>
        /// <param name="inhaleType1"></param>
        /// <param name="inhaleType2"></param>
        /// <returns></returns>
        public static int CompareProduceState(string produceState1, string produceState2)
        {
            if (produceState1 == "�ڲ�")
                produceState1 = "a";
            else if (produceState1 == "����")
                produceState1 = "b";
            else if (produceState1 == "ͣ��")
                produceState1 = "c";
            else
                produceState1 = "d";

            if (produceState2 == "�ڲ�")
                produceState2 = "a";
            else if (produceState2 == "����")
                produceState2 = "b";
            else if (produceState2 == "ͣ��")
                produceState2 = "c";
            else
                produceState2 = "d";

            return String.Compare(produceState1, produceState2);
        }

        /// <summary>
        /// ������ʽ ����
        /// </summary>
        /// <param name="inhaleType1"></param>
        /// <param name="inhaleType2"></param>
        /// <returns></returns>
        public static int CompareInhaleType(string inhaleType1, string inhaleType2)
        {
            if (inhaleType1.IndexOf("��Ȼ����") > -1)
                inhaleType1 = "a";
            else
                inhaleType1 = "b";

            if (inhaleType2.IndexOf("��Ȼ����") > -1)
                inhaleType2 = "a";
            else
                inhaleType2 = "b";

            return String.Compare(inhaleType1, inhaleType2);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="inhaleType1"></param>
        /// <param name="inhaleType2"></param>
        /// <returns></returns>
        public static int CompareMaxPower(int maxPower1, int maxPower2)
        {
            int reuslt = 0;
            if (maxPower1 > maxPower2)
                reuslt = 1;
            else if (maxPower2 > maxPower1)
                reuslt = -1;
            return reuslt;
        }
        /// <summary>
        /// ����˳�� ���\����\������\ָ����
        /// </summary>
        /// <param name="car1"></param>
        /// <param name="car2"></param>
        /// <returns></returns>
        public static int CompareCarByYear(CarInfoForSerialSummaryEntity car1, CarInfoForSerialSummaryEntity car2)
        {
            int ret = 0;
            double year1 = ConvertHelper.GetDouble(car1.CarYear);
            double year2 = ConvertHelper.GetDouble(car2.CarYear);
            if (year1 > year2)
                ret = -1;
            else if (year1 < year2)
                ret = 1;
            else
            {
                ret = CompareBodyForm(car1.BodyForm, car2.BodyForm);
                if (ret == 0)
                {
                    ret = String.Compare(car1.Engine_Exhaust, car2.Engine_Exhaust);
                    if (ret == 0)
                    {
                        ret = CompareTransmissionType(car1.TransmissionType, car2.TransmissionType);
                        if (ret == 0)
                        {
                            double price1 = ConvertHelper.GetDouble(car1.ReferPrice);
                            double price2 = ConvertHelper.GetDouble(car2.ReferPrice);
                            if (price1 > price2)
                                ret = 1;
                            else if (price2 > price1)
                                ret = -1;
                        }
                    }
                }
            }

            return ret;
        }
        #endregion




        /// <summary>
        /// ʵ��Ʒ���Ȱ�������������򣬺�ƴ������
        /// </summary>
        /// <param name="ele1">��һ���ڵ�</param>
        /// <param name="ele2">�ڶ����ڵ�</param>
        /// <returns>�ȽϽ��</returns>
        public static int CompareBrandNode(XmlElement ele1, XmlElement ele2)
        {
            string country1 = ele1.GetAttribute("Country");
            string country2 = ele2.GetAttribute("Country");
            int ret = String.Compare(country1, country2);
            ret *= -1; //����
            if (ret == 0)
            {
                string spell1 = ele1.GetAttribute("Spell");
                string spell2 = ele2.GetAttribute("Spell");
                ret = String.Compare(spell1, spell2);
            }
            return ret;
        }

        /// <summary>
        /// ʵ��Ʒ���ȹ���������������ٰ�ƴ������
        /// </summary>
        /// <param name="ele1">��һ���ڵ�</param>
        /// <param name="ele2">�ڶ����ڵ�</param>
        /// <returns>�ȽϽ��</returns>
        public static int CompareBrandNodeSelfFirst(XmlElement ele1, XmlElement ele2)
        {
            string country1 = ele1.GetAttribute("Country");
            string country2 = ele2.GetAttribute("Country");
            int ret = String.Compare(country1, country2);
            if (ret == 0)
            {
                string spell1 = ele1.GetAttribute("Spell");
                string spell2 = ele2.GetAttribute("Spell");
                ret = String.Compare(spell1, spell2);
            }
            return ret;
        }

        /// <summary>
        /// ����Ʒ�ư�spell����
        /// </summary>
        /// <param name="ele1"></param>
        /// <param name="ele2"></param>
        /// <returns></returns>
        public static int CompareSerialBySpellAsc(XmlElement ele1, XmlElement ele2)
        {
            string spell1 = ele1.GetAttribute("Spell");
            string spell2 = ele2.GetAttribute("Spell");
            return String.Compare(spell1, spell2);
        }

        /// <summary>
        /// ����Ʒ�ư���ע�Ƚ�������
        /// </summary>
        /// <param name="ele1"></param>
        /// <param name="ele2"></param>
        /// <returns></returns>
        public static int CompareSerialByPvDesc(XmlElement ele1, XmlElement ele2)
        {
            int ret = 0;
            int pv1 = Convert.ToInt32(ele1.GetAttribute("CsPV"));
            int pv2 = Convert.ToInt32(ele2.GetAttribute("CsPV"));
            if (pv1 > pv2)
                ret = -1;
            else if (pv1 < pv2)
                ret = 1;

            return ret;
        }

        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="car1"></param>
        /// <param name="car2"></param>
        /// <returns></returns>
        [Obsolete("Do not call this method.")]
        public static int CompareCarByExhaust(EnumCollection.CarInfoForSerialSummary car1, EnumCollection.CarInfoForSerialSummary car2)
        {
            int ret = String.Compare(car1.Engine_Exhaust, car2.Engine_Exhaust);
            if (ret == 0)
            {
                double year1 = ConvertHelper.GetDouble(car1.CarYear);
                double year2 = ConvertHelper.GetDouble(car2.CarYear);
                if (year1 > year2)
                    ret = -1;
                else if (year1 < year2)
                    ret = 1;
                else
                {
                    ret = CompareTransmissionType(car1.TransmissionType, car2.TransmissionType);
                    if (ret == 0)
                    {
                        double price1 = ConvertHelper.GetDouble(car1.ReferPrice);
                        double price2 = ConvertHelper.GetDouble(car2.ReferPrice);
                        if (price1 > price2)
                            ret = 1;
                        else if (price2 > price1)
                            ret = -1;
                    }

                }
            }
            return ret;
        }
        /// <summary>
        /// ����˳�� ���\����״̬\����\������\ָ����
        /// </summary>
        /// <param name="car1"></param>
        /// <param name="car2"></param>
        /// <returns></returns>
        public static int CompareCarByYearAndSale(EnumCollection.CarInfoForSerialSummary car1, EnumCollection.CarInfoForSerialSummary car2)
        {
            int result = 0;
            int sale1 = 0;
            int sale2 = 0;
            if (car1.SaleState == "ͣ��")
                sale1 = 1;

            if (car2.SaleState == "ͣ��")
            {
                sale2 = 1;
            }

            double year1 = ConvertHelper.GetDouble(car1.CarYear);
            double year2 = ConvertHelper.GetDouble(car2.CarYear);
            if (year1 > year2)
                result = -1;
            else if (year1 < year2)
                result = 1;
            else
            {
                result = sale1 - sale2;
                if (result == 0)
                {
                    result = String.Compare(car1.Engine_Exhaust, car2.Engine_Exhaust);
                    if (result == 0)
                    {
                        result = CompareTransmissionType(car1.TransmissionType, car2.TransmissionType);
                        if (result == 0)
                        {
                            double price1 = ConvertHelper.GetDouble(car1.ReferPrice);
                            double price2 = ConvertHelper.GetDouble(car2.ReferPrice);
                            if (price1 > price2)
                                result = 1;
                            else if (price2 > price1)
                                result = -1;
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// ����˳�� ���\����\������\ָ����
        /// </summary>
        /// <param name="car1"></param>
        /// <param name="car2"></param>
        /// <returns></returns>
        public static int CompareCarByYear(EnumCollection.CarInfoForSerialSummary car1, EnumCollection.CarInfoForSerialSummary car2)
        {
            int ret = 0;
            double year1 = ConvertHelper.GetDouble(car1.CarYear);
            double year2 = ConvertHelper.GetDouble(car2.CarYear);
            if (year1 > year2)
                ret = -1;
            else if (year1 < year2)
                ret = 1;
            else
            {
                ret = String.Compare(car1.Engine_Exhaust, car2.Engine_Exhaust);
                if (ret == 0)
                {
                    ret = CompareTransmissionType(car1.TransmissionType, car2.TransmissionType);
                    if (ret == 0)
                    {
                        double price1 = ConvertHelper.GetDouble(car1.ReferPrice);
                        double price2 = ConvertHelper.GetDouble(car2.ReferPrice);
                        if (price1 > price2)
                            ret = 1;
                        else if (price2 > price1)
                            ret = -1;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// ������ҳ�����б�����
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        /// <returns></returns>
        public static int CompareRegionPrice(DataRow row1, DataRow row2)
        {
            int ret = 0;
            int year1 = ConvertHelper.GetInteger(row1["Car_YearType"]);
            int year2 = ConvertHelper.GetInteger(row2["Car_YearType"]);
            if (year1 > year2)
                ret = -1;
            else if (year1 < year2)
                ret = 1;
            else
            {
                double exhaust1 = 0.0;
                double exhaust2 = 0.0;
                Double.TryParse(row1["Engine_Exhaust"].ToString().Trim().Replace("L", ""), out exhaust1);
                Double.TryParse(row2["Engine_Exhaust"].ToString().Trim().Replace("L", ""), out exhaust2);
                if (exhaust1 > exhaust2)
                    ret = 1;
                else if (exhaust1 < exhaust2)
                    ret = -1;
                else
                {
                    string trans1 = row1["UnderPan_TransmissionType"].ToString();
                    string trans2 = row2["UnderPan_TransmissionType"].ToString();
                    ret = CompareTransmissionType(trans1, trans2);
                    if (ret == 0)
                    {
                        double price1 = ConvertHelper.GetDouble(row1["car_ReferPrice"]);
                        double price2 = ConvertHelper.GetDouble(row2["car_ReferPrice"]);
                        if (price1 > price2)
                            ret = 1;
                        else if (price2 > price1)
                            ret = -1;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// ��������ʽ����
        /// </summary>
        /// <param name="bodyForm1"></param>
        /// <param name="bodyForm2"></param>
        /// <returns></returns>
        public static int CompareBodyForm(string bodyForm1, string bodyForm2)
        {
            if (bodyForm1 == bodyForm2) return 0;

            string[] bodyForm = { "����","����", "suv", "mpv", "�ܳ�", "���г�", "�Ʊ���", "��糵", "����", "����", "΢��", "Ƥ��", "�ͳ�", "����","����" };
            int b1 = -1;
            int b2 = -1;
            int length = bodyForm.Length;
            for (int i = 0; i < length; i++)
            {
                if (bodyForm1.ToLower() == bodyForm[i])
                {
                    b1 = i;
                }
                if (bodyForm2.ToLower() == (bodyForm[i]))
                {
                    b2 = i;
                }
                if (b1 > -1 && b2 > -1)
                    break;
            }
            return b1 - b2;
        }

        /// <summary>
        /// �Ƚϱ���������
        /// </summary>
        /// <param name="trans1"></param>
        /// <param name="trans2"></param>
        /// <returns></returns>
        public static int CompareTransmissionType(string trans1, string trans2)
        {
            if (trans1.IndexOf("�ֶ�") > -1)
                trans1 = "a";
            else if (trans1.IndexOf("���Զ�") > -1)
                trans1 = "b";
            else if (trans1.IndexOf("�Զ�") > -1)
                trans1 = "c";
            else if (trans1.IndexOf("����һ��") > -1)
                trans1 = "d";
            else if (trans1.IndexOf("CVT") > -1)
                trans1 = "e";
            else trans1 = "f";



            if (trans2.IndexOf("�ֶ�") > -1)
                trans2 = "a";
            else if (trans2.IndexOf("���Զ�") > -1)
                trans2 = "b";
            else if (trans2.IndexOf("�Զ�") > -1)
                trans2 = "c";
            else if (trans2.IndexOf("����һ��") > -1)
                trans2 = "d";
            else if (trans2.IndexOf("CVT") > -1)
                trans2 = "e";
            else
                trans2 = "f";

            return String.Compare(trans1, trans2);
        }

        /// <summary>
        /// �������ַ���
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int CompareStringDesc(string str1, string str2)
        {
            return String.Compare(str1, str2) * -1;
        }
        /// <summary>
        /// ����Ʒ���������
        /// </summary>
        /// <param name="xelem1"></param>
        /// <param name="xelem2"></param>
        /// <returns></returns>
        public static int TreeBrandCompare(XmlElement xelem1, XmlElement xelem2)
        {
            if (xelem1.GetAttribute("Country") != xelem2.GetAttribute("Country")
                && xelem1.GetAttribute("") == "����")
            {
                return -1;
            }
            else if (xelem1.GetAttribute("Country") != xelem2.GetAttribute("Country")
                && xelem2.GetAttribute("") == "����")
            {
                return 1;
            }
            else if (xelem1.GetAttribute("Country").CompareTo(xelem2.GetAttribute("Country")) < 0)
            {
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// ������Ʒ���������
        /// </summary>
        /// <param name="xelem1"></param>
        /// <param name="xelem2"></param>
        /// <returns></returns>
        public static int TreeSerialCompare(XmlElement xelem1, XmlElement xelem2)
        {
            string xelem1Sale = xelem1.GetAttribute("CsSaleState");
            string xelem2Sale = xelem2.GetAttribute("CsSaleState");
            if (xelem1Sale == xelem2Sale && xelem1.GetAttribute("Spell").CompareTo(xelem2.GetAttribute("Spell")) < 0)
            {
                return -1;
            }
            else if (xelem1Sale == xelem2Sale)
            {
                return 1;
            }
            else if (xelem1Sale == "����")
            {
                return -1;
            }
            else if (xelem1Sale == "����" && xelem2Sale == "ͣ��")
            {
                return -1;
            }
            else if (xelem1Sale == "����" && xelem2Sale == "����")
            {
                return 1;
            }
            else if (xelem1Sale == "ͣ��")
            {
                return 1;
            }
            return 1;
        }
        /// <summary>
        /// ������Ʒ������
        /// </summary>
        /// <param name="cspe1"></param>
        /// <param name="cspe2"></param>
        /// <returns></returns>
        public static int TreeSerialCompare(CarSerialPhotoEntity cspe1, CarSerialPhotoEntity cspe2)
        {
            string xelem1Sale = cspe1.SaleState;
            string xelem2Sale = cspe2.SaleState;
            if (xelem1Sale == xelem2Sale && cspe1.Cs_spell.CompareTo(cspe2.Cs_spell) < 0)
            {
                return -1;
            }
            else if (xelem1Sale == xelem2Sale)
            {
                return 1;
            }
            else if (xelem1Sale == "����")
            {
                return -1;
            }
            else if (xelem1Sale == "����" && xelem2Sale == "ͣ��")
            {
                return -1;
            }
            else if (xelem1Sale == "����" && xelem2Sale == "����")
            {
                return 1;
            }
            else if (xelem1Sale == "ͣ��")
            {
                return 1;
            }
            return 1;
        }
        /// <summary>
        /// ��Ʒ�ƴ��ɷ���ʱ������
        /// </summary>
        /// <param name="xelem1"></param>
        /// <param name="xelem2"></param>
        /// <returns></returns>
        public static int SerialAskPublishTimeCompare(XmlElement xelem1, XmlElement xelem2)
        {
            string firstPublishTime = xelem1.GetAttribute("lasttime");
            string secondPublisTime = xelem2.GetAttribute("lasttime");

            if (string.IsNullOrEmpty(firstPublishTime)) return 1;
            if (string.IsNullOrEmpty(secondPublisTime)) return 1;

            DateTime firstDt = ConvertHelper.GetDateTime(firstPublishTime);
            DateTime secondDt = ConvertHelper.GetDateTime(secondPublisTime);

            int result = firstDt.CompareTo(secondDt);
            if (result > 0)
            {
                return -1;
            }
            else if (result == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
            return 1;
        }
        /// <summary>
        /// ��Ʒ�ƴ�����������
        /// </summary>
        /// <param name="xelem1"></param>
        /// <param name="xelem2"></param>
        /// <returns></returns>
        public static int SerialAskNumberCompare(XmlElement xelem1, XmlElement xelem2)
        {
            int firstNumber = ConvertHelper.GetInteger(xelem1.GetAttribute("countnum"));
            int secondeNumber = ConvertHelper.GetInteger(xelem2.GetAttribute("countnum"));
            if (firstNumber > secondeNumber)
            {
                return -1;
            }
            else if (firstNumber == secondeNumber)
            {
                return 0;
            }
            else
            {
                return 1;
            }
            return 1;
        }
        /// <summary>
        /// ��Ʒ��ͼƬ����ʱ������
        /// </summary>
        /// <param name="xelem1"></param>
        /// <param name="xelem2"></param>
        /// <returns></returns>
        public static int SerialImagePublishTimeCompare(XmlElement xelem1, XmlElement xelem2)
        {
            string firstPublishTime = xelem1.GetAttribute("T");
            string secondPublisTime = xelem2.GetAttribute("T");

            if (string.IsNullOrEmpty(firstPublishTime)) return 1;
            if (string.IsNullOrEmpty(secondPublisTime)) return 1;

            DateTime firstDt = ConvertHelper.GetDateTime(firstPublishTime);
            DateTime secondDt = ConvertHelper.GetDateTime(secondPublisTime);

            int result = firstDt.CompareTo(secondDt);
            if (result > 0)
            {
                return -1;
            }
            else if (result == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
            return 1;
        }
        /// <summary>
        /// ��Ʒ��ͼƬPV����
        /// </summary>
        /// <param name="xelem1"></param>
        /// <param name="xelem2"></param>
        /// <returns></returns>
        public static int SerialImagePvCompare(XmlElement xelem1, XmlElement xelem2)
        {
            int firstNumber = ConvertHelper.GetInteger(xelem1.GetAttribute("CsPV"));
            int secondeNumber = ConvertHelper.GetInteger(xelem2.GetAttribute("CsPV"));
            if (firstNumber > secondeNumber)
            {
                return -1;
            }
            else if (firstNumber == secondeNumber)
            {
                return 0;
            }
            else
            {
                return 1;
            }
            return 1;
        }
        /// <summary>
        /// �ȶ�ʡ��ʡ֮���˳��
        /// </summary>
        /// <param name="xelem1"></param>
        /// <param name="xelem2"></param>
        /// <returns></returns>
        public static int CompareProvinceOrder(XmlElement xelem1, XmlElement xelem2)
        {
            int newsNumber1 = ConvertHelper.GetInteger(xelem1.GetAttribute("hangqing"));
            int newsNumber2 = ConvertHelper.GetInteger(xelem2.GetAttribute("hangqing"));
            if (newsNumber1 > newsNumber2)
            {
                return -1;
            }
            else if (newsNumber1 == newsNumber2)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        /// <summary>
        /// ��Ʒ����ɫ ����
        /// ��ͼ��ǰ ��ͼ�ں� ��� �Ӵ�С  ɫֵ�Ӵ�С
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns></returns>
        public static int SerialColorCompare(SerialColorForSummaryEntity color1, SerialColorForSummaryEntity color2)
        {
            int result = EmptyCompare(color1.ImageUrl, color2.ImageUrl);
            if (result == 0)
            {
                result = color2.YearType - color1.YearType;
                if (result == 0)
                {
                    result = string.Compare(color1.ColorRGB, color2.ColorRGB);
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static int EmptyCompare(string s1, string s2)
        {
            int result = 0;
            if ((string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
                        || (!string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2)))
            {
                result = 0;
            }
            if (string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2))
                result = 1;
            if (!string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
                result = -1;
            return result;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="exhaust1"></param>
        /// <param name="exhaust2"></param>
        /// <returns></returns>
        public static int ExhaustCompare(string s1, string s2)
        {
            double exhaust1 = 0;
            double exhaust2 = 0;
            double.TryParse(s1.TrimEnd('L'), out exhaust1);
            double.TryParse(s2.TrimEnd('L'), out exhaust2);
            int result = 0;
            if (exhaust1 > exhaust2)
                result = 1;
            else if (exhaust1 < exhaust2)
                result = -1;
            return result;
        }
        /// <summary>
        /// �������� ���� ��T 
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static int ExhaustCompareNew(string s1, string s2)
        {
            double exhaust1 = 0;
            double exhaust2 = 0;
            double.TryParse(s1.TrimEnd('L', 'T'), out exhaust1);
            double.TryParse(s2.TrimEnd('L', 'T'), out exhaust2);
            int result = 0;
            if (exhaust1 > exhaust2)
                result = 1;
            else if (exhaust1 < exhaust2)
                result = -1;
            else
            {
                result = string.Compare(s1, s2);
            }
            return result;
        }
    }
}
