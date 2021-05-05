using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This class is used to control a game objects animator state for combat.
/// We only use this as a toggle packet meaning its only execute to enable the triggers not disable because the animators treat "Triggers" like this.
/// </summary>
public class HandleTriggerCombat : IIncomingPacketHandler
{ 
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for Trigger Combat packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_TRIGGER_COMABT;
    
    /// <summary>
    /// This function reads the game object id finds a animator if we have one and set the combat states. All animators are assumed to have the named paramters.
    /// </summary>
    /// <param name="buffer">Buffer message containg combat triggger info</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        Guid index = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));

        int combatStage = buffer.ReadInt();

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject player = GameManager.Instance.ServerSpawns[index];
            if (player != null)
            {
                Animator animator = player.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetInteger("CombatState", combatStage);
                    animator.SetTrigger("TriggerAttack");
                }
            }
        });
    }

   

}
