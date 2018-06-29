using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace CoreTestApp.Hubs
{
    public class ChatHub : Hub
    {
        public Task Broadcast(string message)
        {
            //--- 接続されている全ユーザーにメッセージを配信
            var timestamp = DateTime.Now.ToString();
            return this.Clients.All.SendAsync("Receive", message, timestamp);
        }
    }
}