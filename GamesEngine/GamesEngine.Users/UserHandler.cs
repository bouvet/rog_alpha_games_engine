using Newtonsoft.Json;

namespace GamesEngine.Users;

public interface IUserHandler
{
    public IUser? GetUser(int id);
}

public class UserHandler : IUserHandler
{
    private Dictionary<int, User> Users = new();

    public UserHandler()
    {
        using var r = new StreamReader("../GamesEngine.Users/Users.json");
        var json = r.ReadToEnd();
        var items = JsonConvert.DeserializeObject<List<User>>(json);

        foreach (var user in items)
        {
            Users.Add(user.ID, user);
        }
    }

    public IUser? GetUser(int id)
    {
        return Users.GetValueOrDefault(id % System.Math.Max(Users.Count, 1), null);
    }
}