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
        public bool m_walkable;
        public Vector3 m_worldPosition;

        public short m_gridX;
        public short m_gridY;
        public int m_movementPenalty;

        public int m_gCost;
        public int m_hCost;
        public int m_heapIndex;
        internal Node m_parent;

        public Node(bool _walkable, Vector3 _worldPos, short _gridX, short _gridY, int _penalty)
        {
            this.m_walkable = _walkable;
            m_worldPosition = _worldPos;
            this.m_gridX = _gridX;
            m_gridY = _gridY;
            m_movementPenalty = _penalty;
        }

        public int m_fCost
        {
            get
            {
                return m_gCost + m_hCost;
            }
        }

        public int m_index
        {
            get
            {
                return m_heapIndex;
            }
            set
            {
                m_heapIndex = value;
            }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = m_fCost.CompareTo(nodeToCompare.m_fCost);
            if (compare == 0)
            {
                compare = m_hCost.CompareTo(nodeToCompare.m_hCost);
            }
            return -compare;
        }
    }
}
