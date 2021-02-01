using DotNetty.Handlers.Logging;
using DotNetty.Transport.Channels;
using System;
using UnityEngine;

public class ChannelPipeLineHandler : ChannelInitializer<IChannel>, IConnectionManager
{

    public ChannelPipeLineHandler()
    {

    }


    //main socket init. 
    protected override void InitChannel(IChannel a_channel)
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
