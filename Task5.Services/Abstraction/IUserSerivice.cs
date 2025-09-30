using Task5.Models.User;

namespace Task5.Services.Abstraction;

public interface IUserService
{
    public Task<IEnumerable<User>> GetUserList();

    public User GetUser(string email);

    public void BlockUsers(IEnumerable<string> users);

    public void DeleteUsers(IEnumerable<string> users);

    public void VerifyUser(string email);
}