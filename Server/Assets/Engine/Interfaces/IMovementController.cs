using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Engine.Interfaces
{
    public interface IMovementController
    {
        Task Move(Player character, Vector3 moveVector);
        Task Move(Player character, float angle, List<int> keys);
    }
}