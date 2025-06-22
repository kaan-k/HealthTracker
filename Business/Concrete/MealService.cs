using AutoMapper;
using Business.Abstract;
using Core.Utilities.Validation;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities;
using Entities.Dtos;
using System.Collections.Generic;

namespace Business
{
    public class MealService : IMealService
    {
        private readonly IMealDal _mealDal;
        private readonly IMapper _mapper;

        public MealService(IMealDal mealDal, IMapper mapper)
        {
            _mealDal = mealDal;
            _mapper = mapper;
        }

        public List<Meal> GetMealsByUserId(int userId)
        {
            return _mealDal.GetAll(m => m.UserId == userId);
        }

        public Meal GetById(int id)
        {
            return _mealDal.Get(m => m.Id == id);
        }

        public void Add(MealDto dto)
        {
            ValidationHelper.ValidateObject(dto);
            dto.Date = DateTime.SpecifyKind(dto.Date.Date, DateTimeKind.Utc);
            var meal = _mapper.Map<Meal>(dto);
            _mealDal.Add(meal);
        }

        public void Update(MealDto dto)
        {
            ValidationHelper.ValidateObject(dto);
            dto.Date = DateTime.SpecifyKind(dto.Date.Date, DateTimeKind.Utc);

            var meal = _mapper.Map<Meal>(dto);
            _mealDal.Update(meal);
        }

        public void Delete(int id)
        {
            _mealDal.Delete(id);
        }
    }
}
