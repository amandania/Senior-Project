using UnityEngine;
using DotNetty.Buffers;
using System;
using System.Text;

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

								UnityMainThreadDispatcher.Instance().Enqueue(() =>
								{
												GameObject player = null;
												var hasVal = GameManager.instance.ServerSpawns.TryGetValue(index, out player);
												
												if (hasVal) {
																Lerp(index, player, oldPos, pos, oldRotation, newRotation, moveSpeed, horizontalInput, verticalInput);
												}
								});
				}
				void Lerp(Guid index, GameObject a_player, Vector3 a_lastRealPosition, Vector3 a_realPosition, Quaternion a_lastRotation, Quaternion a_realrotation, float a_moveSpeed, float a_horizontal, float a_vertical)
				{
								if (a_player == null) {
												return;
								}

								var animator = a_player.GetComponent<Animator>();

								float timeStartedLerping = Time.time;
								float timeToLerp = 20;
								float travelSpeed = (a_player.transform.position - a_realPosition).magnitude;
								if (animator != null && index.ToString() != NetworkManager.instance.myIndex.ToString()) {
												//Debug.Log("non local player speed changed for animator");
												animator.SetFloat("Speed", a_moveSpeed);
            animator.SetFloat("HorizontalInput", a_horizontal);
            animator.SetFloat("VerticalInput", a_vertical);
								}
								float lerpPercentage = (Time.time - timeStartedLerping / timeToLerp);
								a_player.transform.position = Vector3.Lerp(a_player.transform.position, a_realPosition, lerpPercentage);

								lerpPercentage = (Time.time - timeStartedLerping / timeToLerp);


        a_player.transform.rotation = Quaternion.Lerp(a_player.transform.rotation, a_realrotation, lerpPercentage);

				}
				public IncomingPackets PacketType => IncomingPackets.HANDLE_MOVE_CHARACTER;

}
