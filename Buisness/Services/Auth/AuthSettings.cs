namespace Buisness.Services.Auth;

public class AuthSettings
{
    public TimeSpan Expires { get; set; }
    public required string SecretKey { get; set;  }
}