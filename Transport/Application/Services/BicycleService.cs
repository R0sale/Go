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

        public async Task<Bicycle> CreateBicycleAsync(CreateBicycleDto createBikeDto)
        {
            var bike = _mapper.Map<Bicycle>(createBikeDto);

            await _repository.CreateBicycleAsync(bike);

            return bike;
        }

        public async Task DeleteBicycleAsync(string id)
        {
            var bike = await _repository.GetBicycleByIdAsync(id);

            await _repository.DeleteBicycleAsync(bike);
        }

        public async Task UpdateBicycleAsync(string id, BicycleDto updateBikeDto)
        {
            var bike = _mapper.Map<Bicycle>(updateBikeDto);

            bike.Id = id;

            await _repository.UpdateBicycleAsync(bike);
        }
    }
}
