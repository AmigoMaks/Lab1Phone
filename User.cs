using SQLite;

namespace Lab1Phone;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }

    [Unique]
    public string Login { get; set; }

    public string PasswordHash { get; set; }
    public byte[] Salt { get; set; }
}