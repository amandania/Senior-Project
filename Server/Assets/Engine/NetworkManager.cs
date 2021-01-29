using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autofac;
using Engine;
using Serilog;
using DotNetty.Common.Internal.Logging;
using Serilog.Extensions.Logging;
using Engine.Interfaces;
using Engine.Net.Packet;
using Engine.DataLoader;
using Engine.Entity.pathfinding;
using DotNetty.Transport.Channels;
using DotNetty.Buffers;
using System.Threading.Tasks;

public class NetworkManager : MonoBehaviour {

				private IWorld m_world { get; set; }

				// Use this for initialization
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

								//Resolves startables
								container.Resolve<ServerBooter>();

								IWorld k;
								container.TryResolve<IWorld>(out k);
								Debug.Log("We have world of player count: " + k.m_players.Count);
								m_world = k;
								//Debug.Log("BOOTING");
				}
    public static IChannel channel2;
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
       // builder.RegisterType<NPCMovement>().As<INPCMovement>().SingleInstance();

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

        transform.gameObject.AddComponent<WorldHandler>();
    }

				public async Task SendPacketToAll(IOutGoingPackets packet)
				{
								var buffer = Unpooled.Buffer();
								buffer.WriteInt((int)packet.PacketType);
								buffer.WriteBytes(packet.GetPacket());

								foreach (var player in m_world.m_players)
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
        channel2.CloseAsync();
    }
}
