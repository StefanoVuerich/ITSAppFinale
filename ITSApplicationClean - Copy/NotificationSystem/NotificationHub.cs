using Data;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace NotificationSystem
{
    public class NotificationHub : Hub
    {
        private static List<string> users = new List<string>();

        protected void GetUser_IP()
        {
            string VisitorsIPAddr = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
            }
            string text = "Your IP is: " + VisitorsIPAddr;
            Clients.All.showIp(text);
        }

        public override Task OnConnected()
        {
            string clientId = Context.ConnectionId;

            if (users.IndexOf(clientId) == -1)
            {
                users.Add(clientId);
            }
            ShowUsersOnLine();
            GetUser_IP();
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            string clientId = Context.ConnectionId;

            if (users.IndexOf(clientId) == -1)
            {
                users.Add(clientId);
            }
            ShowUsersOnLine();
            return base.OnReconnected();
        }

        public override Task OnDisconnected()
        {
            string clientId = Context.ConnectionId;

            if (users.IndexOf(clientId) > -1)
            {
                users.Remove(clientId);
            }
            ShowUsersOnLine();
            return base.OnDisconnected();
        }

        public void ShowUsersOnLine()
        {
            Clients.All.showUsersOnLine(users.Count);
        }

        public static void Notification(string notification)
        {
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            var clients = hubContext.Clients.All;
            hubContext.Clients.All.broadcastNotification(notification);
        }

        public void GetRedisNews(string lastClientReadedNews)
        {
            List<string> unsendedNotifications = (List<string>)RedisNotificationRepository.GetLastNotifications(lastClientReadedNews);
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            string connectionid = Context.ConnectionId;
            hubContext.Clients.Client(connectionid).incomingNotifications(unsendedNotifications);
        }
    }
}