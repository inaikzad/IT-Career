using Diary.Models.User;

namespace Diary.Services;

public interface IUserService
{
    void Create(CreateUserDto user);

    void Login(LoginUserDto user);
    void Logout();
}