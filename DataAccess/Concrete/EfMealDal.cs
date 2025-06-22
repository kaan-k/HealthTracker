using Core.DataAccess;
using DataAccess.Abstract;
using Entities;
using DataAccess;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfMealDal : EfEntityRepositoryBase<Meal, AppDbContext>, IMealDal
    {
        
    }
}
