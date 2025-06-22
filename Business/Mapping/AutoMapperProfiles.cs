using AutoMapper;
using Entities;
using Entities.Dtos;

namespace Core.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Meal, MealDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<WeightLog, WeightLogDto>().ReverseMap();
            CreateMap<Workout, WorkoutDto>().ReverseMap();
            CreateMap<WaterLog, WaterLogDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();



        }
    }
}
