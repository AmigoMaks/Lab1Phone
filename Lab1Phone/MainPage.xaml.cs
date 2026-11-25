namespace Lab1Phone;

public partial class MainPage : ContentPage
{
    private IDispatcherTimer _timer;
    private DateTime _startTime;
    private TimeSpan _elapsedTime = TimeSpan.Zero;
    private bool _isRunning = false;

    public MainPage()
    {
        InitializeComponent();

        _timer = Application.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(100);
        _timer.Tick += OnTimerTick;
    }

    private void OnTimerTick(object sender, EventArgs e)
    {
        if (_isRunning)
        {
            var currentElapsedTime = _elapsedTime + (DateTime.Now - _startTime);
            TimerLabel.Text = currentElapsedTime.ToString(@"hh\:mm\:ss\.f");
        }
    }

    private void OnStartTimerClicked(object sender, EventArgs e)
    {
        if (!_isRunning)
        {
            _startTime = DateTime.Now;
            _timer.Start();
            _isRunning = true;
        }
    }

    private void OnStopTimerClicked(object sender, EventArgs e)
    {
        if (_isRunning)
        {
            _elapsedTime += (DateTime.Now - _startTime);
            _timer.Stop();
            _isRunning = false;
        }
    }

    private void OnResetTimerClicked(object sender, EventArgs e)
    {
        _timer.Stop();
        _isRunning = false;
        _elapsedTime = TimeSpan.Zero;
        TimerLabel.Text = "00:00:00.0";
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedItem = LocationPicker.SelectedItem as string;

        if (selectedItem == "Місто")
            BackgroundImage.Source = "https://picsum.photos/id/122/1080/1920";
        else if (selectedItem == "Ліс")
            BackgroundImage.Source = "https://picsum.photos/id/28/1080/1920";
        else if (selectedItem == "Гори")
            BackgroundImage.Source = "https://picsum.photos/id/29/1080/1920";
    }
}