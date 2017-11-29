using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
     public class EvaluationTopRank
    {
        public int StyleId { get; set; }
        public int PropertyId { get; set; }
        public double PropertyValue { get; set; }
        public int EvaluationId { get; set; }        
        public string StyleName { get; set; }        
        public int Year { get; set; }
        public string ModelDisplayName { get; set; }
        public string ModelLevel { get; set; }       
        public string Unit { get; set; }        
        public string LevelSpell { get; set; }        
        public string ModelAllSpell { get; set; }
        public bool IsExistReport { get; set; }
        
    }
}
