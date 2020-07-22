using CityInfo.Api.Contexts;
using CityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _ctx;
        public CityInfoRepository(CityInfoContext ctx)
        {
            _ctx = ctx;   
        }
        public IEnumerable<City> GetCities()
        {
           return _ctx.Cities.OrderBy(a => a.Name).ToList();
        }

        public City GetCity(int cityId, bool IncludePointsOfInterest)
        {
            if(IncludePointsOfInterest)
            {
                return _ctx.Cities.Include(a => a.PointsOfInterest).Where(c => c.Id == cityId).FirstOrDefault();
            }
            return _ctx.Cities.Where(c => c.Id == cityId).FirstOrDefault();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _ctx.PointsOfInterest.Where(p => p.Id == pointOfInterestId && p.CityId == cityId).FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return _ctx.PointsOfInterest.Where(a => a.CityId == cityId).ToList();
        }
        public bool CityExists(int cityId)
        {
            return _ctx.Cities.Any(c => c.Id == cityId);
        }
            
    }
}
