using Entities.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Contracts;
using Entities.Dtos;
using AutoMapper;
using System.Transactions;

namespace Infrastructure
{
    public class FacilitiesRepository : IFacilityRepository
    {
        private readonly IMongoCollection<Facility> _facilities;
        private readonly IMapper _mapper;

        public FacilitiesRepository(IOptions<FacilitiesDatabaseSettings> facilitiesDatabaseSettings, IMapper mapper)
        {
            var mongoClient = new MongoClient(facilitiesDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(facilitiesDatabaseSettings.Value.DatabaseName);

            _facilities = mongoDatabase.GetCollection<Facility>(facilitiesDatabaseSettings.Value.FacilitiesCollectionName);

            var keys = Builders<Facility>.IndexKeys.Geo2D(f => f.Coordinates);
            _facilities.Indexes.CreateOne(new CreateIndexModel<Facility>(keys));

            _mapper = mapper;
        }

        public async Task<IList<Facility>> GetFacilitiesAsync() => await _facilities.Find(_ => true).ToListAsync();

        public async Task<Facility> GetFacilityAsync(string id) => await _facilities.Find(facility => facility.Id == id).FirstOrDefaultAsync();

        public async Task<Facility> CreateFacilityAsync(Facility newFacility)
        {
            await _facilities.InsertOneAsync(newFacility);

            return newFacility;
        }

        public async Task UpdateFacilityAsync(string id, Facility updatedFacility) => await _facilities.ReplaceOneAsync(facility => facility.Id == id, updatedFacility);

        public async Task RemoveFacilityAsync(string id) => await _facilities.DeleteOneAsync(facility => facility.Id == id);

        public async Task<IList<Facility>> GetFacilitiesNearByAsync(double longitude, double latitude, double radius)
        {
            const int DEGREE_IN_KM = 111;

            var radiusInDegrees = radius / DEGREE_IN_KM;

            var filter = Builders<Facility>.Filter.Near(f => f.Coordinates, longitude, latitude, maxDistance: radiusInDegrees);

            return await _facilities.Find(filter).ToListAsync();
        }
    }
}
