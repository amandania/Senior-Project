using DotNetty.Buffers;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
/// <summary>
/// Anytime we want to refresh a container we are going to send this packet. A refresh consist of an array of SlotItems.
/// We are going to send the number of items and constructor the bytes array for what slot items exist in our contaier.
/// We only use this class for Hotkey Items. <see cref="Hotkeys.RefrehsItems()"/>
/// </summary>
public class SendRefreshContainer : IOutGoingPackets
{
    /// <summary>
    /// Container Frefresh Packet Header ID
    /// </summary>
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

    /// <summary>
    /// This function will create the buffer message contain all our container items to send.
    /// Client will handle the visual draws based on if we find the item resources models.
    /// </summary>
    /// <returns>Buffer message to send container refrehs</returns>
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
            buffer.WriteBoolean(slot.IsActive);
        }

        return buffer;
    }
}
