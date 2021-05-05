using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;
/// <summary>
/// This class is used to handle all character object movements performed on the server. We have to interpolate between its current position and its recieving positions to account for some delay. 
/// This would look smoother with some local movement which i dont have yet but i may consider adding.
/// </summary>
public class HandleMoveCharacter : IIncomingPacketHandler
{
    /// <summary>
    /// Packet Id used to refrence this class when an incoming packet type is recieved by server.
    /// <see cref="ChannelEventHandler.HandleDataPackets"/>
    /// <return>Enum ordinal for Move Character packet</return>
    /// </summary>
    public IncomingPackets PacketType => IncomingPackets.HANDLE_MOVE_CHARACTER;

    /// <summary>
    /// This function is used to move our game objects. We also set our animator value for strafing so we apply all the appropiate movement looks
    /// We recieve a buffer contain the object id moving, the speed its moving, horizontal and vertical input based on servers movevector the strafe paramater if it was set, the time we started the movement on server
    /// and finally if it was an npc moving or a player
    /// </summary>
    /// <param name="buffer">The buffer message containing movement data</param>
    public void ExecutePacket(IByteBuffer buffer)
    {
        int guidLength = buffer.ReadInt();
        Guid index = Guid.Parse(buffer.ReadString(guidLength, Encoding.Default));


        Vector3 oldPos = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
        Quaternion oldRotation = Quaternion.Euler(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());


        Vector3 pos = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
        Quaternion newRotation = Quaternion.Euler(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());

        float moveSpeed = buffer.ReadFloat();
        float horizontalInput = buffer.ReadFloat();
        float verticalInput = buffer.ReadFloat();
        bool isStrafing = buffer.ReadBoolean();

        float timeToLerp = buffer.ReadFloat();
        bool isNpc = buffer.ReadBoolean();
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject player = null;
            var hasVal = GameManager.Instance.ServerSpawns.TryGetValue(index, out player);

            if (hasVal)
            {
                Lerp(index, player, oldPos, pos, oldRotation, newRotation, moveSpeed, horizontalInput, verticalInput, isStrafing, timeToLerp);
            }
        });
    }

    /// <summary>
    /// This function is used to move a player based on % covered to goal. We set it here and then update in on movesync <see cref="MoveSync"/>
    /// </summary>
    /// <param name="index">Game object id moving</param>
    /// <param name="a_player">The actual game object</param>
    /// <param name="a_lastRealPosition">The Position it was originally at before movement starterd on the server </param>
    /// <param name="a_realPosition">The Position after movement was done on server</param>
    /// <param name="a_lastRotation">The Rotation it was originally at before movement starterd on the server </param>
    /// <param name="a_realrotation">The Rotation after movement was done on server<</param>
    /// <param name="a_moveSpeed">The movement speed we are traveling at</param>
    /// <param name="a_horizontal">Sideways movement for strafing</param>
    /// <param name="a_vertical">Forward movement input speed</param>
    /// <param name="strafe">Strafe paramater for animator</param>
    /// <param name="timeToLerp">Float value for when server started to send the movement</param>
    private void Lerp(Guid index, GameObject a_player, Vector3 a_lastRealPosition, Vector3 a_realPosition, Quaternion a_lastRotation, Quaternion a_realrotation, float a_moveSpeed, float a_horizontal, float a_vertical, bool strafe, float timeToLerp)
    {
        if (a_player == null)
        {
            //UnityEngine.Debug.Log("error no character: ", a_player);
            return;
        }
        float timeStartedLerping = Time.deltaTime;

        var animator = a_player.GetComponent<Animator>();

        if (animator != null && index.ToString() != NetworkManager.instance.myIndex.ToString())
        {
            ////Debug.Log("non local player speed changed for animator");
            animator.SetBool("IsStrafing", strafe);
            animator.SetFloat("Speed", a_moveSpeed);
            animator.SetFloat("HorizontalInput", a_horizontal);
            animator.SetFloat("VerticalInput", a_vertical);
        }

        float lerpPercentage = (Time.deltaTime * 15 - timeStartedLerping / timeToLerp);
        //a_player.transform.position = Vector3.Lerp(a_player.transform.position, a_realPosition, timeToLerp);

        lerpPercentage = (Time.deltaTime * 30 - timeStartedLerping / timeToLerp);

        var movesync = a_player.GetComponent<MoveSync>();
        movesync.lerpTime = timeToLerp;
        movesync.lastreal = a_lastRealPosition;
        movesync.lastrotation = a_lastRotation;
        movesync.endGoal = a_realPosition;
        movesync.endRotation = a_realrotation;

        movesync.doLerp = true;

        movesync.StartLerp();
        //a_player.transform.rotation = Quaternion.Lerp(a_player.transform.rotation, a_realrotation, timeToLerp);

    }
}
