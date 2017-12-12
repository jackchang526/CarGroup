﻿using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.MongoDB;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BitAuto.CarChannel.BLL
{
    public class EvaluationBll
    {
        /// <summary>
        /// 根据车系ID获取车款的最新评测数据
        /// </summary>
        /// <param name="evaluationId"></param>
        /// <returns></returns>
        public List<EvaluationEntity> GetByStyleEvaluation(int evaluationId)
        {
            List<EvaluationEntity> list = new List<EvaluationEntity>();
            EvaluationDal evaluationDal = new EvaluationDal();
            DataSet ds = evaluationDal.GetByStyleEvaluation(evaluationId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EvaluationEntity item = new EvaluationEntity();
                    item.CarId = Convert.ToInt32(dr["StyleId"].ToString());
                    item.SerialId = Convert.ToInt32(dr["ModelId"].ToString());
                    item.EvaluationId = Convert.ToInt32(dr["EvaluationId"].ToString());
                    item.GroupId = Convert.ToInt32(dr["GroupId"].ToString());
                    item.GroupName = Convert.ToString(dr["GroupNmae"]).Trim();
                    item.Name = Convert.ToString(dr["Name"]).Trim();
                    item.PropertyId = Convert.ToInt32(dr["PropertyId"].ToString());
                    item.PropertyValue = Convert.ToString(dr["PropertyValue"]).Trim();
                    item.Unit = Convert.ToString(dr["Unit"]).Trim();
                    list.Add(item);
                }
            }
            return list;
        }
        public TestBaseEntity GetTestBaseEntityById(int evaluationId)
        {
            TestBaseEntity item = new TestBaseEntity();
            EvaluationDal evaluationDal = new EvaluationDal();
            DataSet ds = evaluationDal.GetTestBaseEntityById(evaluationId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<string> testerList = new List<string>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    item.ModelName = Convert.ToString(dr["ModelName"].ToString());
                    item.Year = Convert.ToString(dr["Year"].ToString());
                    item.StyleName = Convert.ToString(dr["StyleName"].ToString());
                    item.TestTime = Convert.ToDateTime(dr["EvaluatingTime"].ToString());
                    item.Site = Convert.ToString(dr["Site"]).Trim();
                    //item.Tester = Convert.ToString(dr["EditorsName"]).Trim();

                    //20171106 去掉括号中的部门（马保青）
                    string[] testArr = Convert.ToString(dr["EditorsName"]).Trim().Split(',', '、', '，');
                    foreach (string tester in testArr)
                    {
                        int index = tester.IndexOf('(');
                        if (index >= 0)
                        {
                            testerList.Add(tester.Substring(0, index));
                        }
                        else
                        {
                            testerList.Add(tester);
                        }

                    }
                    item.Tester = string.Join(",", testerList.ToArray());

                    item.EquipmentOperator = Convert.ToString(dr["EquipmentOperator"].ToString());
                    item.Kilometers = Convert.ToInt32(dr["Kilometers"]);
                    item.WeatherDesc = Convert.ToString(dr["WeatherDesc"].ToString());
                    item.Weather = Convert.ToString(dr["Weather"].ToString());
                    item.StyleId = Convert.ToInt32(dr["StyleId"]);
                    item.Wind = Convert.ToString(dr["Wind"].ToString());
                    item.Temperature = Convert.ToString(dr["Temperature"].ToString());
                    break;//只获取一条数据
                }
            }
            return item;
        }
        public T GetOne<T>(IMongoQuery query)
        {
            EvaluationDal evaluationDal = new EvaluationDal();
            return evaluationDal.GetOne<T>(query);
        }
        /// <summary>
        /// 查询指定字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public T GetOne<T>(IMongoQuery query, string[] fields, Dictionary<string, int> sortdic = null)
        {
            EvaluationDal evaluationDal = new EvaluationDal();
            return evaluationDal.GetOne<T>(query, fields, sortdic);
        }

        public List<int> GetExistReportEvaluationId()
        {
            List<int> list = new List<int>();
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, @"Data\ExistReportEvaluationIdList\EvaluationIdList.xml");
            try
            {
                if (File.Exists(xmlFile))
                {
                    XDocument doc = XDocument.Load(xmlFile);
                    var query = from p in doc.Element("Root").Element("EvaluationIdList").Elements("Item") select p;
                    query.ToList().ForEach(item =>
                    {
                        int evaluationId = ConvertHelper.GetInteger(item.Attribute("EvaluationId").Value);
                        list.Add(evaluationId);
                    });
                }
            }
            catch (Exception e)
            {
                CommonFunction.WriteLog(e.ToString());
            }
            return list;
        }

        public List<T> GetPingCeAllList<T>(IMongoQuery query, int top, Dictionary<string, int> sortdic = null, params string[] fields)
        {
            EvaluationDal evaluationDal = new EvaluationDal();
            return evaluationDal.GetPingCeAllList<T>(query, top,sortdic, fields);
        }

        public List<T> GetPingCeList<T>(IMongoQuery query, int index, int pageCount, out int total, Dictionary<string, int> sortdic = null, params string[] fields)
        {
            EvaluationDal evaluationDal = new EvaluationDal();
            return evaluationDal.GetPingCeList<T>(query, index, pageCount,out total, sortdic, fields);
        }
    }
}
