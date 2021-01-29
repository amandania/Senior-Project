using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Text;

public class HandlePlayerAnimation : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        Guid index = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));

        int oldanimlength = buffer.ReadInt();
        string lastanim = buffer.ReadString(oldanimlength, Encoding.Default);
        int currentAnimLength = buffer.ReadInt();
        string currentanim = buffer.ReadString(currentAnimLength, Encoding.Default);

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject player = GameManager.instance.playerList[index];
            Animator animator = player.GetComponent<Animator>();
            var runAnimPlaying = player.GetComponent<Animator>().GetBool("run");
            player.GetComponent<Animator>().SetBool(lastanim, false);
            player.GetComponent<Animator>().SetBool(currentanim, true);
        });
    }

    public void HandleAnimator(Animator animator, string anim, string lastAnim)
    {
        animator.SetBool(lastAnim, false);
        animator.SetBool(anim, true);   
    }


    public IncomingPackets PacketType => IncomingPackets.HANDLE_PLAYER_ANIMATION;

}
