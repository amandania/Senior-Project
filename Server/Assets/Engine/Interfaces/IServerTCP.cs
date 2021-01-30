using Autofac;
using System.Threading.Tasks;

namespace Engine.Interfaces
{
    public interface IServerTCP : IStartable
    {
							 Task Initalize(int port);
				}
}
