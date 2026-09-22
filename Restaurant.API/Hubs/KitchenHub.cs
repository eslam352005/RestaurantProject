using Microsoft.AspNetCore.SignalR;

namespace Restaurant.API.Hubs
{
    public class KitchenHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            var branchId = Context.GetHttpContext()?.Request.Query["branchId"];
            if(!string.IsNullOrEmpty(branchId))
            {
               await Groups.AddToGroupAsync(Context.ConnectionId, $"branch_{branchId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var branchId = Context.GetHttpContext()?.Request.Query["branchId"];
            if (!string.IsNullOrEmpty(branchId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"branch_{branchId}");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
