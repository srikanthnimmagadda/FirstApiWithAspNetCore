using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Dto
{
    public class PointOfInterestCreate
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage ="The Name field is required.")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
