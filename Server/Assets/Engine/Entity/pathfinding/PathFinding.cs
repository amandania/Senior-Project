using Engine.DataLoader;
using Engine.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Engine.Entity.pathfinding
{
    public class PathFinding : IPathFinding
    {
        public ILoadMapData Grid { get; }
        
        public PathFinding(ILoadMapData globalGrid)
        {
            Grid = globalGrid;
        }

        public PathResult FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;

            Node startNode = Grid.NodeFromWorldPoint(startPos);
            Node targetNode = Grid.NodeFromWorldPoint(targetPos);
           
            startNode.m_parent = startNode;


            if (startNode.m_walkable && targetNode.m_walkable)
            {
                CustomHeap<Node> openSet = new CustomHeap<Node>(Grid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        //print ("Path found: " + sw.ElapsedMilliseconds + " ms");
                        pathSuccess = true;
                        break;
                    }

                    foreach (Node neighbour in Grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.m_walkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newMovementCostToNeighbour = currentNode.m_gCost + GetDistance(currentNode, neighbour) + neighbour.m_movementPenalty;
                        if (newMovementCostToNeighbour < neighbour.m_gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.m_gCost = newMovementCostToNeighbour;
                            neighbour.m_hCost = GetDistance(neighbour, targetNode);
                            neighbour.m_parent = currentNode;

                            if (!openSet.Contains(neighbour))
                                openSet.Add(neighbour);
                            else
                                openSet.UpdateItem(neighbour);
                        }
                    }
                }   
            }
            if (pathSuccess)
            {
                waypoints = RetracePath(startNode, targetNode);
                pathSuccess = waypoints.Length > 0;
            }
            return new PathResult(waypoints, pathSuccess);
        }

        public class PathResult
        {
            public Vector3[] path;
            public bool success;

            public PathResult(Vector3[] path, bool success)
            {
                this.path = path;
                this.success = success;
            }
        }
        Vector3[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.m_parent;
            }
            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            return waypoints;

        }

        Vector3[] SimplifyPath(List<Node> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = new Vector2(path[i - 1].m_gridX - path[i].m_gridX, path[i - 1].m_gridY - path[i].m_gridY);
                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i].m_worldPosition);
                }
                directionOld = directionNew;
            }
            return waypoints.ToArray();
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Math.Abs(nodeA.m_gridX - nodeB.m_gridX);
            int dstY = Math.Abs(nodeA.m_gridY - nodeB.m_gridY);

            if (dstX > dstY)
                return 6 * dstY + 2 * (dstX - dstY);
            return 6 * dstX +  2 * (dstY - dstX);
        }

    }
}
