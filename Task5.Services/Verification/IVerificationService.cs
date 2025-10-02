
using Task5.Database.Entities;

namespace Task5.Services.Verification;

public interface IVerificationService
{
    public Task Verify(UserEntity user, string confirmActionUrl);
}