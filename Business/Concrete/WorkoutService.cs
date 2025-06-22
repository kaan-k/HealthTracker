using AutoMapper;
using Business.Abstract;
using Core.Utilities.Validation;
using DataAccess.Abstract;
using Entities;
using Entities.Dtos;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutDal _workoutDal;
        private readonly IMapper _mapper;

        public WorkoutService(IWorkoutDal workoutDal, IMapper mapper)
        {
            _workoutDal = workoutDal;
            _mapper = mapper;
        }

        public void Add(WorkoutDto dto)
        {
            ValidationHelper.ValidateObject(dto);
            dto.Date = DateTime.SpecifyKind(dto.Date.Date, DateTimeKind.Utc);
            var workout = _mapper.Map<Workout>(dto);
            _workoutDal.Add(workout);
        }

        public void Update(WorkoutDto dto)
        {
            ValidationHelper.ValidateObject(dto);
            dto.Date = DateTime.SpecifyKind(dto.Date.Date, DateTimeKind.Utc);
            var workout = _mapper.Map<Workout>(dto);
            _workoutDal.Update(workout);
        }

        public void Delete(int id)
        {
            _workoutDal.Delete(id);
        }

        public List<WorkoutDto> GetWorkoutsByUserId(int userId)
        {
            var workouts = _workoutDal.GetAll(w => w.UserId == userId);
            return _mapper.Map<List<WorkoutDto>>(workouts);
        }

        public WorkoutDto GetById(int id)
        {
            var workout = _workoutDal.Get(w => w.Id == id);
            return _mapper.Map<WorkoutDto>(workout);
        }
    }
}
