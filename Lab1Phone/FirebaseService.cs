using Firebase.Database;
using Firebase.Database.Query;

namespace Lab1Phone;

public class FirebaseService
{
    private readonly FirebaseClient _firebase = new FirebaseClient("https://labphone-1490a-default-rtdb.europe-west1.firebasedatabase.app/");

    public async Task SaveUserAsync(User user)
    {
        await _firebase.Child("Users").PostAsync(user);
    }

    public async Task<User?> GetUserByLoginAsync(string login)
    {
        var users = await _firebase.Child("Users").OnceAsync<User>();
        return users.Select(x => x.Object).FirstOrDefault(u => u.Login == login);
    }

    public async Task<List<Book>> GetBooksByCategoryAsync(string category)
    {
        var books = await _firebase.Child("Books").OnceAsync<Book>();
        var list = books.Select(x => x.Object).ToList();

        if (category == "Всі")
            return list;

        return list.Where(b => b.Category == category).ToList();
    }

    public async Task SeedBooksIfEmptyAsync()
    {
        var books = await _firebase.Child("Books").OnceAsync<Book>();
        if (!books.Any())
        {
            await _firebase.Child("Books").PostAsync(new Book
            {
                Title = "Кобзар",
                Author = "Т. Шевченко",
                Pages = 720,
                Category = "Поезія",
                ImageUrl = "https://bookchef.ua/storage/products/18489/photo/main.jpeg",
                Description = "Збірка поетичних творів Тараса Шевченка, що стала символом української літератури."
            });
            await _firebase.Child("Books").PostAsync(new Book
            {
                Title = "Дюна",
                Author = "Ф. Герберт",
                Pages = 800,
                Category = "Фантастика",
                ImageUrl = "https://covers.openlibrary.org/b/isbn/9780441172719-L.jpg",
                Description = "Культовий науково-фантастичний роман про пустельну планету Арракіс та боротьбу за владу."
            });
            await _firebase.Child("Books").PostAsync(new Book
            {
                Title = "Шерлок Холмс",
                Author = "А. Конан Дойл",
                Pages = 350,
                Category = "Детектив",
                ImageUrl = "https://covers.openlibrary.org/b/isbn/9780140439083-L.jpg",
                Description = "Класичні детективні історії про видатного лондонського слідчого та доктора Ватсона."
            });
        }
    }
}