using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This class is used to destroy any server game object including other players (not local)
/// </summary>
public class HandleDestroyGameObject : IIncomingPacketHandler
{
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for destry gameobject packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_DESTROY_OBJECT;


    /// <summary>
    /// This function is executed and reads the buffer containg game object id to destroy and its type (monster/other)
    /// </summary>
    /// <param name="buffer">Buffer Message</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        var length = buffer.ReadInt();
        var guid = Guid.Parse(buffer.ReadString(length, Encoding.Default));
        var isMonster = buffer.ReadBoolean();
        //server should only authoartate this send to destroy monsters and objects not players.
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var obj = GameManager.Instance.ServerSpawns[guid];
            if (obj != null)
            {
                //if we are looking at whats about to be destroy get rid of it as a look.
                var currentLook = Camera.main.GetComponent<PlayerCamera>().lookPoint;
                if (currentLook != null & currentLook == obj)
                {
                    GameManager.Instance.camera.GetComponent<PlayerCamera>().lookPoint = null;
                }
                //Debug.Log("Game object destroyed on client.");
                GameManager.Instance.DestroyServerObject(guid, isMonster);
            }
        });
    }

   

}
