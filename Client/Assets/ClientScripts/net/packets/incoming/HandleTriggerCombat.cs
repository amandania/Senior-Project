using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class HandleTriggerCombat : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        Guid index = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));

        int combatStage = buffer.ReadInt();

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject player = GameManager.Instance.ServerSpawns[index];
            Animator animator = player.GetComponent<Animator>();
            animator.SetInteger("CombatState", combatStage);
            animator.SetTrigger("TriggerAttack");
        });
    }


    public IncomingPackets PacketType => IncomingPackets.HANDLE_TRIGGER_COMABT;

}
