using AutoMapper;
using Task5.Database.Repositories;
using Task5.Models.User;
using Task5.Services.Abstraction;

namespace Task5.Services;

public class UserService : BaseService, IUserSerivice
{
    private readonly IUserRepository repository;
    private readonly IMapper mapper;

    public UserService(IUserRepository repository, IMapper mapper) : base()
    {
        this.repository = repository;
        this.mapper = mapper;
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

    public void VerifyUser(string email)
    {
        throw new NotImplementedException();
    }
}