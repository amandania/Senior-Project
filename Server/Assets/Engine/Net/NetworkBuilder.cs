using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// This is the main network class. We create our Dotnetty classes here, and define our login headders with the required Dotnetty header values to all possible connections. Noramlly we would expand this and encrypt it with our own login headers making it so no other client versions can connect easily.
/// Its improtant to realize this is just setting up the global ChannelEventHalndler functions. to listen when the server network is notified to do so. Dotnetty is easy like that, takes care of the complicated mess to a multi threaded asynchornous network.
/// </summary>
public class NetworkBuilder : IServerTCP
{
    //Class workers to handle our packet reads independtly 
    private readonly IEventLoopGroup m_bossgroup = new MultithreadEventLoopGroup();
    private readonly IEventLoopGroup m_workgroup = new MultithreadEventLoopGroup();

    //Main server workd setup
    private ServerBootstrap m_bootstrap = new ServerBootstrap();
    
    //Channel instance and required depencies for each channel to be exposed to.
    private readonly ChannelEventHandler m_channelEventHandler;
    private readonly IWorld m_world;
    private readonly IPlayerDataLoader m_playerLoader;

    public NetworkBuilder(ChannelEventHandler a_channelEventHandler, IWorld a_world, IPlayerDataLoader a_playerLoader)
    {
        m_channelEventHandler = a_channelEventHandler;
        m_world = a_world;
        m_playerLoader = a_playerLoader;
    }

    /// <summary>
    /// This is our main server function to setup our network and all playersessions to be created on channel event invokes.
    /// </summary>
    /// <param name="a_port"></param>
    /// <returns>awaited Asynchronous task</returns>
    public async Task Initalize(int a_port)
    {
        m_bootstrap.Group(m_bossgroup, m_workgroup);
        m_bootstrap.Channel<TcpServerSocketChannel>();
        m_bootstrap.Option(ChannelOption.SoBacklog, 8192);
        m_bootstrap.ChildOption(ChannelOption.SoKeepalive, true);

        //_bootstrap.Handler(new LoggingHandler("SRV-LSTN", LogLevel.TRACE));
        m_bootstrap.ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
        {
            //Check docks
            var childPipeline = channel.Pipeline;
            var playerSession = new PlayerSession(channel, m_world, m_playerLoader);
            channel.GetAttribute(ChannelEventHandler.SESSION_KEY).SetIfAbsent(playerSession);
            //childPipeline.AddLast(new LoggingHandler("SRV-CONN", LogLevel.TRACE));
            childPipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
            childPipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
            childPipeline.AddLast(m_channelEventHandler);
        }));
        //_world.SpawnMonsters();
        Debug.Log("Server is listening on port " + a_port);
        NetworkManager.channel = await m_bootstrap.BindAsync(a_port);
    }

    public void Start()
    {

        //Debug.Log("STARTING");
        //Initalize(5555).ConfigureAwait(false);

    }
}
