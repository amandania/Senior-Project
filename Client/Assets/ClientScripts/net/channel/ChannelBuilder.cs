using UnityEngine;
using UnityEditor;
using System.Net;
using System;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels.Sockets;
using System.Threading.Tasks;
using Assets.ClientScripts.net.channel;
using Assets.ClientScripts.net.packets.outgoing;
using DotNetty.Common.Utilities;
using DotNetty.Codecs;

public class ChannelBuilder
{

    private IEventLoopGroup _workgroup = new MultithreadEventLoopGroup();
    public readonly ChannelEventHandler m_channelEventHandler = new ChannelEventHandler();
    private Bootstrap _bootstrap;

    public async Task InitClientTcp(string ip, int port)
    {
        InitializeBootstrap();
        NetworkManager.networkStream = await _bootstrap.ConnectAsync(IPAddress.Loopback, 5555);
        //NetworkManager.instance.SendPacket(new SendLoginRequest().CreteatePacket());
    }

    private void InitializeBootstrap()
    {
        _bootstrap = new Bootstrap();
        _bootstrap.Group(_workgroup);
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