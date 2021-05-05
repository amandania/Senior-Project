using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used to handle the login screen portion of the game. Its important to realize when we are on this screen, we also dont have a valid network available.
/// We only create the socket connection after we hit the login button and create a temporary state for the response code recieved by server to be handled appropiaely. <see cref="HandleLoginResponse"/>
/// </summary>

public class LoginScreen : MonoBehaviour
{
    //Our netwrok instance
    private NetworkManager _net;

    //log input details
    public Text username;
    public InputField password;
    public Text responseMessage;
    
    // Use this for initialization

    private void Start()
    {
        _net = GameObject.Find("GameManager").GetComponent<NetworkManager>();
    }
    public static int ResponseCode = -1;

    /// <summary>
    /// This function is triggered by out onclick event for the login button. We attempt to connect to the server, and try to log in
    /// When we get the packet be we either display the response code message or log in.
    /// <see cref="HandleLoginResponse.ExecutePacket"/>
    /// </summary>
    public async void OnLogin()
    {
        var connected = false;

        try
        {
            await _net.GetBuilder().InitClientTcp(_net.ipAddress, _net.port).ConfigureAwait(false);

            _net.SendPacket(new SendLoginRequest(username.text, password.text).CreatePacket());
            connected = true;
        }
        catch (Exception)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                responseMessage.text = "Server is currently unavailable.";
            });
        }

        //Debug.Log("Connected: " + connected);
        if (connected)
        {
            //Debug.Log("running to check if response code changes..");
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;
            var tick = 0;
            await Task.Factory.StartNew(() =>
            {

                while (ResponseCode == -1)
                {
                    tick++;
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        responseMessage.text = "Attemping to connect..";
                    });
                }
            }, tokenSource2.Token);

            if (ResponseCode == 0)
            {
                tick = 0;
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    responseMessage.text = "Logging in...";
                });
            }else if (ResponseCode == 1)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    responseMessage.text = "Invalid Credentials.";
                });
            }else if (ResponseCode == 1)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    responseMessage.text = "Invalid Username";
                });
            }
        }

    }

    // Update is called once per frame
    private void Update()
    {

    }
}
