using AutoMapper;
using Business.Abstract;
using Core.Utilities.Validation;
using DataAccess.Abstract;
using Entities.Dtos;

public class UserService : IUserService
{
    private readonly IUserDal _userDal;
    private readonly IMapper _mapper;

    public UserService(IUserDal userDal, IMapper mapper)
    {
        _userDal = userDal;
        _mapper = mapper;
    }

    public List<UserDto> GetAllUsers()
    {
        var users = _userDal.GetAll();
        return _mapper.Map<List<UserDto>>(users);
    }

    public UserDto GetUserById(int id)
    {
        var user = _userDal.Get(u => u.Id == id);
        return _mapper.Map<UserDto>(user);
    }

    public void AddUser(UserDto dto)
    {
        ValidationHelper.ValidateObject(dto);
        var user = _mapper.Map<User>(dto);
        _userDal.Add(user);
    }

    public void UpdateUser(UserDto dto)
    {
        ValidationHelper.ValidateObject(dto);
        var user = _mapper.Map<User>(dto);
        _userDal.Update(user);
    }

    public void DeleteUser(int id)
    {
        _userDal.Delete(id);
    }

    public void UpdateUserWeight(User user, double weight)
    {
        throw new NotImplementedException();
    }
}
