using AutoMapper;
using Business.Abstract;
using Core.Utilities.Validation;
using DataAccess.Abstract;
using Entities;
using Entities.Dtos;
using System.Collections.Generic;
using System.Linq;

public class WeightLogService : IWeightLogService
{
    private readonly IWeightLogDal _weightLogDal;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public WeightLogService(IWeightLogDal weightLogDal, IMapper mapper, IUserService userService)
    {
        _weightLogDal = weightLogDal;
        _mapper = mapper;
        //_userService = userService;
    }

    public void AddWeightLog(WeightLogDto dto)
    {
        ValidationHelper.ValidateObject(dto);
        dto.Date = DateTime.SpecifyKind(dto.Date.Date, DateTimeKind.Utc);

        var log = _mapper.Map<WeightLog>(dto);
        _weightLogDal.Add(log);
    }

    public void UpdateWeightLog(WeightLogDto dto)
    {
        ValidationHelper.ValidateObject(dto);
        dto.Date = DateTime.SpecifyKind(dto.Date.Date, DateTimeKind.Utc);

        var log = _mapper.Map<WeightLog>(dto);
        

        //_userService.UpdateUserWeight(log.User, dto.WeightKg);
        _weightLogDal.Update(log);
    }

    public void DeleteWeightLog(int id)
    {
        _weightLogDal.Delete(id);
    }

    public WeightLogDto GetById(int id)
    {
        var log = _weightLogDal.Get(w => w.Id == id);
        return _mapper.Map<WeightLogDto>(log);
    }

    public List<WeightLogDto> GetLogsByUserId(int userId)
    {
        var logs = _weightLogDal.GetAll(w => w.UserId == userId)
                    .OrderByDescending(w => w.Date)
                    .ToList();

        return _mapper.Map<List<WeightLogDto>>(logs);
    }
    public void Delete(int id)
    {
        _weightLogDal.Delete(id);
    }
}
