using Core.DataAccess;
using DataAccess.Abstract;
using Entities;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfWaterLogDal : EfEntityRepositoryBase<WaterLog, AppDbContext>, IWaterLogDal { }
}