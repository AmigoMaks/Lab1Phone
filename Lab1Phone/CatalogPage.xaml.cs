namespace Lab1Phone;

public partial class CatalogPage : ContentPage
{
    private readonly FirebaseService _dbService;

    public CatalogPage(FirebaseService dbService)
    {
        InitializeComponent();
        _dbService = dbService;

        UserInfoLabel.Text = $"Профіль: {AppState.CurrentUserName} ({AppState.CurrentRole})";

        if (AppState.CurrentRole == "Admin")
        {
            AdminBadge.IsVisible = true;
        }

        LoadBooks("Всі");
    }

    private async void LoadBooks(string category)
    {
        var books = await _dbService.GetBooksByCategoryAsync(category);
        BooksCollection.ItemsSource = books;
    }

    private void OnCategoryChanged(object? sender, EventArgs e)
    {
        if (CategoryPicker.SelectedItem is string selectedCategory)
        {
            LoadBooks(selectedCategory);
        }
    }

    private async void OnBookTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Book selectedBook)
        {
            await Navigation.PushAsync(new BookDetailsPage(selectedBook));
        }
    }
}