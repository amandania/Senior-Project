using DotNetty.Buffers;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SendRefreshContainer : IOutGoingPackets
{
    public OutGoingPackets PacketType => OutGoingPackets.SEND_REFRESH_CONTAINER;

    private readonly string m_containerName;
    private readonly List<SlotItem> m_slotItems;
    private readonly bool m_deleteOnEmpty = false;

    public SendRefreshContainer(string a_container, bool deleteOnEmpty, List<SlotItem> a_items)
    {
        m_containerName = a_container;
        m_slotItems = a_items;
        m_deleteOnEmpty = deleteOnEmpty;
    }

    public IByteBuffer GetPacket()
    {
        var buffer = Unpooled.Buffer();
        buffer.WriteInt(m_containerName.Length);
        buffer.WriteString(m_containerName, Encoding.Default);

        buffer.WriteInt(m_slotItems.Count);
        buffer.WriteBoolean(m_deleteOnEmpty);
        foreach (SlotItem slot in m_slotItems)
        {
            buffer.WriteInt(slot.ItemName.Length);
            buffer.WriteString(slot.ItemName, Encoding.Default);
            buffer.WriteInt(slot.Amount);
        }

        return buffer;
    }
}
