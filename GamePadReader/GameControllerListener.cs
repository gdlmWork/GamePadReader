using Microsoft.Extensions.Logging;
using Windows.Gaming.Input;

public class GameControllerListener
{
    private readonly Func<RawGameController, bool> _gameControllerSelector;
    private readonly ILogger<GameControllerListener> _logger;
    
    private RawGameController? _gameController;
    private (ulong timestamp, bool[] buttons, GameControllerSwitchPosition[] switches, double[] axis) _lastReadState;

    private CancellationTokenSource? _cts;
    private Task? _listenTask;

    public TimeSpan Resolution = TimeSpan.FromMilliseconds(100);
    
    public event EventHandler<ButtonEventArgs> ButtonPressed;
    public event EventHandler<ButtonEventArgs> ButtonReleased;
    // public event EventHandler<SwitchPositionChangedEventArgs> SwitchPositionChanged;

    public GameControllerListener(Func<RawGameController, bool> gameControllerSelector, ILogger<GameControllerListener> logger)
    {
        _gameControllerSelector = gameControllerSelector;
        _logger = logger;
        
        RawGameController.RawGameControllerAdded += (sender, controller) =>
        {
            if (_gameControllerSelector(controller))
            {
                if (_gameController == null)
                {
                    _logger.LogInformation("Game Controller connected.");
                    _gameController = controller;
                }
                else
                {
                    _logger.LogInformation("Game Controller already connected.");
                }
            }
        };
        RawGameController.RawGameControllerRemoved += (sender, controller) =>
        {
            if (controller == _gameController)
            {
                Console.WriteLine("Game Controller disconnected");
                _gameController = null;
            }
        };

        logger.LogDebug($"{RawGameController.RawGameControllers.Count}");
        _gameController = RawGameController.RawGameControllers.FirstOrDefault(gc => _gameControllerSelector(gc));
    }

    public void Start()
    {
        if (_cts != null)
        {
            throw new ArgumentException("Already started");
        }

        _cts = new CancellationTokenSource();
        _listenTask = Task.Run(ListenLoop, _cts.Token);
    }

    public void StopAsync()
    {
        if (_cts == null)
        {
            throw new ArgumentException("Not started");
        }
        _cts.Cancel();
        _listenTask?.Wait();
        _cts = null;
        _listenTask = null;
    }

    private async Task ListenLoop()
    {
        while (!_cts.IsCancellationRequested)
        {
            if (_gameController != null)
            {
                var buttons = new bool[_gameController.ButtonCount];
                var switches = new GameControllerSwitchPosition[_gameController.SwitchCount];
                var axis = new double[_gameController.AxisCount];
                var ts = _gameController.GetCurrentReading(buttons, switches, axis);
                if (ts > _lastReadState.timestamp)
                {
                    for (var i = 0; i < _gameController.ButtonCount; i++)
                    {
                        if (buttons[i] && !_lastReadState.buttons[i])
                        {
                            ButtonPressed?.Invoke(this, new ButtonEventArgs(i));
                        } 
                        else if (!buttons[i] && _lastReadState.buttons[i])
                        {
                            ButtonReleased?.Invoke(this, new ButtonEventArgs(i));
                        }
                    }

                    _lastReadState = (ts, buttons, switches, axis);
                }
            }

            await Task.Delay(Resolution).ConfigureAwait(false);
        }
    }
}