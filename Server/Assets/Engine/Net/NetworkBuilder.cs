using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkBuilder : IServerTCP
{

    //Ill leave this class for now.
    private readonly IEventLoopGroup m_bossgroup = new MultithreadEventLoopGroup();
    private readonly IEventLoopGroup m_workgroup = new MultithreadEventLoopGroup();
    private ServerBootstrap m_bootstrap = new ServerBootstrap();
    
    private readonly ChannelEventHandler m_channelEventHandler;
    private readonly IWorld m_world;

    public NetworkBuilder(ChannelEventHandler a_channelEventHandler, IWorld a_world)
    {
        m_channelEventHandler = a_channelEventHandler;
        m_world = a_world;

    }

    public async Task Initalize(int port)
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
            var playerSession = new PlayerSession(channel, m_world);
            channel.GetAttribute(ChannelEventHandler.SESSION_KEY).SetIfAbsent(playerSession);
            //childPipeline.AddLast(new LoggingHandler("SRV-CONN", LogLevel.TRACE));
            childPipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
            childPipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
            childPipeline.AddLast(m_channelEventHandler);
        }));
        //_world.SpawnMonsters();
        Debug.Log("Server is listening on port " + port);
        NetworkManager.channel = await m_bootstrap.BindAsync(port);
    }

    public void Start()
    {

        //Debug.Log("STARTING");
        //Initalize(5555).ConfigureAwait(false);

    }
}
