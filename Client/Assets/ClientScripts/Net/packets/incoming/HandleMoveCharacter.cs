using DotNetty.Buffers;
using System;
using System.Text;
using UnityEngine;

public class HandleMoveCharacter : IIncomingPacketHandler
{
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
    public IncomingPackets PacketType => IncomingPackets.HANDLE_MOVE_CHARACTER;

}
