using System.Collections.Generic;

namespace CityInfo.API.Dto
{
    public class City : CityBase
    {
        /// <summary>
        ///
        /// </summary>
        public ICollection<PointOfInterest> PointOfInterest { get; set; } = new List<PointOfInterest>();

        /// <summary>
        /// 
        /// </summary>
        public int NumberOfPointsOfInterest
        {
            get { return PointOfInterest.Count; }
        }
    }
}
