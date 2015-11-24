using Microsoft.AspNet.SignalR;

namespace DrSharp.Web
{
    public class SharpHub : Hub
    {
        public void Send(string question, string answer)
        {
            Clients.All.broadcastAnswer(question, answer);
        }
    }
}