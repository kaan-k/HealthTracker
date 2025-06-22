
using Core.DataAccess;
using DataAccess.Abstract;
using DataAccess;
using Entities;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, AppDbContext>, IUserDal
    {
    }
}
