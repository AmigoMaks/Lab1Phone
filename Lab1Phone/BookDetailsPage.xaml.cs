namespace Lab1Phone;

public partial class BookDetailsPage : ContentPage
{
    public BookDetailsPage(Book book)
    {
        InitializeComponent();

        BookCover.Source = book.ImageUrl;
        TitleLabel.Text = book.Title;
        AuthorLabel.Text = book.Author;
        PagesLabel.Text = $"Кількість сторінок: {book.Pages}";
        DescLabel.Text = book.Description;
    }
}