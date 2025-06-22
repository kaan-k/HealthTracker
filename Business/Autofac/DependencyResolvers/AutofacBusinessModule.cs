using Autofac;
using AutoMapper;
using Business;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
        builder.RegisterType<MealService>().As<IMealService>().SingleInstance();
        builder.RegisterType<WeightLogService>().As<IWeightLogService>().SingleInstance();
        builder.RegisterType<WorkoutService>().As<IWorkoutService>().SingleInstance();

        builder.RegisterType<WaterLogService>().As<IWaterLogService>().SingleInstance();
        builder.RegisterType<EfWaterLogDal>().As<IWaterLogDal>().SingleInstance();

        builder.RegisterType<EfUserDal>().As<IUserDal>().SingleInstance();
        builder.RegisterType<EfMealDal>().As<IMealDal>().SingleInstance();
        builder.RegisterType<EfWeightLogDal>().As<IWeightLogDal>().SingleInstance();
        builder.RegisterType<EfWorkoutDal>().As<IWorkoutDal>().SingleInstance();

       
    }
}
