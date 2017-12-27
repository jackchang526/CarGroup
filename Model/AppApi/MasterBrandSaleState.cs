using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.AppApi
{
    public class MasterBrandSaleState
    {
        public int bs_Id { get; set; }
        /// <summary>
        ///  -1:停销、0:待销、1:在销、2:待查
        /// </summary>
        public string CbSaleState { get; set; }
    }
}
