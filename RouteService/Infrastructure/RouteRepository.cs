using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Infrastructure.Configuration;
using Entities.Models;
using MongoDB.Bson;
using AutoMapper;
using Entities.Contracts;

namespace Infrastructure
{
    public class RouteRepository : IRouteRepository
    {
        private readonly IMongoCollection<Route> _routes;

        public RouteRepository(IOptions<RoutesDatabaseSettings> opts)
        {
            var mongoClient = new MongoClient(opts.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(opts.Value.DatabaseName);

            _routes = mongoDatabase.GetCollection<Route>(opts.Value.RoutesCollectionName);
        }

        public async Task<IEnumerable<Route>> GetAllRoutesAsync() =>
            await _routes.Find(_ => true).ToListAsync();

        public async Task<Route?> GetRouteByIdAsync(string id) =>
            await _routes.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Route>> GetRoutesByOwnerUidAsync(string uid) =>
            await _routes.Find(x => x.OwnerUid.Equals(uid)).ToListAsync();

        public async Task CreateRouteAsync(Route route) => 
            await _routes.InsertOneAsync(route);

        public async Task DeleteRouteAsync(string id) =>
            await _routes.DeleteOneAsync(x => x.Id.Equals(id));

        public async Task UpdateRouteAsync(Route route) =>
            await _routes.ReplaceOneAsync(x => x.Id == route.Id, route);    
    }
}
