using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.AppApi
{
    [Serializable]
    public class CarSerialExtEntity
    {
        //Id, Name, CoverImageId, CoverImageUrl, WhiteCoverUrl, ImageCount, MinPrice, MaxPrice, DealerCount, PriceCount
        public int Id { get; set; }
        public string Name { get; set; }
        public int CoverImageId { get; set; }
        public string CoverImageUrl { get; set; }
        public string WhiteCoverUrl { get; set; }
        public int ImageCount { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int DealerCount { get; set; }
        public int PriceCount { get; set; }
    }
}
