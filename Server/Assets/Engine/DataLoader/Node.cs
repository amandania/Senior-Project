using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Engine.DataLoader
{
    public class Node : IItem<Node>
    {
        public bool walkable;
        public Vector3 worldPosition;

        public short gridX;
        public short gridY;
        public int movementPenalty;

        public int gCost;
        public int hCost;
        internal Node parent;
        int heapIndex;

        public Node(bool _walkable, Vector3 _worldPos, short _gridX, short _gridY, int _penalty)
        {
            walkable = _walkable;
            worldPosition = _worldPos;
            gridX = _gridX;
            gridY = _gridY;
            movementPenalty = _penalty;
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public int _Index
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }
    }
}
