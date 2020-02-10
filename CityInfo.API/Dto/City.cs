using System.Collections.Generic;

namespace CityInfo.API.Dto
{
    public class City
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<PointOfInterest> PointsOfInterest { get; set; } = new List<PointOfInterest>();

        /// <summary>
        /// 
        /// </summary>
        public int NumberOfPointsOfInterest
        {
            get { return PointsOfInterest.Count; }
        }
    }
}
