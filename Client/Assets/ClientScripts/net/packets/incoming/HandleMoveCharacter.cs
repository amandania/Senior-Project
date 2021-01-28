using UnityEngine;
using DotNetty.Buffers;
using System;
using System.Text;

public class HandleMoveCharacter : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        Guid index = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));

        
        float oldX = buffer.ReadFloat();
        float oldY = buffer.ReadFloat();
        float oldz = buffer.ReadFloat();
        Quaternion oldRotation = Quaternion.Euler(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());

        float x = buffer.ReadFloat();
        float y = buffer.ReadFloat();
        float z = buffer.ReadFloat();
        Quaternion newRotation = Quaternion.Euler(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());


        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject player = GameManager.instance.playerList[index];
            Lerp(player, new Vector3(oldX, oldY, oldz), new Vector3(x, y, z), oldRotation, newRotation);
        });
    }
    void Lerp(GameObject player, Vector3 lastRealPosition, Vector3 realPosition, Quaternion lastRealrotation, Quaternion realrotation)
    {   
        float timeStartedLerping = Time.time;
        float timeToLerp = 20f;

        float lerpPercentage = (Time.time - timeStartedLerping / timeToLerp);
        player.transform.position = Vector3.Lerp(player.transform.position, realPosition, lerpPercentage);

        lerpPercentage = (Time.time - timeStartedLerping / timeToLerp);
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, realrotation, lerpPercentage);

    }
    public IncomingPackets PacketType => IncomingPackets.HANDLE_MOVE_CHARACTER;

}
