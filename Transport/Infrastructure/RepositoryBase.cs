using Entities.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Configuration;
using Entities.Models;

namespace Infrastructure
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : Transport
    {
        protected readonly IMongoCollection<T> _transport;
        protected readonly Type typeOfRepository = typeof(T);

        protected RepositoryBase(IOptions<TransportDatabaseSettings> transportDatabaseSettings)
        {
            var mongoClient = new MongoClient(transportDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(transportDatabaseSettings.Value.DatabaseName);

            _transport = mongoDatabase.GetCollection<T>(transportDatabaseSettings.Value.TransportCollectionName);
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            Console.WriteLine(typeOfRepository.Name);

            foreach (var t in await _transport.Find(t => t.Type.ToString().Equals(typeOfRepository.Name)).ToListAsync())
            {
                Console.WriteLine(t);
            }

            Console.WriteLine("Car".Equals(TransportType.Car.ToString()));

            foreach (var t in _transport.Find(t => t.Type.ToString().Equals(typeOfRepository.Name)).ToList())
            {
                Console.WriteLine(t);
            }

            return await _transport.Find(t => t.Type.ToString().Equals(typeOfRepository.Name)).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByConditionAsync(Func<T, bool> expression) => (await FindAllAsync()).Where(expression);

        public async Task CreateAsync(T entity) => await _transport.InsertOneAsync(entity);
        public async Task DeleteAsync(T entity) => await _transport.DeleteOneAsync(transport => transport.Id.Equals(entity.Id));
        public async Task UpdateAsync(T entity) => await _transport.ReplaceOneAsync(transport => transport.Id.Equals(entity.Id), entity);
    }
}
