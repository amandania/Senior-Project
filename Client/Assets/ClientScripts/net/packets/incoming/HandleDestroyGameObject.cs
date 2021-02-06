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
                var currentLook = GameManager.instance.camera.GetComponent<PlayerCamera>().lookPoint;
                if (currentLook.gameObject == obj)
                {
                    GameManager.instance.camera.GetComponent<PlayerCamera>().lookPoint = null;
                }

                GameObject.Destroy(obj);
                GameManager.instance.ServerSpawns.Remove(guid);
            }
        });
    }

    public IncomingPackets PacketType => IncomingPackets.HANDLE_LOGIN_RESPONSE;

}
