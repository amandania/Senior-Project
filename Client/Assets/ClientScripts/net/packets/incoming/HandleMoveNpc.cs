using UnityEngine;
using System.Collections;
using DotNetty.Buffers;
using System;
using System.Text;
using System.Collections.Generic;

public class HandleMoveNpc : IIncomingPacketHandler
{
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        string guidString = buffer.ReadString(guidLength, Encoding.Default);

        float lastXStep = buffer.ReadFloat();
        float lastYStep = buffer.ReadFloat();
        float lastZStep = buffer.ReadFloat();

        float XStep = buffer.ReadFloat();
        float YStep = buffer.ReadFloat();
        float ZStep = buffer.ReadFloat();


        float lookX = buffer.ReadFloat();
        float lookY = buffer.ReadFloat();
        float lookZ = buffer.ReadFloat();
        float turnSpeed = buffer.ReadFloat();

        Guid guid = Guid.Parse(guidString);


        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject npcToMove = GameManager.instance.npcList[guid];

            Vector3 OldPosition = new Vector3(lastXStep, npcToMove.transform.position.y, lastZStep);
            
            Vector3 RealPosition = new Vector3(XStep, npcToMove.transform.position.y, ZStep);

            Vector3 LookPoint = new Vector3(lookX, npcToMove.transform.position.y, lookZ);

            //handle Lerp

            float timeStartedToLerp = Time.time;
            float timeToLerp = 20f;

            float lerpPercentage = (Time.time - timeStartedToLerp / timeToLerp);
            npcToMove.transform.position = Vector3.Lerp(OldPosition, RealPosition, lerpPercentage);

            lerpPercentage = (Time.time - timeStartedToLerp / timeToLerp);
            //npcToMove.transform.rotation = Quaternion.Lerp(OldRotation, RealRotation, lerpPercentage);
            Quaternion targetRotation = Quaternion.LookRotation(LookPoint - RealPosition);
            npcToMove.transform.rotation = Quaternion.Lerp(npcToMove.transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

        });

    }

    public IncomingPackets PacketType => IncomingPackets.HANDLE_MOVE_NPC;

}
