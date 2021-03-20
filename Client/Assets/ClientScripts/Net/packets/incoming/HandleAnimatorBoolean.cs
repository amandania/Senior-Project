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
    /// This functions reads our game object Id as a string and we convert it into a Guid. We read the boolean paramater name for the animator to handle, and the sate to set it too. Finally we execute the changes on the main thread to the actual game object
    /// </summary>
    /// <param name="buffer">Contains animators parent model game object id and the state data to set</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        Guid index = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));

        int triggerLength = buffer.ReadInt();
        string triggerName = buffer.ReadString(triggerLength, Encoding.Default);
        bool triggerStae = buffer.ReadBoolean();
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject player = GameManager.Instance.ServerSpawns[index];
            Animator animator = player.GetComponent<Animator>();
            animator.SetBool(triggerName, triggerStae);
        });
    }

    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for animator boolean packet change</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_ANIMATION_BOOL;

}
