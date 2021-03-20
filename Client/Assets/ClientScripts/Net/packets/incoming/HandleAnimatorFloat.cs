using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This packet is used to set any float paramater value by name for specific game object id s' Animation Controller
/// </summary>

public class HandleAnimatorFloat : IIncomingPacketHandler
{
    /// <summary>
    /// This functions reads our game object Id as a Guid, our boolean paramater name, and the float value to set it too.
    /// </summary>
    /// <param name="buffer">Contains object id and trigger name</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        Guid index = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));

        int floatLength = buffer.ReadInt();
        string floatName = buffer.ReadString(floatLength, Encoding.Default);
        float value = buffer.ReadFloat();
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject player = GameManager.Instance.ServerSpawns[index];
            Animator animator = player.GetComponent<Animator>();
            Debug.Log("Setting animator float: " + floatName);
            if (GameManager.Instance.AnimatorHasParamter(animator, floatName)) {
                Debug.Log("found animator float: " + floatName);
                animator.SetFloat(floatName, value);
            }
        });

    }

    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for animator packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_FLOAT_ANIMATOR;

}
