using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.BLL
{
    public class TreeFactory
    {
        public TreeData GetTreeDataObject(string tagType)
        {
            switch (tagType)
            {
                case "chexing":
                    return new CarTree();
                case "daogou":
                    return new DaogouTree();
                case "pingce":
                    return new PingCeTree();
                case "tujie":
                    return new TuJieTree();
                case "hangqing":
                    return new HangQingTree();
                case "index":
                    return new CarIndex();
                case "xiaoliang":
                    return new SaleTree();
				//case "ucar":
				//	return new UCarTree();
                case "keji":
                    return new KeJiTree();
                case "anquan":
                    return new AnQuanTree();
                case "baoyang":
                    return new BaoYangTree();
                default :
                    return null;
            }
        }
    }
}
