using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Task5.Database.Entities;
using Task5.Database.Repositories;
using Task5.Models;
using Task5.Models.User;
using Task5.Services.Authentication;
using Task5.Services.Exceptions;
using Task5.WebApi.Mapper;

namespace Task5.Services.Users;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository repository;
    private readonly UserManager<UserEntity> userManager;

    public UserService(IUserRepository repository, IMapper mapper, ILogger<UserService> logger, UserManager<UserEntity> userManager)
    : base(mapper, logger)
    {
        this.repository = repository;
        this.userManager = userManager;
    }

    public async Task<PaginatedResult<User>> GetUserList(string? currentUserId, UserFilter filter)
    {
        if (! (await this.CheckUserAccess(currentUserId)))
        {
            throw new AccessDeniedException("You have to login to non-blocked account to use the app.");
        }

        var users = await this.repository.GetUsers();

        return PaginationHelper.Paginate(
            users,
            filter.PageNumber,
            filter.PageSize,
            x => this.mapper.Map<User>(x)
        );
    }

    public User GetUser(string email)
    {
        throw new NotImplementedException();
    }

    public void BlockUsers(IEnumerable<string> users)
    {
        throw new NotImplementedException();
    }

    public void DeleteUsers(IEnumerable<string> users)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> CheckUserAccess(string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        var user = await this.userManager.FindByIdAsync(userId);

        if (user is null) return false;

        return !(user.UserStatus == (int)UserStatus.Blocked);
    }
}