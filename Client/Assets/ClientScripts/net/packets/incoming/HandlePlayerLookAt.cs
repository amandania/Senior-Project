using DotNetty.Buffers;
using System;
using System.Text;
/// <summary>
/// This class is used to trigger any look camera actions by the server. When we set this our camera will and movement information will be relative to the target we are looking at.
/// It will also force you into a strafe mode.
/// </summary>
public class HandlePlayerLookAt : IIncomingPacketHandler
{
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for Player Look AT packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_PLAYER_LOOKAT;

    /// <summary>
    /// This function is used to read the valid server id for the gameobject our local client is trying to look at.
    /// </summary>
    /// <param name="buffer">Buffer message containg game object id.</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        Guid index = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var lookAt = GameManager.Instance.ServerSpawns[index];
            if (lookAt != null)
            {
                GameManager.Instance.camera.GetComponent<PlayerCamera>().lookPoint = GameManager.Instance.ServerSpawns[index].transform;
            }
        });
    }
    
}
