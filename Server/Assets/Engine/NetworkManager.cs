using UnityEngine;
using Autofac;
using Engine;
using Serilog;
using DotNetty.Common.Internal.Logging;
using Serilog.Extensions.Logging;
using Engine.Interfaces;
using Engine.DataLoader;
using Engine.Entity.pathfinding;
using DotNetty.Transport.Channels;
using DotNetty.Buffers;
using System.Threading.Tasks;

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

public class NetworkManager : MonoBehaviour {

				private IWorld m_world { get; set; }

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
				void Start () {
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
								

								IWorld k = m_world;
								container.TryResolve<IWorld>(out k);
								m_world = k;

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

								foreach (var player in m_world.Players)
								{
												if (player.m_session._channel.Active && player.m_session._channel.IsWritable)
												{
																await player.m_session.WriteToChannel(buffer.RetainedDuplicate()).ConfigureAwait(false);
												}
								}
				}

				// Update is called once per frame
				void Update () {
		
				}

    private void OnApplicationQuit()
    {   
        channel.CloseAsync();
    }
}
