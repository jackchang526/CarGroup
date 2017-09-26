using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    public class TestBaseEntity
    {
        /// <summary>
        /// 车系ID
        /// </summary>
        public int StyleId { get; set; }
        /// <summary>
        /// 车系名称
        /// </summary>
        public string ModelName { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public string Year { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StyleName { get; set; }        
        /// <summary>
        /// 测试时间
        /// </summary>
        public DateTime TestTime { get; set; }
        /// <summary>
        /// 测试地点
        /// </summary>
        public string Site { get; set; }
        /// <summary>
        /// 测试员
        /// </summary>
        public string Tester { get; set; }
        /// <summary>
        /// 仪器操作员
        /// </summary>
        public string EquipmentOperator { get; set; }
        /// <summary>
        /// 里程数
        /// </summary>
        public int Kilometers { get; set; }
        /// <summary>
        /// 天气描述
        /// </summary>
        public string WeatherDesc { get; set; }
        /// <summary>
        /// 天气
        /// </summary>
        public string Weather { get; set; }

        public string ShowName { get; set; }
        /// <summary>
        /// 风力
        /// </summary>
        public string Wind { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public string Temperature { get; set; }
    }
}
