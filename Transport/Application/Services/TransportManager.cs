using Entities.Contracts.Repositories;
using Entities.Contracts.Services;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Entities.Dto;

namespace Application.Services
{
    public class TransportManager(ITransportRepository transportRepository) : ITransportManager
    {
        private readonly ITransportRepository _transportRepository = transportRepository;

        public async Task<IEnumerable<Transport>> GetAllTransportAsync(Filter filter)
        {
            return (await _transportRepository.GetAllTransportAsync()).Where(t => t.TransportType.Equals(filter.Type));
        }

        public async Task<IEnumerable<Transport>> GetUsersTransport(string uid)
        {
            var transport = await _transportRepository.FindUsersTransportAsync(uid);

            return transport;
        }
    }
}
