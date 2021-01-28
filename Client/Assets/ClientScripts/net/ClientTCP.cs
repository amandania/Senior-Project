using DotNetty.Transport.Channels;
using System;
using System.Net.Sockets;
using UnityEngine;

namespace Assets.ClientScripts.net
{

    public class ClientTCP
    {
        public IChannel m_channel;
      
        public void InitalizeNetwork()
        {
            var builder = new ChannelBuilder();
            builder.InitClientTcp(NetworkManager.instance.ipAddress, NetworkManager.instance.port).ConfigureAwait(false);
        }
    }
}
