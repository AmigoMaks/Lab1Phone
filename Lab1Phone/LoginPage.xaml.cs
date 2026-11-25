namespace Lab1Phone;

public partial class LoginPage : ContentPage
{
    private readonly FirebaseService _dbService;
    private IDispatcherTimer? _authTimer;
    private int _timeLeft = 60;

    public LoginPage(FirebaseService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _dbService.SeedBooksIfEmptyAsync();

        if (_authTimer == null)
        {
            _authTimer = Application.Current.Dispatcher.CreateTimer();
            _authTimer.Interval = TimeSpan.FromSeconds(1);
            _authTimer.Tick += OnAuthTimerTick;
            _authTimer.Start();
        }
    }

    private void OnAuthTimerTick(object? sender, EventArgs e)
    {
        _timeLeft--;
        TimeLabel.Text = $"Час на вхід: {_timeLeft} сек";

        if (_timeLeft <= 0)
        {
            _authTimer?.Stop();
            NameEntry.IsEnabled = false;
            LoginEntry.IsEnabled = false;
            PasswordEntry.IsEnabled = false;
            LoginBtn.IsEnabled = false;
            RegBtn.IsEnabled = false;
            GuestBtn.IsEnabled = false;
            StatusLabel.Text = "Час на авторизацію вичерпано!";
        }
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LoginEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            StatusLabel.Text = "Введіть логін та пароль!";
            return;
        }

        var user = await _dbService.GetUserByLoginAsync(LoginEntry.Text);
        if (user != null && PasswordHasher.VerifyPassword(PasswordEntry.Text, user.PasswordHash, user.Salt))
        {
            _authTimer?.Stop();
            AppState.CurrentRole = user.Role;
            AppState.CurrentUserName = user.Name;

            Application.Current.Windows[0].Page = new NavigationPage(new CatalogPage(_dbService));
        }
        else
        {
            StatusLabel.Text = "Невірний логін або пароль!";
        }
    }

    private async void OnRegisterClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(LoginEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            StatusLabel.Text = "Заповніть всі поля!";
            return;
        }

        if (await _dbService.GetUserByLoginAsync(LoginEntry.Text) != null)
        {
            StatusLabel.Text = "Цей логін вже зайнятий!";
            return;
        }

        string hash = PasswordHasher.HashPassword(PasswordEntry.Text, out byte[] salt);
        var newUser = new User
        {
            Name = NameEntry.Text,
            Login = LoginEntry.Text,
            PasswordHash = hash,
            Salt = salt,
            Role = "User"
        };

        await _dbService.SaveUserAsync(newUser);
        StatusLabel.Text = "Зареєстровано! Тепер увійдіть.";
    }

    private void OnGuestClicked(object? sender, EventArgs e)
    {
        _authTimer?.Stop();
        AppState.CurrentRole = "Guest";
        AppState.CurrentUserName = "Гість";
        Application.Current.Windows[0].Page = new NavigationPage(new CatalogPage(_dbService));
    }
}