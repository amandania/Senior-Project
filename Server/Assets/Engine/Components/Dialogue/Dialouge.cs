using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
/// <summary>
/// This class is used to represent a Dialogue. A dilogue conists of possible messages boards or a sarray of strings. Dialogues have a correspendion array of strings to represent options avaialble for message index
/// Dialogues have a dialogue id and a dialouge title as some identifier variabls. <see cref="DialogueSystem"/> for implmentations.
/// </summary>
public class Dialogue
{
    private int m_dialougeId;
    private string m_dialougeTitle;
    private string[] m_dialougeMessages;

    //[n][n] gives u the dialouge message index and its coresspendoing responses
    private Dictionary<int, string[]> optionLists;

    public int OptionClicked = -1;
    

    public Dialogue(int a_dialougeId, string a_dialogueTitle, string[] a_messages, Dictionary<int, string[]> a_responses)
    {
        m_dialougeId = a_dialougeId;
        m_dialougeTitle = a_dialogueTitle;
        m_dialougeMessages = a_messages;
        optionLists = a_responses;
    }

    /// <summary>
    /// This function will continue an existing dialogue to its next prompt if it has one.
    /// </summary>
    /// <param name="a_player">Player continuing dialogue</param>
    /// <returns>Task to send to a player. We dont wait for it but we just treat this as a task so the network thread can be execute by a unity main thread or server thread.</returns>
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
    
    /// <summary>
    /// Getter function to get dialouge id
    /// </summary>
    /// <returns> integer dialogue id</returns>
    public int GetDialougeId()
    {
        return m_dialougeId;
    }
    /// <summary>
    /// Getter function to get dialouge title
    /// </summary>
    /// <returns> string dialogue title</returns>
    public string GetDialogueTitle()
    {
        return m_dialougeTitle;
    }
   
    /// <summary>
    /// Get all the messages in this dialogue
    /// </summary>
    /// <returns>Array of strings</returns>
    public string[] GetDialogueMessages()
    {
        return m_dialougeMessages;
    }
}
