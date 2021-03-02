using Autofac;
using DotNetty.Buffers;
using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;
using Engine.DataLoader;
using Engine.Entity.pathfinding;
using Serilog;
using Serilog.Extensions.Logging;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Networkmanager is the heart of the servers game builder and network creation.
/// Its a Monobehavior class because thats how unity triggers a startup file
/// The purpose of this class is to build the container with all dependencies the game relies on
/// Dependcies include all our Interafaces
/// </summary>
public class NetworkManager : MonoBehaviour
{

    public IWorld World { get; set; }

    public static IChannel channel { get; set; }
    

    /// <summary>
    /// This function will create our Container ten register all our server dependcies(Interfaces) into the container
    /// We also create our network logger to track any errors.
    /// Errors are only displayed if Visual Studio is attached to unity
    /// We also set our m_world to the registered single instance dependency of IWorld
    /// Finally we resolve the container with the ServerBooter to execute our inital threads
    /// </summary>
    private void Start()
    {
        var containerBuilder = new ContainerBuilder();

        RegisterDependencies(containerBuilder);

        var container = containerBuilder.Build();

        var loggerConfiguration = new LoggerConfiguration().WriteTo.Debug();
        loggerConfiguration.MinimumLevel.Verbose();
        var logger = loggerConfiguration.CreateLogger();

        Log.Logger = logger;

        logger.Information("Loading: " + nameof(Start));

        containerBuilder.RegisterInstance(logger).As<Serilog.ILogger>();

        //Setup Netty logger
        InternalLoggerFactory.DefaultFactory.AddProvider(new SerilogLoggerProvider(logger));


        IWorld k = World;
        container.TryResolve<IWorld>(out k);
        World = k;

        container.Resolve<ServerBooter>();
    }

    private void RegisterDependencies(ContainerBuilder builder)
    {
        // Boot
        builder.RegisterType<ServerBooter>();

        //Auto Startables
        //transform.gameObject.AddComponent<World>();
        builder.RegisterType<MapData>().As<ILoadMapData>().As<IStartable>().SingleInstance();
        builder.RegisterType<World>().As<IWorld>().As<IStartable>().SingleInstance();
        builder.RegisterType<NetworkBuilder>().As<IServerTCP>().As<IStartable>().SingleInstance();


        //Networking instances
        builder.RegisterType<ChannelEventHandler>().SingleInstance();
        builder.RegisterType<ChannelPipeLineHandler>().As<IConnectionManager>().SingleInstance();
        builder.RegisterType<ChannelPipeLineHandler>().As<IConnectionManager>().SingleInstance();
        //builder.RegisterType<NPCMovement>().As<INPCMovement>().SingleInstance();

        //Utility Dependencies
        builder.RegisterType<PathFinding>().As<IPathFinding>().SingleInstance();
        builder.RegisterType<PacketHandler>().As<IPacketHandler>().SingleInstance();
        builder.RegisterType<InputController>().As<IInputControl>().SingleInstance();

        //Packets
        builder.RegisterType<HandleMapLoaded>().As<IIncomingPackets>();
        builder.RegisterType<LoginResponsePacket>().As<IIncomingPackets>();
        builder.RegisterType<HandleLeftMouseClick>().As<IIncomingPackets>();
        builder.RegisterType<HandleMovementInput>().As<IIncomingPackets>();
        builder.RegisterType<HandleActionKeys>().As<IIncomingPackets>();
        builder.RegisterType<HandleChatMessage>().As<IIncomingPackets>();

        //Player startables we want to make sure all the other dependencies are built
        builder.RegisterType<PlayerData>().As<IPlayerDataLoader>().As<IStartable>().SingleInstance();

    }

    public async Task SendPacketToAll(IOutGoingPackets packet)
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt((int)packet.PacketType);
        buffer.WriteBytes(packet.GetPacket());

        foreach (var player in World.Players)
        {
            if (player.Session.m_channel.Active && player.Session.m_channel.IsWritable)
            {
                await player.Session.WriteToChannel(buffer.RetainedDuplicate()).ConfigureAwait(false);
            }
        }
    }
    
    // Update is called once per frame
    private void Update()
    {

    }

    private void OnApplicationQuit()
    {
        if (channel != null)
        {
            channel.CloseAsync();
        }
    }
}
