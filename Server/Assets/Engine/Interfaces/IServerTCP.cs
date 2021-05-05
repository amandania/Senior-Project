using Autofac;
using System.Threading.Tasks;
/// <summary>
/// This server interface is used to implement the single instance depedency. <see cref="NetworkManager.RegisterDependencies(Autofac.ContainerBuilder)"/>
/// </summary>
public interface IServerTCP : IStartable
{
    //Server startup task 
    Task Initalize(int a_port);
}
