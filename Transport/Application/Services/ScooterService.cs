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
    public class ScooterService(IScooterRepository repository, IMapper mapper) : IScooterService
    {
        private readonly IScooterRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ScooterDto>> GetAllScootersAsync()
        {
            var scooters = await _repository.GetAllScootersAsync();

            var scootersDto = _mapper.Map<IEnumerable<ScooterDto>>(scooters);

            return scootersDto;
        }

        public async Task<ScooterDto> GetScooterByIdAsync(string id)
        {
            var scooter = await _repository.GetScooterByIdAsync(id);

            var scooterDto = _mapper.Map<ScooterDto>(scooter);

            return scooterDto;
        }

        public async Task<Scooter> CreateScooterAsync(CreateScooterDto createScooterDto)
        {
            var scooter = _mapper.Map<Scooter>(createScooterDto);

            await _repository.CreateScooterAsync(scooter);

            return scooter;
        }

        public async Task DeleteScooterAsync(string id)
        {
            var scooter = await _repository.GetScooterByIdAsync(id);

            await _repository.DeleteScooterAsync(scooter);
        }

        public async Task UpdateScooterAsync(string id, ScooterDto updateScooterDto)
        {
            var scooter = _mapper.Map<Scooter>(updateScooterDto);

            scooter.Id = id;

            await _repository.UpdateScooterAsync(scooter);
        }
    }
}
