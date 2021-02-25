using DotNetty.Buffers;
using System;
using System.Text;

public class HandlePlayerLookAt : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        Guid index = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var lookAt = GameManager.instance.ServerSpawns[index];
            if (lookAt != null)
            {
                GameManager.instance.camera.GetComponent<PlayerCamera>().lookPoint = GameManager.instance.ServerSpawns[index].transform;
            }
        });
    }


    public IncomingPackets PacketType => IncomingPackets.HANDLE_PLAYER_LOOKAT;

}
