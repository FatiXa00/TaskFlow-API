using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskFlow.Hubs;

[Authorize]
public class TaskHub : Hub
{
    // Appelé quand un client se connecte
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            // Ajoute l'utilisateur à son groupe personnel
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnConnectedAsync();
    }

    // Appelé quand un client se déconnecte
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnDisconnectedAsync(exception);
    }

    // Méthode pour notifier la création d'une tâche
    public async Task NotifyTaskCreated(string taskId, string title)
    {
        await Clients.All.SendAsync("TaskCreated", taskId, title);
    }

    // Méthode pour notifier la mise à jour d'une tâche
    public async Task NotifyTaskUpdated(string taskId, string status)
    {
        await Clients.All.SendAsync("TaskUpdated", taskId, status);
    }

    // Méthode pour notifier la suppression d'une tâche
    public async Task NotifyTaskDeleted(string taskId)
    {
        await Clients.All.SendAsync("TaskDeleted", taskId);
    }
}