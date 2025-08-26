using AutoMapper;
using Entities.Contracts.Repositories;
using Entities.Contracts.Services;
using Entities.Dto;
using Entities.Models.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CarService(ICarRepository repository, IMapper mapper) : ICarService
    {
        private readonly ICarRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<CarDto>> GetAllCarsAsync()
        {
            var cars = await _repository.GetAllCarsAsync(trackChanges: false);

            var carsDto = _mapper.Map<IEnumerable<CarDto>>(cars);

            return carsDto;
        }

        public async Task<CarDto> GetCarByIdAsync(string id)
        {
            var car = await _repository.GetCarByIdAsync(id, trackChanges: false);

            var carDto = _mapper.Map<CarDto>(car);

            return carDto;
        }

        public async Task<Car> CreateCarAsync(CreateCarDto createCarDto)
        {
            var car = _mapper.Map<Car>(createCarDto);

            _repository.CreateCar(car);

            await _repository.SaveAsync();

            return car;
        }

        public async Task DeleteCarAsync(string id)
        {
            var car = await _repository.GetCarByIdAsync(id, trackChanges: false);

            _repository.DeleteCar(car);

            await _repository.SaveAsync();
        }

        public async Task UpdateCarAsync(string id, CarDto updateCarDto)
        {
            var car = await _repository.GetCarByIdAsync(id, trackChanges: true);

            _mapper.Map(updateCarDto, car);

            await _repository.SaveAsync();
        }
    }
}
