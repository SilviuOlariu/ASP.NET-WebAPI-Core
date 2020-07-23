using CityInfo.Api.Entities;
using CityInfo.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Services
{
   public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        City GetCity(int cityId, bool IncludePointsOfInterest);
        IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId);
        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);
        bool CityExists(int cityId);
        void InsertPointOfInterest(int cityId, PointOfInterest pointOfInterest);
        void UpdatePointOfInterest(int cityId, PointOfInterest pointOfInterest);
        void DetelePointOfInterest(PointOfInterest pointOfInterest);
        bool Save();

    }
}
