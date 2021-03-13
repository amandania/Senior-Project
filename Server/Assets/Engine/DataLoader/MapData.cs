using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Engine.DataLoader
{
    public class MapData : ILoadMapData
    {
        private Dictionary<Tuple<short, short>, long> m_mapIndexData = new Dictionary<Tuple<short, short>, long>();
        private BinaryReader m_mapDataFileReader;

        private Node[,] m_grid { get; set; }
        public int m_penaltyMin = int.MaxValue;
        public int m_penaltyMax = int.MinValue;
        public const int m_gridSizeX = 500;
        public const int m_gridSizeY = 500;

        public System.Numerics.Vector2 m_gridWorldSize { get; } = new System.Numerics.Vector2(m_gridSizeX, m_gridSizeY);

        public int MaxSize
        {
            get
            {
                return m_gridSizeX * m_gridSizeY;
            }
        }

        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + m_gridWorldSize.X / 2) / m_gridWorldSize.X;
            float percentY = (worldPosition.z + m_gridWorldSize.Y / 2) / m_gridWorldSize.Y;
            percentX = Clamp01(percentX);
            percentY = Clamp01(percentY);

            short x = (short)Math.Round((double)(m_gridSizeX - 1) * percentX);
            short y = (short)Math.Round((double)(m_gridSizeX - 1) * percentY);
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
            if (m_grid[x, y] == null)
            {
                LoadGrid(x, y);
            }

            return m_grid[x, y];
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

                    short checkX = (short)(node.m_gridX + x);
                    short checkY = (short)(node.m_gridY + y);

                    if (checkX >= 0 && checkX < m_gridSizeX && checkY >= 0 && checkY < m_gridSizeY)
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
            private int heapIndex;

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
            lock (m_mapDataFileReader)
            {
                if (m_grid[x, y] != null)
                {
                    return;
                }

                var position = m_mapIndexData[new Tuple<short, short>(x, y)];
                m_mapDataFileReader.BaseStream.Position = position;

                var walkable = m_mapDataFileReader.ReadBoolean();
                var worldPositionX = m_mapDataFileReader.ReadSingle();
                var worldPositionY = m_mapDataFileReader.ReadSingle();
                var movementPenalty = m_mapDataFileReader.ReadInt32();
                m_grid[x, y] = new Node(walkable,
                    new Vector3(worldPositionX, 0, worldPositionY), x, y, movementPenalty);
            }
        }

        public void Start()
        {
            m_grid = new Node[m_gridSizeX, m_gridSizeY];
            m_mapDataFileReader = new BinaryReader(File.Open(Path.Combine(Application.dataPath, "MapData/object.data"), FileMode.Open));

            //Load indexes
            using (var streamReader = new BinaryReader(File.Open(Path.Combine(Application.dataPath, "MapData/object.idx"), FileMode.Open)))
            {
                while (streamReader.BaseStream.Position < streamReader.BaseStream.Length)
                {
                    var x = streamReader.ReadInt16();
                    var y = streamReader.ReadInt16();
                    var position = streamReader.ReadInt64();
                    m_mapIndexData.Add(new Tuple<short, short>(x, y), position);
                }
            }
        }

        public void Dispose()
        {
            m_mapDataFileReader.Dispose();
        }


    }
}
