using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class SendEquipmentAction : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_EQUIPMENT_ACTION;

    //Person doing the equipment action
    private readonly Player m_player;
    
    //This string is used to let client know what method name we are trying to invoke. Such as Equip/Unequip
    private string m_callbackName;

    
    //Used to find/add the item model name to respective properties of m_players' client object
    private string m_itemName;
    private string m_parentTransformName;

    public SendEquipmentAction(Player a_player, string a_callbackName, string a_itemName, string a_parentTransoformName)
    {
        m_player = a_player;
        m_callbackName = a_callbackName;
        m_itemName = a_itemName;
        m_parentTransformName = a_parentTransoformName;
    }

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
