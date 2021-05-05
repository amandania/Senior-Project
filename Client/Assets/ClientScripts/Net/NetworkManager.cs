using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    [SerializeField] public string ipAddress;
    [SerializeField] public int port;
    [SerializeField] public Guid myIndex;


    public static NetworkManager instance;
    public GameObject LocalPlayerGameObject;

    private ChannelBuilder m_channelBuilder;
    public static IChannel networkStream;
    public static ConcurrentQueue<Guid> PlayersToInsantiate = new ConcurrentQueue<Guid>();

    public Terrain terrain;
    // Use this for initialization

    void Start()
    {

        DontDestroyOnLoad(this);
        SetBuilder(new ChannelBuilder());
        instance = this;
    }

    public void SendPacket(IByteBuffer a_buffer)
    {
        networkStream.WriteAndFlushAsync(a_buffer).ConfigureAwait(false);
    }

    private void OnApplicationQuit()
    {
        //Debug.Log("Application quit.");
        if (networkStream != null)
        {
            networkStream.CloseAsync();
        }
    }

    public void SetBuilder(ChannelBuilder a_builder)
    {
        m_channelBuilder = a_builder;
    }

    public ChannelBuilder GetBuilder()
    {
        return m_channelBuilder;
    }
    

}
