using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Common.MongoDB
{
    public interface ICollectionEntity
    {
        string GetCollectionName();

        string GetDataBaseName();
    }
}
