using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LabsWebApp5.Helpers
{
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("Send", message);
        }
    }
}
