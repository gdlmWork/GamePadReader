﻿using Capgemini.Slotmachine.Hubs;
using Microsoft.AspNetCore.SignalR;
using Windows.Gaming.Input;

namespace Capgemini.Slotmachine.BackgroundServices
{
    public class GameControllerBackgroundService : BackgroundService
    {
        private readonly Func<RawGameController, bool> _gameControllerSelector;
        private readonly IHubContext<GameControllerHub> _hubContext;
        private readonly ILogger<GameControllerBackgroundService> _logger;

        private RawGameController? _gameController;
        private (ulong timestamp, bool[] buttons, GameControllerSwitchPosition[] switches, double[] axis) _lastReadState;

        public TimeSpan Resolution = TimeSpan.FromMilliseconds(10);

        public GameControllerBackgroundService(
            Func<RawGameController, bool> gameControllerSelector, 
            IHubContext<GameControllerHub> hubContext,
            ILogger<GameControllerBackgroundService> logger)
        {
            _gameControllerSelector = gameControllerSelector;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RawGameController.RawGameControllerAdded += (sender, controller) =>
            {
                if (_gameControllerSelector(controller))
                {
                    if (_gameController == null)
                    {
                        _logger.LogInformation("Game Controller connected.");
                        _gameController = controller;
                        _lastReadState = ReadController(_gameController);
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
                    _logger.LogInformation("Game Controller disconnected");
                    _gameController = null;
                }
            };

            // Need to enumerate RawGameControllers once before the game controller shows up... #strange...
            _logger.LogDebug($"{RawGameController.RawGameControllers.Count}");
            _gameController = RawGameController.RawGameControllers.FirstOrDefault(gc => _gameControllerSelector(gc));
            if (_gameController != null)
            {
                _lastReadState = ReadController(_gameController);
            }


            while (!stoppingToken.IsCancellationRequested)
            {
                if (_gameController != null)
                {
                    var (ts, buttons, switches, axis) = ReadController(_gameController);
                    if (ts > _lastReadState.timestamp)
                    {
                        for (var i = 0; i < _gameController.ButtonCount; i++)
                        {
                            if (buttons[i] && !_lastReadState.buttons[i])
                            {
                                await _hubContext.Clients.All
                                    .SendAsync("ButtonPressed", i, stoppingToken)
                                    .ConfigureAwait(false);
                                
                            }
                            else if (!buttons[i] && _lastReadState.buttons[i])
                            {
                                //POST
                                await _hubContext.Clients.All
                                    .SendAsync("ButtonReleased", i, stoppingToken)
                                    .ConfigureAwait(false);
                            }
                        }

                        _lastReadState = (ts, buttons, switches, axis);
                    }
                }

                await Task.Delay(Resolution, stoppingToken).ConfigureAwait(false);
            }
        }

        private static (ulong ts, bool[] buttons, GameControllerSwitchPosition[] switches, double[] axis) ReadController(RawGameController controller)
        {
            var buttons = new bool[controller.ButtonCount];
            var switches = new GameControllerSwitchPosition[controller.SwitchCount];
            var axis = new double[controller.AxisCount];
            var ts = controller.GetCurrentReading(buttons, switches, axis);
            return (ts, buttons, switches, axis);
        }
    }

    public record ButtonEvent
    {
        public int ButtonId { get; }
        
        public ButtonEvent(int id)
        {
            ButtonId = id;
        }
    }
}
