namespace Business.Interfaces.Auth;

public interface ICurrentUserService
{
    public Guid GetUserId();
}