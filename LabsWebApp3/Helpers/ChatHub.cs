using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using LabsWebApp3.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using static LabsWebApp3.Helpers.Config;

namespace LabsWebApp3.Helpers
{
    [Authorize]
    public class ChatHub : Hub
    {
        private const string prefix = "ИНФО: ";
        private readonly UserManager<IdentityUser> userManager;
        private readonly DataManager dataManager;

        private readonly ConnectionMapping<string> connections;

        public ChatHub(
            UserManager<IdentityUser> userManager,
            DataManager dataManager,
            ConnectionMapping<string> connections)
        {
            this.userManager = userManager;
            this.dataManager = dataManager;
            this.connections = connections;
        }

        public async Task SendMessage(string message, string recipient)
        {
            var user = Context.User.Identity.Name;
            if (recipient == default)
            {
                await Clients.All.SendAsync("ReceiveMessage",
                    $"{user}: " + message);
                //ЗДЕСЬ МОЖНО ПОПИЛИТЬ СОХРАНЕНИЕ СООБЩЕНИЙ
            }
            else
            {
                var recipientUser = await userManager.FindByNameAsync(recipient);
                if (recipientUser is null)
                {
                    await Clients.Clients(connections.GetConnections(user).ToList())
                        .SendAsync("ReceiveMessage",
                        $"(приватно){prefix}участник \"{recipient}\" не найден");
                    return;
                }

                await Clients.Clients(connections.GetConnections(user).ToList())
                    .SendAsync("ReceiveMessage",
                        $"(приватно для {recipient}): " + message);
                await Clients.Clients(connections.GetConnections(recipient).ToList())
                    .SendAsync("ReceiveMessage",
                    $"(приватно от {user}): " + message);
            }
        }

        public async Task SendBlock(string recipient, string tickStr)
        {
            const string impossible = " - блокировка невозможна";
            var recipientUser = await userManager.FindByNameAsync(recipient);
            var conns = connections.GetConnections(Context.User.Identity.Name).ToList();
            if (recipientUser is null)
            {
                await Clients.Clients(conns)
                    .SendAsync("ReceiveMessage",
                        $"{prefix}участник \"{recipient}\" не найден - блокировка невозможна");
                return;
            }
            var ticks = Convert.ToInt64(tickStr);
            if (await userManager.IsInRoleAsync(recipientUser, RoleAdmin))
            {
                await Clients.Clients(conns)
                    .SendAsync("ReceiveMessage",
                        $"{prefix}\"{recipient}\" является администратором{impossible}");
                return;
            }

            if (ticks < 0)
            {
                if (await userManager.IsInRoleAsync(recipientUser, RoleModerator))
                {
                    await Clients.Clients(conns)
                        .SendAsync("ReceiveMessage",
                        $"{prefix}\"{recipient}\" является модератором - нет возможности блокировки на всегда");
                    return;
                } 
                await userManager.RemoveFromRolesAsync(
                    recipientUser,
                    new[] { RoleReader, RoleWriter });
            }

            DateTime upto = DateTime.Now.AddTicks(ticks);
            var recipients = connections.GetConnections(recipient).ToList();
            
            if(recipients.Any())
                await Clients.Clients(recipients).SendAsync("ReceiveBlocked", upto);

            await dataManager.Functions.AddBlockAsync(recipientUser.Id, upto);

            await Clients.Clients(conns)
                .SendAsync("ReceiveMessage",
                    $"(приватно)\"{recipient}\" успешно заблокирован до {upto}");
        }

        public override async Task OnConnectedAsync()
        {
            connections.Add(Context.User.Identity.Name, Context.ConnectionId);
            if (connections.GetConnections(Context.User.Identity.Name).Count() == 1)
                await Clients.All.SendAsync("Notify",
                    $"{prefix}{Context.User.Identity.Name} вошел в чат");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            connections.Remove(Context.User.Identity.Name, Context.ConnectionId);
            if (!connections.GetConnections(Context.User.Identity.Name).Any())
                await Clients.All.SendAsync("Notify",
                    $"{prefix}{Context.User.Identity.Name} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
