using System;

using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.BLL.IndexManager
{
   public class IndexFactory
    {
       /// <summary>
       /// 创建指数类
       /// </summary>
        /// <param name="indexType">指数类型</param>
        /// <returns>IIndex接口</returns>
       public static IAutoIndex Create(IndexType indexType)
       {
           switch (indexType)
           {
               case IndexType.Dealer:
                   return new DealerIndex();
               case IndexType.UV:
                   return new UVIndex();
               case IndexType.Sale:
                   return new SaleIndex();
               default:
                   throw new Exception("IndexFactory is not Create "+indexType);
           }
       }
    }
}
