using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// This class is added to any Local Game object <see cref="GameManager.SpawnPlayer(string, System.Guid, UnityEngine.Vector3, UnityEngine.Quaternion, bool)"/>
/// We listen for movement keys here and send our expected move vector to the server. Thats it, we have no local movement currently because we let our server handle how we move and where we move to.
/// We also take into account camera look vector and apply that to the movement vector so our server transforms are relative to our look move vector. 
/// </summary>

public class KeyListener : MonoBehaviour
{

    #region Private Members

    //Control local aniamtions before server just to perform some instant feel to animation change.
    private Animator _animator;


    //We need to come back to this and apply local movement so our client prediction on the packet is smoother
    private CharacterController _characterController;

    #endregion

    #region Public Members

    //Use this variables to apply local movement before sending the movement to server
    public float Speed = 5.0f;
    public float RotationSpeed = 240.0f;

    public MouseInputUIBlocker m_uiBlocker;

    //Local classes that should exist on startup for this local gameobject
    public PlayerCamera PlayerCam { get; set; }
    public ChatManager ChatManager;
    #endregion

    //Allow movement input to be sent
    public bool mIsControlEnabled = true;

    // Use this for initialization
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        m_uiBlocker = GetComponent<MouseInputUIBlocker>();
        ChatManager = GameObject.Find("HUD").transform.Find("Chat").GetComponent<ChatManager>();
    }
    
    //TODO variables for local player etwork lerping
    public bool canSendNetworkMovement;
    public float timeBetweenMovementStart;
    public float timeBetweenMovementEnd;
    public Vector3 lastMove;
    private Vector3 MaxVector = new Vector3(1, 0, 1);
    public float networkSendRate = 5;
    

    /// <summary>
    /// This function listens for all input keys inlcuding special input keys such as hotkeys, and escape along with interaction keyboard input "F"
    /// </summary>
    private void Update()
    {
        if (!mIsControlEnabled)
        {
            return;
        }
        var mouseButton1Down = Input.GetMouseButtonDown(0);
        if (EventSystem.current && mouseButton1Down && !EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("mouse clicked");
            NetworkManager.instance.SendPacket(new SendMouseLeftClick().CreatePacket());
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            NetworkManager.instance.SendPacket(new SendActionKeys(KeyInput.HOTKEY1).CreatePacket());
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            NetworkManager.instance.SendPacket(new SendActionKeys(KeyInput.HOTKEY2).CreatePacket());
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            NetworkManager.instance.SendPacket(new SendActionKeys(KeyInput.HOTKEY3).CreatePacket());
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            NetworkManager.instance.SendPacket(new SendActionKeys(KeyInput.HOTKEY4).CreatePacket());
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            NetworkManager.instance.SendPacket(new SendActionKeys(KeyInput.HOTKEY5).CreatePacket());
        }
        else if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            NetworkManager.instance.SendPacket(new SendActionKeys(KeyInput.HOTKEY6).CreatePacket());
        }
        else if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            NetworkManager.instance.SendPacket(new SendActionKeys(KeyInput.HOTKEY7).CreatePacket());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            NetworkManager.instance.SendPacket(new SendActionKeys(KeyInput.HOTKEY8).CreatePacket());
        } else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            NetworkManager.instance.SendPacket(new SendActionKeys(KeyInput.HOTKEY9).CreatePacket());
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            //print("Input F");
            NetworkManager.instance.SendPacket(new SendActionKeys(KeyInput.F).CreatePacket());
        }

    }

    private Vector3 m_zeroVector = Vector3.zero;

    // Update is called once per frame data
    /// <summary>
    /// Update is called once per frame data and is used to send movement vector to server
    /// </summary>
    private void FixedUpdate()
    {
        if (mIsControlEnabled && Camera.main != null)
        {

            // Get Input for axis
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // Calculate the forward vector
            Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, MaxVector).normalized;
            Vector3 move = v * camForward_Dir + h * Camera.main.transform.right;

            if (ChatManager.ChatActive)
            {
                move = m_zeroVector;
            }
            ////Debug.Log(move.magnitude / (isSprinting ? 1 : 2) );
            Vector3 relativeInput = transform.InverseTransformDirection(move);


            ////Debug.Log("Horizontal :" + relativeInput.x + ", Vertical:" + relativeInput.z);
            _animator.SetBool("IsStrafing", PlayerCam.lockMovementToCam);
            _animator.SetFloat("HorizontalInput", relativeInput.x);
            _animator.SetFloat("VerticalInput", relativeInput.z);
            _animator.SetFloat("Speed", move.magnitude);

            if (NetworkManager.networkStream.IsWritable)
            {
                ////Debug.Log("disabled movement send");
                lastMove = move;
                NetworkManager.instance.SendPacket(new SendMovementPacket(move, PlayerCam.lockMovementToCam, Camera.main.transform.eulerAngles.y).CreatePacket());
            }
        }
    }


}
