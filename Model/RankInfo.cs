using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    public class RankInfo
    {
        public int LevelTotal { get; set; }
        public int LevelNum { get; set; }
        public string LevelName { get; set; }
        public string LevelAvg { get; set; }
        public string LevelBestName { get; set; }
        public string LevelBestValue { get; set; }
        public string LevelBestUnit { get; set; }
        public double Beat { get; set; }
    }
}
