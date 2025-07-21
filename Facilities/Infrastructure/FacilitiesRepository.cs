using Entities.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts;

namespace Infrastructure
{
    public class FacilitiesRepository : IFacilityRepository
    {
        private readonly IMongoCollection<Facility> _facilities;

        public FacilitiesRepository(IOptions<FacilitiesDatabaseSettings> facilitiesDatabaseSettings)
        {
            var mongoClient = new MongoClient(facilitiesDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(facilitiesDatabaseSettings.Value.DatabaseName);

            _facilities = mongoDatabase.GetCollection<Facility>(facilitiesDatabaseSettings.Value.FacilitiesCollectionName); 
        }

        public async Task<IList<Facility>> GetFacilitiesAsync() => await _facilities.Find(_ => true).ToListAsync();

        public async Task<Facility> GetFacilityAsync(string id) => await _facilities.Find(facility => facility.Id == id).FirstOrDefaultAsync();

        public async Task CreateFacilityAsync(Facility newFacility) => await _facilities.InsertOneAsync(newFacility);

        public async Task UpdateFacilityAsync(string id, Facility updatedFacility) => await _facilities.ReplaceOneAsync(facility => facility.Id == id, updatedFacility);

        public async Task RemoveFacilityAsync(string id) => await _facilities.DeleteOneAsync(facility => facility.Id == id);
    }
}
