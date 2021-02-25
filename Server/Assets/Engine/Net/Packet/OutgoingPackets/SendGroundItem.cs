using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class SendGroundItem : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_GROUND_ITEM;

    private readonly ItemBase m_item;
    

    public SendGroundItem(ItemBase a_item)
    {
        m_item = a_item;

    }

    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        String guid = m_item.InstanceGuid.ToString();
        int length = guid.Length;

        buffer.WriteInt(length);
        buffer.WriteString(guid, Encoding.Default);


        string resourceName = m_item.ItemName;
        buffer.WriteInt(resourceName.Length);
        buffer.WriteString(resourceName, Encoding.Default);


        Vector3 plrPos = m_item.transform.position;
        buffer.WriteFloat(plrPos.x);
        buffer.WriteFloat(plrPos.y);
        buffer.WriteFloat(plrPos.z);

        Vector3 rotation = m_item.transform.rotation.eulerAngles;
        buffer.WriteFloat(rotation.x);
        buffer.WriteFloat(rotation.y);
        buffer.WriteFloat(rotation.z);

        

        return buffer;
    }
}
