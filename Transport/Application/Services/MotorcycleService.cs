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
    public class MotorcycleService(IMotorcycleRepository repository, IMapper mapper) : IMotorcycleService
    {
        private readonly IMotorcycleRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<MotorcycleDto>> GetAllMotorcyclesAsync()
        {
            var motos = await _repository.GetAllMotorcyclesAsync();

            var motosDto = _mapper.Map<IEnumerable<MotorcycleDto>>(motos);

            return motosDto;
        }

        public async Task<MotorcycleDto> GetMotorcycleByIdAsync(string id)
        {
            var moto = await _repository.GetMotorcycleByIdAsync(id);

            var motoDto = _mapper.Map<MotorcycleDto>(moto);

            return motoDto;
        }

        public async Task<Motorcycle> CreateMotorcycleAsync(CreateMotorcycleDto createMotoDto, string uid)
        {
            var moto = _mapper.Map<Motorcycle>(createMotoDto);

            moto.UserId = uid;

            await _repository.CreateMotorcycleAsync(moto);

            return moto;
        }

        public async Task DeleteMotorcycleAsync(string id, string uid)
        {
            var moto = await _repository.GetMotorcycleByIdAsync(id);

            if (!moto.UserId.Equals(uid))
                throw new DeletionIsNotAllowedException(uid);

            await _repository.DeleteMotorcycleAsync(moto);
        }


        public async Task UpdateMotorcycleAsync(string id, MotorcycleDto updateMotoDto, string uid)
        {
            var currentMoto = await _repository.GetMotorcycleByIdAsync(id);

            if (!currentMoto.UserId.Equals(uid))
                throw new UpdateIsNotAllowedException(uid);

            var moto = _mapper.Map<Motorcycle>(updateMotoDto);

            moto.Id = id;

            await _repository.UpdateMotorcycleAsync(moto);
        }
    }
}
