using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using Assets.ClientScripts.net.packets.outgoing;

public class HandleAnimatorTrigger : IIncomingPacketHandler
{
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

    public IncomingPackets PacketType => IncomingPackets.HANDLE_ANIMATOR_TRIGGER;

}
