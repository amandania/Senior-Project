using Assets.ClientScripts.net.packets.outgoing;
using DotNetty.Transport.Channels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour
{

    private NetworkManager _net;

    public Text username;
    public InputField password;
    public Text responseMessage;
    private IDisposable _loginScreenSubscription;
    // Use this for initialization

    void Start()
    {
        this.username.text = "aki";
        this.password.text = "lol";
        _net = GameObject.Find("GameManager").GetComponent<NetworkManager>();
    }
    public static int ResponseCode = -1;

    public async void onLogin()
    {
        var connected = false;

        try
        {
            await _net.GetBuilder().InitClientTcp(_net.ipAddress, _net.port).ConfigureAwait(false);

            _net.SendPacket(new SendLoginRequest(username.text, password.text).CreatePacket());
            connected = true;
        }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
        catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                responseMessage.text = "Server is currently unavailable.";
            });
        }

        Debug.Log("Connected: " + connected);
        if (connected)
        {
            Debug.Log("running to check if response code changes..");
            var tokenSource2    = new CancellationTokenSource();
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
            }
            if (ResponseCode == 1)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    responseMessage.text = "Invalid Credentials.";
                });
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
