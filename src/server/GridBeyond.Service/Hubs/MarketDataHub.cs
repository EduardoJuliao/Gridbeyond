using System;
using System.Threading.Tasks;
using GridBeyond.Domain.EventArgs;
using Microsoft.AspNetCore.SignalR;

namespace GridBeyond.Service.Hubs
{
    public class MarketDataHub : Hub, IMarketDataHub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task OnValidRecord(ValidRecordEventArgs args)
        {
            await Clients.Caller.SendAsync("OnValidRecord", args);
        }
    }
}