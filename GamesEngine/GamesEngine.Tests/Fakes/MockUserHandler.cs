using GamesEngine.Users;

namespace GamesEngine.Tests.Fakes;

public class MockUserHandler : IUserHandler
{
    private IUser? _user;

    public MockUserHandler(IUser? user)
    {
        _user = user;
    }

    public IUser? GetUser(int id)
    {
        return _user;
    }
}