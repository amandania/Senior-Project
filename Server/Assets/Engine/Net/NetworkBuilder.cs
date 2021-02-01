using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Engine.Interfaces;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkBuilder : IServerTCP
{

    //Ill leave this class for now.
    private IEventLoopGroup _bossgroup = new MultithreadEventLoopGroup();
    private IEventLoopGroup _workgroup = new MultithreadEventLoopGroup();
    private ServerBootstrap _bootstrap = new ServerBootstrap();

    private readonly IConnectionManager _connectionManager;
    private readonly ChannelEventHandler _channelEventHandler;
    private readonly IWorld _world;

    public NetworkBuilder(IConnectionManager connectionManager, ChannelEventHandler channelEventHandler, IWorld world)
    {
        _channelEventHandler = channelEventHandler;
        _connectionManager = connectionManager;
        _world = world;

    }

    public async Task Initalize(int port)
    {
        _bootstrap.Group(_bossgroup, _workgroup);
        _bootstrap.Channel<TcpServerSocketChannel>();
        _bootstrap.Option(ChannelOption.SoBacklog, 8192);
        _bootstrap.ChildOption(ChannelOption.SoKeepalive, true);

        //_bootstrap.Handler(new LoggingHandler("SRV-LSTN", LogLevel.TRACE));
        _bootstrap.ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
        {
            //Check docks
            var childPipeline = channel.Pipeline;
            var playerSession = new PlayerSession(channel, _world);
            channel.GetAttribute(ChannelEventHandler.SESSION_KEY).SetIfAbsent(playerSession);
            //childPipeline.AddLast(new LoggingHandler("SRV-CONN", LogLevel.TRACE));
            childPipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
            childPipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
            childPipeline.AddLast(_channelEventHandler);
        }));
        //_world.SpawnMonsters();
        Debug.Log("Server is listening on port " + port);
        NetworkManager.channel2 = await _bootstrap.BindAsync(port);
				}

    public void Start()
    {

        //Debug.Log("STARTING");
        //Initalize(5555).ConfigureAwait(false);

    }
}
