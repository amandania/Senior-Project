using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// Anytime we have a health bar to change we will apply the value changes here.
/// </summary>

public class HandleHealthChange : IIncomingPacketHandler
{
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for animator packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_HEALTH_CHANGED;

    /// <summary>
    /// This functions executes the packet to update any heath bar. We send a bool value to see if its a player. If its not the end bytes change to a guid string to associate with an object and their health bar if present.
    /// </summary>
    /// <param name="buffer">Contains object id and trigger name</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        float percentageToSet = buffer.ReadFloat();
        bool localHealth = buffer.ReadBoolean();
        string serverId = null;
        if (!localHealth)
        {
            //then we must be getting packet containg buffer data with game model that has the healthbar
            int length = buffer.ReadInt();
            serverId = buffer.ReadString(length, Encoding.Default);
        }
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            
            HealthBar bar = null;
            GameObject hud = null;
            if (!localHealth)
            {
                hud = GameManager.Instance.ServerSpawns[Guid.Parse(serverId)];
            } else {
                hud = GameObject.FindGameObjectWithTag("LocalHealthBar");
            }

            if (hud != null)
            {
                bar = hud.GetComponent<HealthBar>();
            }
            if (bar != null)
            {
                bar.SetValue(percentageToSet);
            }
        });
    }
    

}
