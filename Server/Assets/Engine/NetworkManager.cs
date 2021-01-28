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

public class NetworkManager : MonoBehaviour {

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

        //Debug.Log("BOOTING");
    }
    public static IChannel channel2;
    private void RegisterDependencies(ContainerBuilder builder)
    {
        // Boot
        builder.RegisterType<ServerBooter>();

        //Auto Startables
        transform.gameObject.AddComponent<World>();
        builder.RegisterType<MapData>().As<ILoadMapData>().As<IStartable>().SingleInstance();
        builder.RegisterType<World>().As<IWorld>().As<IStartable>().SingleInstance();
        builder.RegisterType<NetworkBuilder>().As<IServerTCP>().As<IStartable>().SingleInstance();

        builder.RegisterType<PathFinding>().As<IPathFinding>().SingleInstance();


        builder.RegisterType<ChannelEventHandler>().SingleInstance();
        builder.RegisterType<ChannelPipeLineHandler>().As<IConnectionManager>().SingleInstance();
       // builder.RegisterType<NPCMovement>().As<INPCMovement>().SingleInstance();
        builder.RegisterType<MovementController>().As<IMovementController>().SingleInstance();

        //Extra
        builder.RegisterType<PacketHandler>().As<IPacketHandler>().SingleInstance();
        builder.RegisterType<MovementController>().As<IMovementController>().SingleInstance();

        //Packets
        builder.RegisterType<LoginResponsePacket>().As<IIncomingPackets>();
        builder.RegisterType<IdleRequest>().As<IIncomingPackets>();
        //builder.RegisterType<InputKeyResponsePacket>().As<IIncomingPackets>();
        builder.RegisterType<HandleMovementInput>().As<IIncomingPackets>();
        builder.RegisterType<HandleActionKeys>().As<IIncomingPackets>();
        builder.RegisterType<HandleMapLoaded>().As<IIncomingPackets>();

        transform.gameObject.AddComponent<WorldHandler>();
    }


    // Update is called once per frame
    void Update () {
		
	}
    private void OnApplicationQuit()
    {   
        channel2.CloseAsync();
    }
}
