using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This packet is used to activate any trigger by name to any gameobject
/// </summary>

public class HandleAnimatorTrigger : IIncomingPacketHandler
{
    /// <summary>
    /// Execute the trigger of the session id triggering the animation
    /// </summary>
    /// <param name="buffer">Contains object id and trigger name</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        Guid index = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));

        int triggerLength = buffer.ReadInt();
        string triggerName = buffer.ReadString(triggerLength, Encoding.Default);

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject player = GameManager.instance.ServerSpawns[index];
            Animator animator = player.GetComponent<Animator>();
            animator.SetTrigger(triggerName);
            Debug.Log("Trigge animator " + triggerName);
        });

    }
    /// <summary>
    /// Aniamtion Trigger Packet Id used to refrence incoming packet handling
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Packet Id</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_ANIMATOR_TRIGGER;

}
