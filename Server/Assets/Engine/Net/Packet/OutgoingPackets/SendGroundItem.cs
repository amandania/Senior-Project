using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This class is responsible for sending a ground object that is being spawned on server to all clients. Or a specifc client, currently we just have any ground object being spawned sent to everyone on the client and everyone that connects afterward also calls this packet.
/// </summary>
public class SendGroundItem : IOutGoingPackets
{

    /// <summary>
    /// Packet identifer for client to execute handle function for this header.
    /// </summary>
    public OutGoingPackets PacketType => OutGoingPackets.SEND_GROUND_ITEM;

    private readonly ItemBase m_item;
    

    public SendGroundItem(ItemBase a_item)
    {
        m_item = a_item;

    }

    /// <summary>
    /// This function will create a buffer message containg the server id for ground item and its position values.
    /// Client uses the infromation to spawn the visual representation
    /// </summary>
    /// <returns>Ground item buffer message</returns>
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
