
using Engine.Net;

namespace Engine.Interfaces
{
   public interface IClientProvider
    {
        PlayerSession[] Clients { get; set; }
    }
}
