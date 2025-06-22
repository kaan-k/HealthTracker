using Entities.Dtos;

namespace Business.Abstract
{
    public interface IUserService
    {
        void AddUser(UserDto dto);
        void UpdateUser(UserDto dto);
        void UpdateUserWeight(User user, double weight);
        void DeleteUser(int id);

        UserDto GetUserById(int id);
        List<UserDto> GetAllUsers();
    }
}
