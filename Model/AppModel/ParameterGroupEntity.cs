using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.AppModel
{
    [Serializable]
    public class ParameterGroupEntity
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        public List<ParameterGroupFieldEntity> Fields { get; set; }
    }
    [Serializable]
    public class ParameterGroupFieldEntity
    {
        public string Key { get; set; }
        public int ParamID { get; set; }
        public string Title { get; set; }
        public string Unit { get; set; }
    }
}
