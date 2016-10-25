using IVA.DbAccess;
using IVA.DbAccess.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IVA.FindExpert.SignalRHub
{
    [Authorize]
    public class MessageHub : Hub
    {
        public MessageHub()
        {
            //var taskTimer = Task.Factory.StartNew(async () =>
            //{
            //    while (true)
            //    {
            //        string timeNow = DateTime.Now.ToString();
            //        //Sending the server time to all the connected clients on the client method SendServerTime()
            //        //Clients.All.SendServerTime(timeNow);

            //        //Delaying by 3 seconds.
            //        await Task.Delay(3000);
            //    }
            //}, TaskCreationOptions.LongRunning );
        }

        public override Task OnConnected()
        {
            //var username = Context.User.Identity.Name;
            HandleConnection();
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            //var username = Context.User.Identity.Name;
            HandleConnection();
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            HandleConnection(true);
            return base.OnDisconnected(stopCalled);
        }

        private void HandleConnection(bool ShouldDisconnect = false)
        {
            var userId = Context.User.Identity.GetUserId<long>();
            var connectionId = Context.ConnectionId;

            using (AppDBContext context = new AppDBContext())
            {
                var userRepo = new UserRepository(context);
                var user = userRepo.GetByUserId(userId);
                user.Connected = !ShouldDisconnect;
                user.ConnectionId = connectionId;
                userRepo.Update(user);
            }
        }

    }
}