using UnityEngine;
using System.Collections;
using System;

public class CombatComponent : MonoBehaviour
{
				private NetworkManager Network;

				public Character Character { get; set; }

				[Header("Combat Data")]
				public Transform m_characterTransform;
				public Transform targetTransform;
				
				private void Awake()
				{
								m_characterTransform = transform;
				}

				private void Update()
				{
								
				}

				public void Attack(Character target)
				{

				}
}
