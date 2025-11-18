using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using TeamManagementSystem.Models;

namespace TeamManagementSystem.Hubs
{
    /// <summary>
    /// ==> SignalR Hub: Handles real-time communication for task updates
    /// ==> Broadcasts changes to all connected clients without requiring page refresh
    /// </summary>
    public class TaskHub : Hub
    {
        /// <summary>
        /// ==> Broadcast notification when a new task is created
        /// ==> All connected clients will receive this update
        /// </summary>
        /// <param name="task">The newly created task object</param>
        public void NotifyTaskCreated(object task)
        {
            // ==> Broadcast to ALL clients that a new task was created
            Clients.All.taskCreated(task);
        }

        /// <summary>
        /// ==> Broadcast notification when a task is updated
        /// ==> Includes status changes, assignment changes, etc.
        /// </summary>
        /// <param name="task">The updated task object</param>
        public void NotifyTaskUpdated(object task)
        {
            // ==> Broadcast to ALL clients that a task was updated
            Clients.All.taskUpdated(task);
        }

        /// <summary>
        /// ==> Broadcast notification when a task is deleted
        /// </summary>
        /// <param name="taskId">ID of the deleted task</param>
        public void NotifyTaskDeleted(int taskId)
        {
            // ==> Broadcast to ALL clients that a task was deleted
            Clients.All.taskDeleted(taskId);
        }

        /// <summary>
        /// ==> Broadcast notification when a task is assigned to a user
        /// ==> Specific user will see the new task in their list
        /// </summary>
        /// <param name="task">The task object with assignment details</param>
        /// <param name="userId">ID of the user the task was assigned to</param>
        public void NotifyTaskAssigned(object task, int userId)
        {
            // ==> Broadcast to ALL clients about the assignment
            Clients.All.taskAssigned(task, userId);
        }

        /// <summary>
        /// ==> Called when a client connects to the hub
        /// ==> Can be used for logging or connection management
        /// </summary>
        public override System.Threading.Tasks.Task OnConnected()
        {
            // ==> Log connection (optional)
            string connectionId = Context.ConnectionId;
            System.Diagnostics.Debug.WriteLine($"Client connected: {connectionId}");

            return base.OnConnected();
        }

        /// <summary>
        /// ==> Called when a client disconnects from the hub
        /// </summary>
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            // ==> Log disconnection (optional)
            string connectionId = Context.ConnectionId;
            System.Diagnostics.Debug.WriteLine($"Client disconnected: {connectionId}");

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// ==> Called when a client reconnects to the hub
        /// </summary>
        public override System.Threading.Tasks.Task OnReconnected()
        {
            // ==> Log reconnection (optional)
            string connectionId = Context.ConnectionId;
            System.Diagnostics.Debug.WriteLine($"Client reconnected: {connectionId}");

            return base.OnReconnected();
        }
    }
}
