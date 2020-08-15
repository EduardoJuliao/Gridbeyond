using System.Threading.Tasks;
using GridBeyond.Domain.EventArgs;

namespace GridBeyond.Service.Hubs
{
    public interface IMarketDataHub
    {
        Task OnValidRecord(ValidRecordEventArgs args);
    }
}