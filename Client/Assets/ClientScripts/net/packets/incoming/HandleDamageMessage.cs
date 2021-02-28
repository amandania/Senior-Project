using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class HandleDamageMessage : IIncomingPacketHandler
{

    public void ExecutePacket(IByteBuffer buffer)
    {
        Guid guid;
        int guidLength = buffer.ReadInt();
        Guid.TryParse(buffer.ReadString(guidLength, Encoding.Default), out guid);

        int damageAmount = buffer.ReadInt();

        float lifeTime = buffer.ReadFloat();
        UnityEngine.Debug.Log("DAMAGE IS BEING POPPED");
        if (guid != null)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                GameObject characterObject = null;
                var hasVal = GameManager.instance.ServerSpawns.TryGetValue(guid, out characterObject);
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


    public IncomingPackets PacketType => IncomingPackets.HANDLE_DAMAGE_MESSAGE;

}
