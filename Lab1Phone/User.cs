using SQLite;

namespace Lab1Phone;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public byte[] Salt { get; set; }
    public string Role { get; set; }
}