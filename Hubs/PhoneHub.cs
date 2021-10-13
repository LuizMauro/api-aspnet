using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MeuTodo.Hubs
{
    public class PhoneHub : Hub
    {

        public Task SendEvent(string eventName,string message)
        {
            return Clients.All.SendAsync(eventName, message);
        }



        public Task NotificationByIdUser(string eventName, string message)
        {
            var id = 1;
            return Clients.All.SendAsync("notification-by-id-${id}", message);
        }
    }
}
