using Core.DataAccess;
using DataAccess.Abstract;
using Entities;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfWorkoutDal : EfEntityRepositoryBase<Workout, AppDbContext>, IWorkoutDal
    {
    }
}
