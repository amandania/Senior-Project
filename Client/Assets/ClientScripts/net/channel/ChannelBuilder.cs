using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Net;
using System.Threading.Tasks;

/// <summary>
/// This class is used to create our main network bootstrap using Netty.
/// 
/// <summary>
/// ChannelBuilder.InitClientTcp()
/// <param name="ip">Server Ip</param>
/// <param name="port">Server Port</param>
/// <returns>BootStrap connection using Netty</returns>
/// </summary>
/// 
/// <see cref="InitializeBootstrap"/> Will be called before we open our network stream so we can setup any header bytes to inital socket connection
/// </summary>
public class ChannelBuilder
{
    //Event Handling Group for our incoming packets
    private readonly IEventLoopGroup m_workgroup = new MultithreadEventLoopGroup();

    /// <summary>
    /// Main Event Handler for created network
    /// </summary>
    public readonly ChannelEventHandler m_channelEventHandler = new ChannelEventHandler();

    //Network bootstrap wrapper
    private Bootstrap _bootstrap;

    /// <summary>
    /// This function is used everytime we attempt to login. <see cref="LoginScreen.OnLogin"/>
    /// </summary>
    /// <param name="ip">The IP address server is hosted on.</param>
    /// <param name="port">The port server is hosted on.</param>
    /// <returns></returns>
    public async Task InitClientTcp(string ip, int port)
    {
        InitializeBootstrap();
        NetworkManager.networkStream = await _bootstrap.ConnectAsync(IPAddress.Loopback, 5555);
        //NetworkManager.instance.SendPacket(new SendLoginRequest().CreteatePacket());
    }

    /// <summary>
    /// Build our inital network channel. Optional header are allowed we just pass the required netty ones.
    /// Creates our bootstrap, assigns the mutli thread wrapper to our bootstrap
    /// Assigns inital TCPSocketChannel to our bootstrap.
    /// Lastly we add the Socket Handler which is our ChannelEventHandler class
    /// </summary>
    private void InitializeBootstrap()
    {
        _bootstrap = new Bootstrap();
        _bootstrap.Group(m_workgroup);
        _bootstrap.Channel<TcpSocketChannel>();
        //_bootstrap.Handler(m_channelPipelineHandler);

        _bootstrap.Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
        {
            var childPipe = channel.Pipeline;
            childPipe.AddLast("framing-enc", new LengthFieldPrepender(2));
            childPipe.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
            childPipe.AddLast(m_channelEventHandler);
        }));
    }
}