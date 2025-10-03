using Task5.Models;
using Task5.Models.User;

namespace Task5.Services.Users;

public interface IUserService
{
    public Task<PaginatedResult<User>> GetUserList(string? currentUserId, UserFilter filter);

    public User GetUser(string email);

    public void BlockUsers(IEnumerable<string> users);

    public void DeleteUsers(IEnumerable<string> users);
}