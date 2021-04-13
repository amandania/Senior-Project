using Autofac;
using DotNetty.Buffers;
using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;
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

    /// <summary>
    /// This function is going to register everything the server treats as a container depency. Or a single instance pointer or a pointer to a pointer.
    /// All single instance are treated a pointer
    /// The incomingpackets are the only thing treated as Global Type to execute from. These are never passed into consrctors because that are executing to a player.
    /// </summary>
    /// <param name="a_builder">Main server container to build too</param>
    private void RegisterDependencies(ContainerBuilder a_builder)
    {
        // Boot
        a_builder.RegisterType<ServerBooter>();

        //Auto Startables
        //transform.gameObject.AddComponent<World>();
        a_builder.RegisterType<MapData>().As<ILoadMapData>().As<IStartable>().SingleInstance();
        a_builder.RegisterType<World>().As<IWorld>().As<IStartable>().SingleInstance();
        a_builder.RegisterType<NetworkBuilder>().As<IServerTCP>().As<IStartable>().SingleInstance();


        //Networking instances
        a_builder.RegisterType<ChannelEventHandler>().SingleInstance();
        a_builder.RegisterType<ChannelPipeLineHandler>().As<IConnectionManager>().SingleInstance();
        //builder.RegisterType<NPCMovement>().As<INPCMovement>().SingleInstance();

        //Utility Dependencies
        a_builder.RegisterType<PathFinding>().As<IPathFinding>().SingleInstance();
        a_builder.RegisterType<PacketHandler>().As<IPacketHandler>().SingleInstance();
        a_builder.RegisterType<InputController>().As<IInputControl>().SingleInstance();

        //Incoming Packets
        a_builder.RegisterType<HandleMapLoaded>().As<IIncomingPackets>();
        a_builder.RegisterType<LoginResponsePacket>().As<IIncomingPackets>();
        a_builder.RegisterType<HandleLeftMouseClick>().As<IIncomingPackets>();
        a_builder.RegisterType<HandleMovementInput>().As<IIncomingPackets>();
        a_builder.RegisterType<HandleActionKeys>().As<IIncomingPackets>();
        a_builder.RegisterType<HandleChatMessage>().As<IIncomingPackets>();
        a_builder.RegisterType<HandleLogoutRequest>().As<IIncomingPackets>();
        a_builder.RegisterType<HandleDialogueClick>().As<IIncomingPackets>();

        //Player startables we want to make sure all the other dependencies are built
        a_builder.RegisterType<PlayerData>().As<IPlayerDataLoader>().As<IStartable>().SingleInstance();

    }


    /// <summary>
    /// This function is used universally on that Unity Main Thread. Its to allow unity thread to send packet changes to everyone connected to the server. Apply global changes.
    /// </summary>
    /// <param name="a_packet"></param>
    /// <returns></returns>
    public async Task SendPacketToAll(IOutGoingPackets a_packet)
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt((int)a_packet.PacketType);
        buffer.WriteBytes(a_packet.GetPacket());

        foreach (var player in World.Players)
        {
            if (player.Session.Channel.Active && player.Session.Channel.IsWritable)
            {
                await player.Session.WriteToChannel(buffer.RetainedDuplicate()).ConfigureAwait(false);
            }
        }
    }
    
    // Update is called once per frame
    private void Update()
    {

    }

    /// <summary>
    /// Clean up funciton
    /// </summary>
    private void OnApplicationQuit()
    {
        if (channel != null)
        {
            channel.CloseAsync();
        }
    }
}
