using DotNetty.Handlers.Logging;
using DotNetty.Transport.Channels;
using Engine.Interfaces;
using System;
using UnityEngine;

public class ChannelPipeLineHandler : ChannelInitializer<IChannel>, IConnectionManager
{

    public ChannelPipeLineHandler()
    {

    }


    //main socket init. 
    protected override void InitChannel(IChannel channel)
    {
        try
        {

            Debug.Log("Server socket binded.");
            //binnded server socket
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}
