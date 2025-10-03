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
        await this.CheckUserAccess(currentUserId);

        var users = await this.repository.GetUsers();

        return PaginationHelper.Paginate(
            users,
            filter.PageNumber,
            filter.PageSize,
            x => this.mapper.Map<User>(x)
        );
    }

    public Task BlockUsers(string? currentUserId, IEnumerable<string> emails) =>
            UserEmailsProceed(currentUserId, emails, BlockUser);

    public Task UnblockUsers(string? currentUserId, IEnumerable<string> emails) =>
        UserEmailsProceed(currentUserId, emails, UnblockUser);

    public Task DeleteUsers(string? currentUserId, IEnumerable<string> emails) =>
        UserEmailsProceed(currentUserId, emails, DeleteUser);

    public async Task DeleteUnverifiedUsers(string? currentUserId)
    {
        await this.CheckUserAccess(currentUserId);
        
        var users = this.userManager.Users.Select(x => x).Where(x => x.EmailConfirmed == false);

        foreach (var user in users)
        {
            await this.DeleteUser(user);
        }
    }

    private async Task CheckUserAccess(string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new AccessDeniedException("You have to login to non-blocked account to use the app.");
        }

        var user = await this.userManager.FindByIdAsync(userId);

        if (user is null || user.UserStatus == (int)UserStatus.Blocked) throw new AccessDeniedException("You have to login to non-blocked account to use the app.");
    }

    private async Task<IEnumerable<UserEntity>> GetUsersByEmails(IEnumerable<string> emails)
    {
        var users = new List<UserEntity>();

        foreach (var email in emails)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user is not null) users.Add(user);
        }

        return users;
    }

    private async Task UserEmailsProceed(string? currentUserId, IEnumerable<string> emails,
                                        Func<UserEntity, Task> action)
    {
        await this.CheckUserAccess(currentUserId);

        var users = await this.GetUsersByEmails(emails);

        foreach (var user in users)
        {
            await action(user);
        }
    }

    private async Task BlockUser(UserEntity user)
    {
        this._logger.LogInformation($"Blocking {user.Email}");
        user.UserStatus = (int)UserStatus.Blocked;
        await this.userManager.SetLockoutEnabledAsync(user, true);
        await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        await this.userManager.UpdateAsync(user);
    }

    private async Task UnblockUser(UserEntity user)
    {
        this._logger.LogInformation($"Unblocking {user.Email}");
        user.UserStatus = user.EmailConfirmed ? (int)UserStatus.Active : (int)UserStatus.Unverified;
        await userManager.SetLockoutEndDateAsync(user, null);
        await this.userManager.UpdateAsync(user);
    }

    private async Task DeleteUser(UserEntity user)
    {
        this._logger.LogInformation($"Deleting {user.Email}");

        await userManager.DeleteAsync(user);
    }
}