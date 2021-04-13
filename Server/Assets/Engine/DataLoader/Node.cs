using UnityEngine;

/// <summary>
/// This class is used to represant a node item in our heap
/// </summary>
public class Node : IItem<Node>
{

    public bool Walkable;
    public Vector3 WorldPosition;

    public short GridX;
    public short GridY;
    public int MovementPenalty;

    public int GCost;
    public int HCost;
    public int HeapIndex;
    public Node Parent;

    /// <summary>
    /// Constrctor for node flag
    /// </summary>
    /// <param name="a_walkable">Is this grid position blocked by an obstacle</param>
    /// <param name="a_worldPos">World position of node</param>
    /// <param name="a_gridX">List index X</param>
    /// <param name="a_gridY">List index Y</param>
    /// <param name="a_penalty">Distance penalty</param>
    public Node(bool a_walkable, Vector3 a_worldPos, short a_gridX, short a_gridY, int a_penalty)
    {
        Walkable = a_walkable;
        WorldPosition = a_worldPos;
        GridX = a_gridX;
        GridY = a_gridY;
        MovementPenalty = a_penalty;
    }
    
    public int m_fCost
    {
        get
        {
            return GCost + HCost;
        }
    }

    public int m_index
    {
        get
        {
            return HeapIndex;
        }
        set
        {
            HeapIndex = value;
        }
    }

    /// <summary>
    /// Compare one node to another against the hcost or distance to each node
    /// </summary>
    /// <param name="nodeToCompare"></param>
    /// <returns>The difference in cost</returns>
    public int CompareTo(Node nodeToCompare)
    {
        int compare = m_fCost.CompareTo(nodeToCompare.m_fCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(nodeToCompare.HCost);
        }
        return -compare;
    }
}