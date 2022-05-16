// See https://aka.ms/new-console-template for more information

using Windows.Gaming.Input;
using Microsoft.Extensions.Logging.Abstractions;

Console.WriteLine("Press Esc key to exit");

var cancellationTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) => cancellationTokenSource.Cancel();




var gameControllerSelector = (RawGameController controller) => controller.ButtonCount > 0;

var gcl = new GameControllerListener(gameControllerSelector, new NullLogger<GameControllerListener>())
{
    Resolution = TimeSpan.FromMilliseconds(10)
};

gcl.ButtonPressed += (sender, eventArgs) =>
{
    Console.WriteLine($"Button {eventArgs.ButtonIndex} pressed");
};

gcl.Start();

cancellationTokenSource.Token.WaitHandle.WaitOne();



// Console.WriteLine($"Controller Count: {RawGameController.RawGameControllers.Count}");
//
// var gameController = RawGameController.RawGameControllers.FirstOrDefault(gc => gameControllerSelector(gc));
// gameController = RawGameController.RawGameControllers.FirstOrDefault(gc => gameControllerSelector(gc));
// Console.WriteLine(gameController != null ? "Game Controller connected" : "Waiting for Game Controller");
//
// RawGameController.RawGameControllerAdded += (sender, controller) =>
// {
//     if (gameController == null && gameControllerSelector(controller))
//     {
//         Console.WriteLine("Game Controller connected");
//         gameController = controller;
//     }
// };
//
// RawGameController.RawGameControllerRemoved += (sender, controller) =>
// {
//     if (controller == gameController)
//     {
//         Console.WriteLine("Game Controller disconnected");
//         gameController = null;
//     }
// };
//
//
// ulong lastReadTimestamp = default;
// while (!cancellationTokenSource.IsCancellationRequested)
// {
//     if (gameController != null)
//     {
//         var buttons = new bool[gameController.ButtonCount];
//         var switches = new GameControllerSwitchPosition[gameController.SwitchCount];
//         var axis = new double[gameController.AxisCount];
//         var ts = gameController.GetCurrentReading(buttons, switches, axis);
//         if (ts > lastReadTimestamp)
//         {
//             Console.WriteLine($"{string.Join("", buttons.Select(x => x ? "1" : "0"))}");
//             lastReadTimestamp = ts;
//         }
//     }
//
//     await Task.Delay(100).ConfigureAwait(false);
// }


public class ButtonEventArgs : EventArgs
{
    public ButtonEventArgs(int buttonIndex)
    {
        ButtonIndex = buttonIndex;
    }

    public int ButtonIndex { get; }
}