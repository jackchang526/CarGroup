using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    public class CarInnerSpaceEntity
    {
        public int CarId { get; set; }
        public double FirstseatToTop { get; set; }
        public double FirstSeatDistance { get; set; }
        public double SecondSeatToTop { get; set; }
        public double ThirdSeatToTop { get; set; }
        public double FirstSeatToTopModelHeight { get; set; }
        public double FirstSeatToTopModelWeight { get; set; }
        public double SecondSeatToTopModelHeight { get; set; }
        public double FirstSeatDistanceModelWeight { get; set; }
        public double FirstSeatDistanceModelHeight { get; set; }
        public double SecondSeatToTopModelWeight { get; set; }
        public double ThirdSeatToTopModelHeight { get; set; }
        public double ThirdSeatToTopModelWeight { get; set; }
        public int TrunkCapacity { get; set; }
        public int TrunkCapacityE { get; set; }
        public string FirstseatToTopImageUrl { get; set; }
        public string SecondSeatToTopImageUrl { get; set; }
        public string FirstSeatDistanceImageUrl { get; set; }
        public string ThirdSeatToTopImageUrl { get; set; }
        public string TrunkCapacityImageUrl { get; set; }
        public string TrunkCapacityEImageUrl { get; set; }
        public string FirstSeatToTopLevel { get; set; }
        public string FirstSeatDistanceLevel { get; set; }
        public string SecondSeatToTopLevel { get; set; }
        public string ThirdSeatToTopLevel { get; set; }
        public string TrunkCapacityLevel { get; set; }
    }
}
