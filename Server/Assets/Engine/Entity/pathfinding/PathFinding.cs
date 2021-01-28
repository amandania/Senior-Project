using Engine.DataLoader;
using Engine.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Engine.Entity.pathfinding
{
    public class PathFinding : IPathFinding
    {
        public ILoadMapData grid { get; }
        
        public PathFinding(ILoadMapData globalGrid)
        {
            grid = globalGrid;
        }

        public PathResult FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;

            Node startNode = grid.NodeFromWorldPoint(startPos);
            Node targetNode = grid.NodeFromWorldPoint(targetPos);
           
            startNode.parent = startNode;


            if (startNode.walkable && targetNode.walkable)
            {
                CustomHeap<Node> openSet = new CustomHeap<Node>(grid.MaxSize);
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

                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.walkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.parent = currentNode;

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
                currentNode = currentNode.parent;
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
                Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i].worldPosition);
                }
                directionOld = directionNew;
            }
            return waypoints.ToArray();
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Math.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Math.Abs(nodeA.gridY - nodeB.gridY);

            if (dstX > dstY)
                return 6 * dstY + 2 * (dstX - dstY);
            return 6 * dstX +  2 * (dstY - dstX);
        }

    }
}
