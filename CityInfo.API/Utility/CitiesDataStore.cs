using CityInfo.API.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Utility
{
    public class CitiesDataStore
    {
        /// <summary>
        /// 
        /// </summary>
        public static CitiesDataStore Default { get; } = new CitiesDataStore();

        /// <summary>
        /// 
        /// </summary>
        public List<City> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<City>()
            {
                new City
                {
                    Id = 1,
                    Name = "New York",
                    Description = "New York City is the most populous city in the United States. New York is also the most densely populated major city in the United States.",
                    PointsOfInterest = new List<PointOfInterest>()
                     {
                         new PointOfInterest() {
                             Id = 1,
                             Name = "Central Park",
                             Description = "The most visited urban park in the United States." },
                          new PointOfInterest() {
                             Id = 2,
                             Name = "Empire State Building",
                             Description = "A 102-story skyscraper located in Midtown Manhattan." },
                     }
                },
                 new City
                {
                    Id = 2,
                    Name = "California",
                    Description = "California City is known for having the third-largest land area of any city in the state of California.",
                    PointsOfInterest = new List<PointOfInterest>()
                     {
                         new PointOfInterest() {
                             Id = 3,
                             Name = "Cathedral of Our Lady",
                             Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans." },
                          new PointOfInterest() {
                             Id = 4,
                             Name = "Antwerp Central Station",
                             Description = "The the finest example of railway architecture in Belgium." },
                     }
                },
                  new City
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "Paris City is the capital and most populous city of France. Paris has been one of Europe's major centres of finance, diplomacy, commerce, fashion, science and the arts.",
                    PointsOfInterest = new List<PointOfInterest>()
                     {
                         new PointOfInterest() {
                             Id = 5,
                             Name = "Eiffel Tower",
                             Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel." },
                          new PointOfInterest() {
                             Id = 6,
                             Name = "The Louvre",
                             Description = "The world's largest museum." },
                     }
                }
            };
        }
    }
}
