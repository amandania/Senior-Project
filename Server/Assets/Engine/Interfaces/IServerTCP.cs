using Autofac;
using System.Threading.Tasks;

public interface IServerTCP : IStartable
{
    Task Initalize(int port);
}
