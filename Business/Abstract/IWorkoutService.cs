using Entities.Dtos;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IWorkoutService
    {
        void Add(WorkoutDto dto);
        void Update(WorkoutDto dto);
        void Delete(int id);
        List<WorkoutDto> GetWorkoutsByUserId(int userId);
        WorkoutDto GetById(int id);
    }
}
