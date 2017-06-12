using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    public class DaoGouEntity
    {
        public string ImageUrl { get; set; }
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public string ReferPrice { get; set; }
        public int SerialId { get; set; }
        public string ShowName { get; set; }
        public string Description { get; set; }

        public string AllSpell { get; set; }

        /// <summary>
        /// 例如：轴距:2650mm;车高:1677mm;车宽:1852mm;行李箱容积:450L
        /// </summary>
        public string DataDescription { get; set; }
        public string ChannelId { get; set; }
    }
}
