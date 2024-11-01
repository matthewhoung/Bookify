namespace Bookify.Domain.Users;

public sealed class Role
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public ICollection<User> Users { get; init; } = new List<User>();

    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static readonly Role Registerd = new(1, "Registered");
}
