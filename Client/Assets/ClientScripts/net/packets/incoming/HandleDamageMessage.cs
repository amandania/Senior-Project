using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This class is used to start the life of a damage text prab. A damage text prfab has the DamageLife mono behavior class. <see cref="DamageLife"/>
/// </summary>
public class HandleDamageMessage : IIncomingPacketHandler
{

    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for damage message packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_DAMAGE_MESSAGE;

    /// <summary>
    /// This function is executed when we recieve this packet. The buffer has the id for the game object getting the damage life and the damage amount to display
    /// </summary>
    /// <param name="buffer">message buffer used to assign damage life class to the game.</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        Guid guid;
        int guidLength = buffer.ReadInt();
        Guid.TryParse(buffer.ReadString(guidLength, Encoding.Default), out guid);

        int damageAmount = buffer.ReadInt();

        float lifeTime = buffer.ReadFloat();
        //UnityEngine.Debug.Log("DAMAGE IS BEING POPPED");
        if (guid != null)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                GameObject characterObject = null;
                var hasVal = GameManager.Instance.ServerSpawns.TryGetValue(guid, out characterObject);
                if (characterObject != null)
                {
                    var damagePrefab = Resources.Load("Prefabs/Damage") as GameObject;
                    var gameObj = GameObject.Instantiate(damagePrefab, characterObject.transform.position + damagePrefab.transform.position, Quaternion.identity, characterObject.transform);
                    var comp = gameObj.GetComponent<DamageLife>();
                    
                    if (NetworkManager.instance.myIndex == guid)
                    {
                        comp.GetComponent<TextMesh>().color = Color.yellow;
                    }
                    if (damageAmount <= 0)
                    {
                        comp.GetComponent<TextMesh>().color = Color.cyan;
                    }
                    
                    comp.StartDamage(damageAmount.ToString(), lifeTime);
                }
            });
        }
    }

}
