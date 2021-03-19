using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Dialogue
{
    private int m_dialougeId;
    private string m_dialougeTitle;
    private string[] m_dialougeMessages;

    //[n][n] gives u the dialouge message index and its coresspendoing responses
    private Dictionary<int, string[]> optionLists;

    public int OptionClicked = -1;
    

    public Dialogue(int a_dialougeId, string a_dialogueTitle, string[] messages, Dictionary<int, string[]> responses)
    {
        m_dialougeId = a_dialougeId;
        m_dialougeTitle = a_dialogueTitle;
        m_dialougeMessages = messages;
        optionLists = responses;
    }

    public async Task ContinueDialouge(Player a_player)
    {
        if (m_dialougeMessages.Length < a_player.DialougeMessageIndex)
        {
            //clse dialouge
            a_player.DialougeMessageIndex = 0;
            Debug.Log("Close dialogue reachd end message");
            return;
        }
        var message = m_dialougeMessages[a_player.DialougeMessageIndex];
        string[] options = optionLists[a_player.DialougeMessageIndex];

        Debug.Log("dialouge to send: " + message);

        Debug.Log("Number of options: " + options.Length);
        for (int i = 0; i < options.Length;i++)
        {
            Debug.Log("Send option: " + options[i]);
        }
        a_player.DialougeMessageIndex += 1;
        await a_player.Session.SendPacket(new SendDialogue(message, options)).ConfigureAwait(false);
    }
    
    public int GetDialougeId()
    {
        return m_dialougeId;
    }

    public string GetDialogueTitle()
    {
        return m_dialougeTitle;
    }
   
    public string[] GetDialogueMessages()
    {
        return m_dialougeMessages;
    }
}
