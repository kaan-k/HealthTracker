
using Autofac;
using System;
using System.Windows.Forms;
using AutoMapper;
using Core.Mapping;

namespace HealthTracker
{
    internal static class Program
    {
        public static IContainer Container { get; private set; }

        [STAThread]
        static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AutofacBusinessModule>();


            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            })).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                var context = c.Resolve<MapperConfiguration>();
                return context.CreateMapper();
            }).As<IMapper>().InstancePerLifetimeScope();

            builder.RegisterType<UserForm>().AsSelf();
            builder.RegisterType<MealForm>().AsSelf();
            builder.RegisterType<WeightLogForm>().AsSelf();
            builder.RegisterType<AddEditUserForm>().AsSelf();
            builder.RegisterType<AddEditMealForm>().AsSelf();
            builder.RegisterType<AddEditWeightLogForm>().AsSelf();
            builder.RegisterType<UserProgressForm>().AsSelf();
            Container = builder.Build();

            ApplicationConfiguration.Initialize();

            using (var scope = Container.BeginLifetimeScope())
            {
                var mainForm = scope.Resolve<UserForm>();
                Application.Run(mainForm);
            }
        }
    }
}
