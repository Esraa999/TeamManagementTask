using Microsoft.AspNet.SignalR;
using TeamManagementSystem.Models.ViewModels;

namespace TeamManagementSystem.Hubs
{
    /// <summary>
    /// ==> Added: SignalR Hub for real-time task notifications
    /// Broadcasts task updates to all connected clients
    /// Enables automatic UI updates without page refresh
    /// </summary>
    public class TaskNotificationHub : Hub
    {
        /// <summary>
        /// ==> Broadcast notification when a new task is created
        /// All connected clients will receive this notification
        /// </summary>
        /// <param name="taskDTO">The newly created task data</param>
        public void NotifyTaskCreated(TaskDTO taskDTO)
        {
            // Send to all connected clients
            Clients.All.taskCreated(taskDTO);
        }

        /// <summary>
        /// ==> Broadcast notification when a task is updated
        /// All connected clients will receive this notification
        /// </summary>
        /// <param name="taskDTO">The updated task data</param>
        public void NotifyTaskUpdated(TaskDTO taskDTO)
        {
            // Send to all connected clients
            Clients.All.taskUpdated(taskDTO);
        }

        /// <summary>
        /// ==> Broadcast notification when a task status changes
        /// All connected clients will receive this notification
        /// </summary>
        /// <param name="taskId">ID of the task whose status changed</param>
        /// <param name="newStatus">The new status value</param>
        public void NotifyTaskStatusChanged(int taskId, string newStatus)
        {
            // Send to all connected clients with task ID and new status
            Clients.All.taskStatusChanged(taskId, newStatus);
        }

        /// <summary>
        /// ==> Broadcast notification when a task is deleted
        /// All connected clients will receive this notification
        /// </summary>
        /// <param name="taskId">ID of the deleted task</param>
        public void NotifyTaskDeleted(int taskId)
        {
            // Send to all connected clients
            Clients.All.taskDeleted(taskId);
        }

        /// <summary>
        /// ==> Send notification to specific user only
        /// Useful for sending notifications to the assigned user
        /// </summary>
        /// <param name="userId">User ID to send notification to</param>
        /// <param name="message">Notification message</param>
        public void SendNotificationToUser(string userId, string message)
        {
            // Send to specific user (requires user to join group with their userId)
            Clients.Group(userId).receiveNotification(message);
        }

        /// <summary>
        /// ==> Called when a client connects to the hub
        /// Can be used to add client to specific groups
        /// </summary>
        public override System.Threading.Tasks.Task OnConnected()
        {
            // Get connection ID
            string connectionId = Context.ConnectionId;
            
            // In a real application, you would get the userId from authentication
            // and add the connection to a user-specific group
            // Example: Groups.Add(connectionId, userId);
            
            return base.OnConnected();
        }

        /// <summary>
        /// ==> Called when a client disconnects from the hub
        /// Can be used to remove client from groups and cleanup
        /// </summary>
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            // Cleanup logic here if needed
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// ==> Helper method to broadcast a general message to all clients
        /// Can be used for system-wide notifications
        /// </summary>
        /// <param name="message">Message to broadcast</param>
        public void BroadcastMessage(string message)
        {
            Clients.All.receiveMessage(message);
        }
    }
}
