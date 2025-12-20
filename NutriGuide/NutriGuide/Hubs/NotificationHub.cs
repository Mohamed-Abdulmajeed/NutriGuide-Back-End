using Microsoft.AspNetCore.SignalR;

namespace NutriGuide.Hubs
{
    public class NotificationHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }
        public async Task JoinCustomerGroup(int customerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Customer-{customerId}");
        }

    }
}
