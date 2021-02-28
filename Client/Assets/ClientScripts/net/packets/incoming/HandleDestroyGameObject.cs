using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class HandleDestroyGameObject : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {
        var length = buffer.ReadInt();
        var guid = Guid.Parse(buffer.ReadString(length, Encoding.Default));

        //server should only authoartate this send to destroy monsters and objects not players.
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var obj = GameManager.instance.ServerSpawns[guid];
            if (obj != null)
            {
                //if we are looking at whats about to be destroy get rid of it as a look.
                var currentLook = Camera.main.GetComponent<PlayerCamera>().lookPoint;
                if (currentLook != null & currentLook == obj)
                {
                    GameManager.instance.camera.GetComponent<PlayerCamera>().lookPoint = null;
                }
                GameManager.instance.DestroyServerObject(guid);
            }
        });
    }

    public IncomingPackets PacketType => IncomingPackets.HANDLE_DESTROY_OBJECT;

}
