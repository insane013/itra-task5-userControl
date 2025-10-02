namespace Task5.Services.Authentication;

public class AuthenticationResult
{
    public bool Succeeded { get; init; }
    public IEnumerable<string> Errors { get; init; } = Enumerable.Empty<string>();

    public static AuthenticationResult Success => new AuthenticationResult { Succeeded = true };
}
