using Microsoft.AspNetCore.SignalR;

namespace PollsApp.Hubs
{
    public class VoteHub : Hub
    {
        // Метод, который может вызываться клиентом для запроса обновления
        public async Task SendUpdate(int pollId)
        {
            await Clients.All.SendAsync("ReceiveResults", pollId);
        }
    }
}
