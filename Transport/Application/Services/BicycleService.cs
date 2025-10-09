using AutoMapper;
using Entities.Contracts.Repositories;
using Entities.Contracts.Services;
using Entities.Dto;
using Entities.Exceptions;
using Entities.Models.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BicycleService(IBicycleRepository repository, IMapper mapper) : IBicycleService
    {
        private readonly IBicycleRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<BicycleDto>> GetAllBicyclesAsync()
        {
            var bikes = await _repository.GetAllBicyclesAsync();

            var bikesDto = _mapper.Map<IEnumerable<Entities.Dto.BicycleDto>>(bikes);

            return bikesDto;
        }

        public async Task<BicycleDto> GetBicycleByIdAsync(string id)
        {
            var bike = await _repository.GetBicycleByIdAsync(id);
            var bikeDto = _mapper.Map<Entities.Dto.BicycleDto>(bike);
            return bikeDto;
        }

        public async Task<IEnumerable<KeyValueDtoObject>> GetSelectedBicycleById(string id)
        {
            return await _repository.FindSelectedTransportByIdAsync(id);
        }

        public async Task<Bicycle> CreateBicycleAsync(CreateBicycleDto createBikeDto, string uid)
        {
            var bike = _mapper.Map<Bicycle>(createBikeDto);

            bike.UserId = uid;

            await _repository.CreateBicycleAsync(bike);

            return bike;
        }

        public async Task DeleteBicycleAsync(string id, string uid)
        {
            var bike = await _repository.GetBicycleByIdAsync(id);

            if (!bike.UserId.Equals(uid))
                throw new DeletionIsNotAllowedException(uid);

            await _repository.DeleteBicycleAsync(bike);
        }

        public async Task UpdateBicycleAsync(string id, BicycleDto updateBikeDto, string uid)
        {
            var currentBicycle = await _repository.GetBicycleByIdAsync(id);

            if (!currentBicycle.UserId.Equals(uid)) 
                throw new UpdateIsNotAllowedException(uid);

            var bike = _mapper.Map<Bicycle>(updateBikeDto);

            bike.Id = id;

            await _repository.UpdateBicycleAsync(bike);
        }
    }
}
