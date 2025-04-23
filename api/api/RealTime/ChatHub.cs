using Microsoft.AspNetCore.SignalR;

namespace api.RealTime
{
    public class ChatHub : Hub
    {
        // Dołącz gracza do pokoju
        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("SystemMessage", $"{Context.ConnectionId} joined {roomName}");
        }

        // Opuść pokój (opcjonalnie)
        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("SystemMessage", $"{Context.ConnectionId} left {roomName}");
        }

        // Wyślij wiadomość do konkretnego pokoju
        public async Task SendMessageToRoom(string roomName, string user, string message)
        {
            await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message);
        }
    }
}
