using Entities.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Dtos;
using AutoMapper;

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
    }
}
