using DotNetty.Buffers;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;

public class HandleEquipment : IIncomingPacketHandler
{
    public IncomingPackets PacketType => IncomingPackets.HANDLE_EQUIPMENT;

    public void ExecutePacket(IByteBuffer buffer)
    {
        Debug.Log("equiipment packet");
        int guidLength = buffer.ReadInt();

        //object doing specfic action to weapon
        Guid index = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));

        int callbackLength = buffer.ReadInt();
        string methodName = buffer.ReadString(callbackLength, Encoding.Default);

        int weaponNameLength = buffer.ReadInt();
        string weaponName = buffer.ReadString(weaponNameLength, Encoding.Default);

        int transformNameLength = buffer.ReadInt();
        string transformParentName = buffer.ReadString(transformNameLength, Encoding.Default);

        Debug.Log("Incoming equipment action: " + methodName + " on item: " + weaponName);
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject player = GameManager.instance.ServerSpawns[index];
            Equipment myEquipments = player.GetComponent<Equipment>();
            if (myEquipments != null)
            {
                MethodInfo methodInfo = myEquipments.GetType().GetMethod(methodName);
                Debug.Log("Method name: " + methodName + " calls info: " + methodInfo);
                if (methodInfo != null)
                {
                    object result = null;
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    object classInstance = Activator.CreateInstance(GetType(), null);

                    //if this function takes no parmaters just execute it
                    if (parameters.Length == 0)
                    {
                        result = methodInfo.Invoke(classInstance, null);
                    }
                    else
                    {
                        //invoke method with paramater
                        result = methodInfo.Invoke(myEquipments, new object[] { weaponName, transformParentName });
                    }
                }
            }
        });
    }



}
