namespace GamesEngine.Users;

public interface IUser
{
    public string Name { get; set; }
    public int ID { get; set; }
    public List<string> Friends { get; set; }
}

public class User : IUser
{
    public string Name { get; set; }
    public int ID { get; set; }
    public List<string> Friends { get; set; }
}