using AutoMapper;
using Entities.Contracts.Repositories;
using Entities.Contracts.Services;
using Entities.Dto;
using Entities.Models.Transports;
using Entities.Exceptions;
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
            var cars = await _repository.GetAllCarsAsync();

            var carsDto = _mapper.Map<IEnumerable<CarDto>>(cars);

            return carsDto;
        }

        public async Task<CarDto> GetCarByIdAsync(string id)
        {
            var car = await _repository.GetCarByIdAsync(id);

            var carDto = _mapper.Map<CarDto>(car);

            return carDto;
        }

        public async Task<Car> CreateCarAsync(CreateCarDto createCarDto, string uid)
        {
            var car = _mapper.Map<Car>(createCarDto);

            car.UserId = uid;

            await _repository.CreateCarAsync(car);

            return car;
        }

        public async Task DeleteCarAsync(string id, string uid)
        {
            var car = await _repository.GetCarByIdAsync(id);

            if (!car.UserId.Equals(uid))
                throw new DeletionIsNotAllowedException(uid);

            await _repository.DeleteCarAsync(car);
        }

        public async Task UpdateCarAsync(string id, CarDto updateCarDto, string uid)
        {
            var currentCar = await _repository.GetCarByIdAsync(id);

            if (!currentCar.UserId.Equals(uid))
                throw new UpdateIsNotAllowedException(uid);

            var car = _mapper.Map<Car>(updateCarDto);

            car.Id = id;

            await _repository.UpdateCarAsync(car);
        }
    }
}
