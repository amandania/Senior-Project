using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

/// <summary>
/// This class is used to send any kind of equipment action to a client. This includes Equipping and Unequipping. We use this class to send details on the equip aswell, like what parent child should this weapon equip to and the weapon name so client can load resources if it needs to.
/// WE also send the main server id of the object getting equip because client should know which id is getting the equip.
/// </summary>
public class SendEquipmentAction : IOutGoingPackets
{

    /// <summary>
    /// Equipment action packet identifer for clients.
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_EQUIPMENT_ACTION;

    //Person doing the equipment action
    private readonly Character m_player;
    
    //This string is used to let client know what method name we are trying to invoke. Such as Equip/Unequip
    private string m_callbackName;

    
    //Used to find/add the item model name to respective properties of m_players' client object
    private string m_itemName;
    private string m_parentTransformName;

    /// <summary>
    /// Main constructor used to create our equip action message fields
    /// </summary>
    /// <param name="a_player">The character getting the equip</param>
    /// <param name="a_callbackName">The action method name to invoke on client Equip/UnEquip</param>
    /// <param name="a_itemName">The item name to perform action on</param>
    /// <param name="a_parentTransoformName">The parent child that gets the equip action item as a child</param>
    public SendEquipmentAction(Character a_player, string a_callbackName, string a_itemName, string a_parentTransoformName)
    {
        m_player = a_player;
        m_callbackName = a_callbackName;
        m_itemName = a_itemName;
        m_parentTransformName = a_parentTransoformName;
    }

    /// <summary>
    /// This function creates our equip action message. The buffer contains
    /// the serverid of game object equipping
    /// the callback name or method name to invoke on client
    /// the item name
    /// and finally the parent trasform name getting the equip parent too.
    /// </summary>
    /// <returns>Buffer message for client to read</returns>
    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        String guid = m_player.GetGuid().ToString();
        int length = guid.Length;

        buffer.WriteInt(length);
        buffer.WriteString(guid, Encoding.Default);

        buffer.WriteInt(m_callbackName.Length);
        buffer.WriteString(m_callbackName, Encoding.Default);

        buffer.WriteInt(m_itemName.Length);
        buffer.WriteString(m_itemName, Encoding.Default);
        buffer.WriteInt(m_parentTransformName.Length);
        buffer.WriteString(m_parentTransformName, Encoding.Default);


        //Debug.Log("Spawning other player with Session Id: {0}" + _player._Session.PlayerId);

        return buffer;
    }
}
