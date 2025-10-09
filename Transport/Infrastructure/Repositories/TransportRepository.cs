using Entities.Models;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Entities.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TransportRepository : ITransportRepository
    {
        private readonly IMongoCollection<Transport> _transport;

        public TransportRepository(IOptions<TransportDatabaseSettings> transportDatabaseSettings)
        {
            var mongoClient = new MongoClient(transportDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(transportDatabaseSettings.Value.DatabaseName);

            _transport = mongoDatabase.GetCollection<Transport>(transportDatabaseSettings.Value.TransportCollectionName);
        }

        public async Task<IEnumerable<Transport>> GetAllTransportAsync()
        {
            var transport = await (await _transport.FindAsync(new BsonDocument())).ToListAsync();

            return transport;
        }

        public async Task<IEnumerable<Transport>> FindUsersTransportAsync(string uid)
        {
            var transport = await (await _transport.FindAsync(t => t.UserId.Equals(uid))).ToListAsync();

            return transport;
        }
    }
}
