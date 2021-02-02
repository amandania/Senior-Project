using Autofac;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Engine.DataLoader;
using Engine.Entity.pathfinding;
using System.Threading.Tasks;
using UnityEngine;

/*NetworkManager*/
/*
				NAME: 
												NetworkManager
				
				Description:
													Networkmanager is the heart of the network
													Its a Monobehavior class because thats how unity triggers a startup file
													The purpose of this class is to build the container with all dependencies the game relies on
													Dependcies include all our Interafaces
*/

public class NetworkManager : MonoBehaviour
{

    public IWorld World { get; set; }

    public static IChannel channel;


    /*void Start()*/
    /*
				NAME
												Start()

				DESCRIPTION
												This function will create our Container
												Then register all our server dependcies (Interfaces) into the container
												We also create our network logger to track any errors.
												  Errors are only displayed if Visual Studio is attached to unity
												We also set our m_world to the registered single instance dependency of IWorld
												Finally we resolve the container with the ServerBooter to execute our inital threads
				*/
    /*void Start()*/
    private void Start()
    {
        var containerBuilder = new ContainerBuilder();

        RegisterDependencies(containerBuilder);

        var container = containerBuilder.Build();

        //var loggerConfiguration = new LoggerConfiguration().WriteTo.Debug();
        //loggerConfiguration.MinimumLevel.Verbose();
        //var logger = loggerConfiguration.CreateLogger();

        //Log.Logger = logger;

        //logger.Information("Loading: " + nameof(Start));

        //containerBuilder.RegisterInstance(logger).As<Serilog.ILogger>();

        //Setup Netty logger
        //InternalLoggerFactory.DefaultFactory.AddProvider(new SerilogLoggerProvider(logger));


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

        builder.RegisterType<PathFinding>().As<IPathFinding>().SingleInstance();


        builder.RegisterType<ChannelEventHandler>().SingleInstance();
        builder.RegisterType<ChannelPipeLineHandler>().As<IConnectionManager>().SingleInstance();
        //builder.RegisterType<NPCMovement>().As<INPCMovement>().SingleInstance();

        //Extra
        builder.RegisterType<PacketHandler>().As<IPacketHandler>().SingleInstance();

        //Packets
        builder.RegisterType<HandleMapLoaded>().As<IIncomingPackets>();
        builder.RegisterType<LoginResponsePacket>().As<IIncomingPackets>();
        builder.RegisterType<IdleRequest>().As<IIncomingPackets>();
        //builder.RegisterType<InputKeyResponsePacket>().As<IIncomingPackets>();
        builder.RegisterType<HandleLeftMouseClick>().As<IIncomingPackets>();
        builder.RegisterType<HandleMovementInput>().As<IIncomingPackets>();
        builder.RegisterType<HandleActionKeys>().As<IIncomingPackets>();

    }

    public async Task SendPacketToAll(IOutGoingPackets packet)
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt((int)packet.PacketType);
        buffer.WriteBytes(packet.GetPacket());

        foreach (var player in World.Players)
        {
            if (player.Session._channel.Active && player.Session._channel.IsWritable)
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
        channel.CloseAsync();
    }
}
