using Capgemini.Slotmachine.BackgroundServices;
using Microsoft.AspNetCore.SignalR;

namespace Capgemini.Slotmachine.Hubs;

public class GameControllerHub : Hub
{
    public async Task ButtonPressed(int buttonId)
    {
        await Clients.All.SendAsync(nameof(ButtonPressed), new ButtonEvent(buttonId));
    }

    public async Task ButtonReleased(int buttonId)
    {
        await Clients.All.SendAsync("ReceiveMessage", new ButtonEvent(buttonId));
    }
}