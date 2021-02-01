using UnityEngine;
using System.Diagnostics;

public class CombatComponent : MonoBehaviour
{
				private NetworkManager Network; // Access main game network to send potential packets

				public Character Character { get; set; } // Gameobject character owner set during compoent addition/set

				public int AttackDistance { get; set; } = 5; // Required distance to perform attack
				public int AttackRate { get; set; } = 2; // Default: every 2 seconds we can attack 

				[Header("Combat Data")]
				public Character TargetCharacter; // Current Main Target, (doesnt neccarily have to be used with Attack(Character))
				public Transform TargetTransform;

				private float DistanceToAttack;

				private Stopwatch AttackStopwatch { get; set; } = new Stopwatch(); // We start it at 2 because this is required attack rate

				private void Awake()
				{

				}

				private void Update()
				{
								
				}

				public void Attack(Character target)
				{
								if (Character == null) 
								{
												return;
								}
								if (!CanAttack(target)) {
												return;
								}
				}
				
				public void PerformAttack(Character target) {
												
				}

				public bool CanAttack(Character target) {
								if (!WithinAttackDistance(target.GetCharModel().transform.position, out DistanceToAttack))
								{
												return false;
								}
								return AttackStopwatch.Elapsed.Seconds > AttackRate;
				}


				public bool WithinAttackDistance(Vector3 targetPosition, out float distance) {
								distance = (transform.position - targetPosition).magnitude;
								return distance <= AttackDistance;
				}


				public Character GetCharacterTarget()
				{
								return TargetCharacter;
				}
}
