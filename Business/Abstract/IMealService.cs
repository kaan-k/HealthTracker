using Entities;
using Entities.Dtos;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IMealService
    {
        List<Meal> GetMealsByUserId(int userId);
        Meal GetById(int id);
        void Add(MealDto dto);
        void Update(MealDto dto);
        void Delete(int id);
    }
}
