using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    public class GroupScore
    {
        public string GroupName { get; set; }

        public double Score { get; set; }

        public Dictionary<string, string> ScoreDesc { get; set; }
        public Dictionary<string, string> CommonDesc { get; set; }
    }
}
