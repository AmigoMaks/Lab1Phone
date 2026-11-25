using SQLite;

namespace Lab1Phone;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        // Створюємо файл users.db3 у захищеній локальній папці телефону
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "users.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<User>().Wait();
    }

    // Збереження нового користувача
    public Task<int> SaveUserAsync(User user) => _database.InsertAsync(user);

    // Пошук користувача за логіном
    public Task<User> GetUserByLoginAsync(string login)
        => _database.Table<User>().Where(u => u.Login == login).FirstOrDefaultAsync();
}