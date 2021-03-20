using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// This class desroys a player gameobject during logout (if othere players discconect it will only destroy the game object here)
/// </summary>
public class HandleLogout : IIncomingPacketHandler
{
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for animator packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_LOGOUT;


    /// <summary>
    /// Read the packet containing client who logged out
    /// Destroy the player object our client
    /// </summary>
    /// <param name="buffer">Contains SessionId of user logging out</param>
    public void ExecutePacket(IByteBuffer buffer)
    {

        var plrIdLength = buffer.ReadInt();
        var playerguid = Guid.Parse(buffer.ReadString(plrIdLength, Encoding.Default));
        var returnPlayerToScreen = buffer.ReadBoolean();
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var gameobject = GameManager.Instance.PlayerList[playerguid];
            GameManager.Instance.DestroyServerObject(playerguid);
            GameManager.Instance.PlayerList.Remove(playerguid);

            if (returnPlayerToScreen)
            {
                Debug.Log("Logout the player to main screen");
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    SceneManager.LoadSceneAsync("LoginScreen", LoadSceneMode.Additive).completed += (t) =>
                    {
                        SceneManager.UnloadSceneAsync("MapScene").completed += (t2) =>
                        {
                            NetworkManager.networkStream.CloseAsync();
                            GameManager.Instance.ServerSpawns.Clear();
                            GameManager.Instance.GroundItems.Clear();
                            GameManager.Instance.PlayerList.Clear();
                            GameManager.Instance.NpcList.Clear();
                            GameObject.Destroy(Camera.main.gameObject.GetComponent<PlayerCamera>());
                        };
                    };
                });
                //We exited the game manually so we return back to login screen here
            }
        });

    }
    

}
