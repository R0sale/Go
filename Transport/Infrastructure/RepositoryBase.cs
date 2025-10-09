using Entities.Contracts;
using Entities.Dto;
using Entities.Exceptions;
using Entities.Models;
using Entities.Models.Transports;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<T>> FindAllAsync() => await _transport.Find(t => t.TransportType.ToString().Equals(typeOfRepository.Name)).ToListAsync();

        public async Task<IEnumerable<T>> FindByConditionAsync(Func<T, bool> expression) => (await FindAllAsync()).Where(expression);

        public async Task<IEnumerable<KeyValueDtoObject>> FindSelectedTransportByIdAsync(string id)
        {
            var props = typeOfRepository.GetProperties();

            var transport = (await FindByConditionAsync(transport => transport.Id.Equals(id))).FirstOrDefault();

            if (transport is null)
                throw new NotFoundException($"Transport with id {id} was not found.");

            var list = new List<KeyValueDtoObject>();

            foreach (var prop in props)
            {
                list.Add(new KeyValueDtoObject { Label = prop.Name, Value = prop.GetValue(transport).ToString() });
            }

            return list;
        }

        public async Task<IEnumerable<T>> FindByConditionAsync(Func<T, bool> expression) => (await FindAllAsync()).Where(expression);
        public async Task CreateAsync(T entity) => await _transport.InsertOneAsync(entity);
        public async Task DeleteAsync(T entity) => await _transport.DeleteOneAsync(transport => transport.Id.Equals(entity.Id));
        public async Task UpdateAsync(T entity) => await _transport.ReplaceOneAsync(transport => transport.Id.Equals(entity.Id), entity);
    }
}
