﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfPointsOfInterests
        {
            get
            {
                return PointsOfInterests.Count;
            }
        }
        public ICollection<PointOfInterestDto> PointsOfInterests { get; set; } = new List<PointOfInterestDto>();

    }
}