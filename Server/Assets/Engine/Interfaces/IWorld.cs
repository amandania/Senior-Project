using Autofac;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// This interface implements the entire game world. It its in control of what exists in the server and also the inital startup tasks.
/// </summary>
public interface IWorld : IStartable, IDisposable
{
    //Server spawn position
    Transform SpawnTransform { get; set; }

    //Active players
    List<Player> Players { get; set; }

    //All monsters even dead ones that are in que to respawn
    List<Npc> Monsters { get; set; }

    //List of all interactable ground items
    List<ItemBase> GroundItems { get; set; }

    //Character controlling functions 
    void AddWorldCharacter(Character a_player);
    void RemoveWorldCharacter(Character a_player);

    //Startup tasks to boot up the world before opening the server socket
    Task LoadNpcInteracts();
    Task LoadMonsters();
    Task LoadGroundItems();

}
