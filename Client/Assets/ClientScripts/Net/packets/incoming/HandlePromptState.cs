using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
/// <summary>
/// We use class to handle all our container refreshes. Containers contain Slots. They are used for Shop items, inventory items and hotkey items currently.
/// </summary>
public class HandlePromptState : IIncomingPacketHandler
{

    /// <summary>
    /// 
    ///
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_CONTAINER_REFRESH;


    /// <summary>
    /// Read the packet containing container inforamtion such the name and items in it.
    /// </summary>
    /// <param name="buffer">Contains container id (name, and length of possible items) and all inventory space data. Including empty. </param>
    public void ExecutePacket(IByteBuffer buffer)
    {

        var containerNameLength = buffer.ReadInt();
        var containerName = buffer.ReadString(containerNameLength, Encoding.Default);

        var containerSpace = buffer.ReadInt();

        var deleteEmpty = buffer.ReadBoolean();
        for (int i = 0; i < containerSpace; i++)
        {
            int itemNameLength = buffer.ReadInt();
            string itemName = buffer.ReadString(itemNameLength, Encoding.Default);
            int itemAmount = buffer.ReadInt();
            var slotindex = i;
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                var taggedContainer = GameObject.FindWithTag(containerName);
                if (taggedContainer != null)
                {
                    var container = taggedContainer.GetComponent<Container>();
                    container.RefreshSlot(slotindex, itemName, itemAmount, deleteEmpty);
                }
            });
        }
        

    }

}