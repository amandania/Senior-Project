using Autofac;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IWorld : IStartable, IDisposable
{
				Transform SpawnTransform { get; set; }
				
				List<Player> Players { get; set; }
				List<Npc> Monsters { get; set; }

				void AddWorldCharacter(Character a_player);
				void RemoveWorldCharacter(Character a_player);

				GameObject GetDefaultPlayerModel();

				Task LoadMonsters();

}
