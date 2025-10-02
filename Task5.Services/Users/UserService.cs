using AutoMapper;
using Microsoft.Extensions.Logging;
using Task5.Database.Repositories;
using Task5.Models.User;

namespace Task5.Services.Users;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository repository;

    public UserService(IUserRepository repository, IMapper mapper, ILogger<UserService> logger) 
    : base(mapper, logger)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<User>> GetUserList()
    {
        var users = await this.repository.GetUsers();

        // Pagination & sorting logic here

        var userModels = users.Select(x => this.mapper.Map<User>(x));

        return userModels;
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
}