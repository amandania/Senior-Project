using UnityEngine;
using Autofac;
using Engine;
using Serilog;
using DotNetty.Common.Internal.Logging;
using Serilog.Extensions.Logging;
using Engine.Interfaces;
using Engine.Net.Packet;
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
        //transform.gameObject.AddComponent<World>();

        builder.RegisterType<World>().As<IWorld>().As<IStartable>().SingleInstance();
        builder.RegisterType<NetworkBuilder>().As<IServerTCP>().As<IStartable>().SingleInstance();

        builder.RegisterType<ChannelEventHandler>().SingleInstance();
        builder.RegisterType<ChannelPipeLineHandler>().As<IConnectionManager>().SingleInstance();

        //Register the Packet Handler first before we do the types
        builder.RegisterType<PacketHandler>().As<IPacketHandler>().SingleInstance();

        //Incoming Client Packets Type
        builder.RegisterType<LoginResponsePacket>().As<IIncomingPackets>();
        builder.RegisterType<IdleRequest>().As<IIncomingPackets>();
        builder.RegisterType<HandleMovementInput>().As<IIncomingPackets>();
        builder.RegisterType<HandleActionKeys>().As<IIncomingPackets>();
        builder.RegisterType<HandleMapLoaded>().As<IIncomingPackets>();

								Debug.Log("Game booting up");
    }


    // Update is called once per frame
    void Update () {
		
	}
    private void OnApplicationQuit()
    {   
        channel2.CloseAsync();
    }
}
