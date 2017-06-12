using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace BitAuto.CarChannel.Model
{
    public class CarDealerList
    {
        public BsonObjectId Id { get; set; }
        public int CarId { get; set; }
        public List<DealerInfo> Dealers { get; set; }
        public CarDealerList()
        {
        }
    }
}
