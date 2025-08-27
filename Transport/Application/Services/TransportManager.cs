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
    public class TransportManager(ICarRepository carRepository, IBicycleRepository bicycleRepository, IMotorcycleRepository motorcycleRepository, IScooterRepository scooterRepository) : ITransportManager
    {
        private readonly ICarRepository _carRepository = carRepository;
        private readonly IBicycleRepository _bicycleRepository = bicycleRepository;
        private readonly IMotorcycleRepository _motorcycleRepository = motorcycleRepository;
        private readonly IScooterRepository _scooterRepository = scooterRepository;

        public async Task<IEnumerable<Transport>> GetAllTransportAsync(Filter filter)
        {
            switch (filter.Type)
            {
                case TransportType.Car:
                    return await _carRepository.GetAllCarsAsync();
                case TransportType.Bicycle:
                    return await _bicycleRepository.GetAllBicyclesAsync();
                case TransportType.Motorcycle:
                    return await _motorcycleRepository.GetAllMotorcyclesAsync();
                case TransportType.Scooter:
                    return await _scooterRepository.GetAllScootersAsync();
            }

            return null;
        }
    }
}
