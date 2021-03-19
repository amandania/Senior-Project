using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyListener : MonoBehaviour
{

    #region Private Members

    private Animator _animator;

    private CharacterController _characterController;

#pragma warning disable CS0414 // The field 'KeyListener.Gravity' is assigned but its value is never used
    private readonly float Gravity = 20.0f;
#pragma warning restore CS0414 // The field 'KeyListener.Gravity' is assigned but its value is never used

    private Vector3 _moveDirection = Vector3.zero;
    private readonly int startHealth;

    private readonly int startFood;

    #endregion

    #region Public Members

    public float Speed = 5.0f;

    public float RotationSpeed = 240.0f;

    public GameObject Hand;

    public float JumpSpeed = 7.0f;

    public bool isSprinting = false;

    public MouseInputUIBlocker m_uiBlocker;

    public PlayerCamera PlayerCam { get; set; }

    public ChatManager ChatManager;
    #endregion


    // Use this for initialization
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        m_uiBlocker = GetComponent<MouseInputUIBlocker>();
        ChatManager = GameObject.Find("HUD").transform.Find("Chat").GetComponent<ChatManager>();
    }


    public bool mIsControlEnabled = true;

    public void EnableControl()
    {
        mIsControlEnabled = true;
    }

    public void DisableControl()
    {
        mIsControlEnabled = false;
    }

    public bool canSendNetworkMovement;
    public float timeBetweenMovementStart;
    public float timeBetweenMovementEnd;
    public Vector3 lastMove;
    private Vector3 MaxVector = new Vector3(1, 0, 1);
    public float networkSendRate = 5;

    private List<int> keys { get; set; } = new List<int>();



    private void Update()
    {
        if (!mIsControlEnabled)
        {
            return;
        }
        var mouseButton1Down = Input.GetMouseButtonDown(0);
        if (mouseButton1Down && !EventSystem.current.IsPointerOverGameObject())
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
