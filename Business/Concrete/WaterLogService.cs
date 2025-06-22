using AutoMapper;
using Business.Abstract;
using Core.Utilities.Validation;
using DataAccess.Abstract;
using Entities;
using Entities.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class WaterLogService : IWaterLogService
    {
        private readonly IWaterLogDal _dal;
        private readonly IMapper _mapper;

        public WaterLogService(IWaterLogDal dal, IMapper mapper)
        {
            _dal = dal;
            _mapper = mapper;
        }

        public void Add(WaterLogDto dto)
        {
            ValidationHelper.ValidateObject(dto);
            _dal.Add(_mapper.Map<WaterLog>(dto));
        }

        public void Update(WaterLogDto dto)
        {
            ValidationHelper.ValidateObject(dto);
            _dal.Update(_mapper.Map<WaterLog>(dto));
        }

        public void Delete(int id) => _dal.Delete(id);

        public WaterLogDto GetById(int id) => _mapper.Map<WaterLogDto>(_dal.Get(x => x.Id == id));

        public List<WaterLogDto> GetLogsByUserId(int userId)
        {
            return _mapper.Map<List<WaterLogDto>>(_dal.GetAll(x => x.UserId == userId));
        }
    }
}