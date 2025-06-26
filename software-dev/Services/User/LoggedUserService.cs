using Diary.Models.Entities;

namespace Diary.Services;

public class LoggedUserService
{

    private User user;

    public User User
    {
        get => this.user;
        set
        {
            user = value;
            IsLogged = user != null;
        }
    }
    public bool IsLogged { get; set; }
}