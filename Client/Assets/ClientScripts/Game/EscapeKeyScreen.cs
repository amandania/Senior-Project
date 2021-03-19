using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeKeyScreen : MonoBehaviour
{
    public GameObject LogPanel;

    public float LastClick;
    // Start is called before the first frame update
    void Start()
    {
        LastClick = Time.time - 265;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool vis = !LogPanel.activeSelf;
            NetworkManager.instance.LocalPlayerGameObject.GetComponent<KeyListener>().mIsControlEnabled = !vis;
            LogPanel.SetActive(vis);
        }
    }

    public void HandleLogoutButton()
    {
        if (Time.time - LastClick < 2) {
            return;
        }
        LastClick = Time.time;
        NetworkManager.instance.SendPacket(new SendLogoutRequest().CreatePacket());
        print("Sent logout request");
        LogPanel.SetActive(false);
    }
}
