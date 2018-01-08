using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.AppApi
{
    public class TopNewCarEntity
    {
        public int ID { get; set; }
        public int MasterBrandId { get; set; }

        public string ShowName { get; set; }
        public string Img { get; set; }
        public string Price { get; set; }
        public string Level { get; set; }
        public string AllSpell { get; set; }

    }
}
