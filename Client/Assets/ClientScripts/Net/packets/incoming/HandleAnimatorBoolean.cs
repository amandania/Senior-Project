using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This packet is used to set any boolean paramater value by name for specific game object id s' Animation Controller
/// </summary>

public class HandleAnimatorBoolean : IIncomingPacketHandler
{
    /// <summary>
    /// This functions reads our game object Id as a Guid, our boolean paramater name, and the sate to set it too.
    /// </summary>
    /// <param name="buffer">Contains object id and trigger name</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        Guid index = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));

        int triggerLength = buffer.ReadInt();
        string triggerName = buffer.ReadString(triggerLength, Encoding.Default);
        bool triggerStae = buffer.ReadBoolean();
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject player = GameManager.instance.ServerSpawns[index];
            Animator animator = player.GetComponent<Animator>();
            animator.SetBool(triggerName, triggerStae);
        });

    }
    /// <summary>
    /// Packet Id used to refrence incoming packet handling
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Packet Id</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_ANIMATION_BOOL;

}
