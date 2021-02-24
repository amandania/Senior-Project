using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hotkeys : Container
{
    private const string CONTAINER_NAME = "Hotkeys";

    private readonly Player m_player;

    public Hotkeys(Player a_player, int a_size)
    {
        DeleteOnRefresh = false;
        m_player = a_player;

        ContainerItems = new List<SlotItem>();
        for (int i = 0; i < a_size; i++)
        {
            ContainerItems.Add(new SlotItem());
        }
    }


    /// <summary>
    /// This function is used to handle a players slot click.
    /// </summary>
    /// <param name="slot"></param>
    public void HandlItemClick(int slot)
    {
        
    }

    public void RefrehsItems()
    {
        m_player.Session.SendPacket(new SendRefreshContainer(CONTAINER_NAME, DeleteOnRefresh, ContainerItems));
        Debug.Log(m_player.UserName + " is refreshing : " + CONTAINER_NAME);
    }

    
}
