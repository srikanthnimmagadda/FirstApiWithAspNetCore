using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Dto
{
    public class PointOfInterestUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "The Name field is required.")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
