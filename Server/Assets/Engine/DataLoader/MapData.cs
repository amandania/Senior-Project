using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Engine.DataLoader
{
    public class MapData : ILoadMapData
    {
        private Dictionary<Tuple<short, short>, long> _mapIndexData = new Dictionary<Tuple<short, short>, long>();
        private BinaryReader _mapDataFileReader;
        Node[,] grid { get; set; }
        public int penaltyMin = int.MaxValue;
        public int penaltyMax = int.MinValue;
        public int gridSizeX = 500;
        public int gridSizeY = 500;

        public System.Numerics.Vector2 gridWorldSize { get; } = new System.Numerics.Vector2(2048, 2048);
        public int MaxSize
        {
            get
            {
                return gridSizeX * gridSizeY;
            }
        }
        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {


            float percentX = (worldPosition.x + gridWorldSize.X / 2) / gridWorldSize.X;
            float percentY = (worldPosition.z + gridWorldSize.Y / 2) / gridWorldSize.Y;
            percentX = Clamp01(percentX);
            percentY = Clamp01(percentY);


            short x = (short)Math.Round((double)(gridSizeX - 1) * percentX);
            short y = (short)Math.Round((double)(gridSizeX - 1) * percentY);
            return GetOrLoadGrid(x, y);
        }
        public float Clamp01(float value)
        {
            if ((double)value < 0.0)
                return 0.0f;
            if ((double)value > 1.0)
                return 1f;
            return value;
        }

        public Node GetOrLoadGrid(short x, short y)
        {
            if (grid[x, y] == null)
            {
                LoadGrid(x, y);
            }

            return grid[x, y];
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (short x = -1; x <= 1; x++)
            {
                for (short y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    short checkX = (short) (node.gridX + x);
                    short checkY = (short) (node.gridY + y);

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(GetOrLoadGrid(checkX, checkY));
                    }
                }
            }
            return neighbours;
        }


        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                value = min;
            if (value > max)
                value = max;
            return value;
        }


        //** Below is what loads the data.
        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public class DataStuff
        {
            public bool walkable;
            public float worldPositionX;
            public float worldPositionY;
            public float worldPositionZ;
            public short gridX;
            public short gridY;
            public int movementPenalty;

            public int gCost;
            public int hCost;
            public Node parent;
            int heapIndex;

            public DataStuff(bool _walkable, float _worldPosX, float _worldPosY, float _worldPosZ, short _gridX, short _gridY, int _penalty)
            {
                walkable = _walkable;
                worldPositionX = _worldPosX;
                worldPositionZ = _worldPosZ;
                worldPositionZ = _worldPosY;
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

            public int HeapIndex
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
        }

        public void LoadGrid(short x, short y)
        {
            lock (_mapDataFileReader)
            {
                if (grid[x, y] != null)
                {
                    return;
                }

                var position = _mapIndexData[new Tuple<short, short>(x, y)];
                _mapDataFileReader.BaseStream.Position = position;

                var walkable = _mapDataFileReader.ReadBoolean();
                var worldPositionX = _mapDataFileReader.ReadSingle();
                var worldPositionY = _mapDataFileReader.ReadSingle();
                var movementPenalty = _mapDataFileReader.ReadInt32();
                grid[x, y] = new Node(walkable,
                    new Vector3(worldPositionX, 0, worldPositionY), x, y, movementPenalty);
																Debug.Log(x + ", " + y + " is walkable:" + walkable);
            }
        }

        public void Start()
        {
            grid = new Node[gridSizeX, gridSizeY];
            _mapDataFileReader = new BinaryReader(File.Open(Path.Combine(Application.dataPath, "MapData/object.data"), FileMode.Open));

            //Load indexes
            using (var streamReader = new BinaryReader(File.Open(Path.Combine(Application.dataPath, "MapData/object.idx"), FileMode.Open)))
            {
                while (streamReader.BaseStream.Position < streamReader.BaseStream.Length)
                {
                    var x = streamReader.ReadInt16();
                    var y = streamReader.ReadInt16();
                    var position = streamReader.ReadInt64();
                    _mapIndexData.Add(new Tuple<short, short>(x, y), position);
                }
            }
        }

        public void Dispose()
        {
            _mapDataFileReader.Dispose();
        }


    }
}
