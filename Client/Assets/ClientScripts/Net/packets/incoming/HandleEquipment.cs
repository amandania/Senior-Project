using DotNetty.Buffers;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;
/// <summary>
/// This class is used to perform equipment actions to any gameobject spawned by a server. Equip actions are Equip and Unequip. We execute game object changes on the unity main thread to allow change.
/// </summary>
public class HandleEquipment : IIncomingPacketHandler
{

    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for equipment action packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_EQUIPMENT;



    /// <summary>
    /// This function is used to equip a specific weapon to any child name
    /// Server is sending us 4 data pieces which is the
    ///     1. Parent gameobject string id that is getting the equip action
    ///     2. The function name to invoke or equip action name
    ///     3. The weapon name to attempt to equip.
    ///     4. The name of the child gameobject getting the weapon transform. (hand or something else)
    /// </summary>
    /// <param name="buffer">Buffer message containing the data pieces needed</param>
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
            GameObject player = GameManager.Instance.ServerSpawns[index];
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
