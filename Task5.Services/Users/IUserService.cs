using Task5.Models;
using Task5.Models.User;

namespace Task5.Services.Users;

public interface IUserService
{
    public Task<PaginatedResult<User>> GetUserList(string? currentUserId, UserFilter filter);
    public Task BlockUsers(string? currentUserId, IEnumerable<string> emails);
    public Task UnblockUsers(string? currentUserId, IEnumerable<string> emails);

    public Task DeleteUsers(string? currentUserId, IEnumerable<string> emails);
    public Task DeleteUnverifiedUsers(string? currentUserId);
}