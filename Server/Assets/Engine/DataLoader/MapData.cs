using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// This class is now deprecated but was used to load up the games map data in grid format. We used this grid to create a pathfinding algirthm using A* implemntation. <see cref="IPathFinding"/> implementation.
/// This was a very early implementation of what i wanted to accomplish in this project. Ended up deprecating it.
/// </summary>
public class MapData : ILoadMapData
{
    /// <summary>
    /// Dictionary for node data in compressed format.
    /// </summary>
    private Dictionary<Tuple<short, short>, long> m_mapIndexData = new Dictionary<Tuple<short, short>, long>();

    //Reader file
    private BinaryReader m_mapDataFileReader;

    //Grid data loaded for each node
    private Node[,] m_grid { get; set; }

    public int PenaltyMin = int.MaxValue;
    public int PenaltyMax = int.MinValue;

    //Grid dimensions
    public const int m_gridSizeX = 500;
    public const int m_gridSizeY = 500;

    //Vector2 of grid size
    public System.Numerics.Vector2 GridWorldSize { get; } = new System.Numerics.Vector2(m_gridSizeX, m_gridSizeY);

    public int MaxSize
    {
        get
        {
            return m_gridSizeX * m_gridSizeY;
        }
    }

    /// <summary>
    /// This function returns a node in the grid based on given world position (vector3)
    /// </summary>
    /// <param name="a_worldPosition">Position to find a node for</param>
    /// <returns>Node position</returns>
    public Node NodeFromWorldPoint(Vector3 a_worldPosition)
    {
        float percentX = (a_worldPosition.x + GridWorldSize.X / 2) / GridWorldSize.X;
        float percentY = (a_worldPosition.z + GridWorldSize.Y / 2) / GridWorldSize.Y;
        percentX = Clamp01(percentX);
        percentY = Clamp01(percentY);

        short x = (short)Math.Round((double)(m_gridSizeX - 1) * percentX);
        short y = (short)Math.Round((double)(m_gridSizeX - 1) * percentY);
        return GetOrLoadGrid(x, y);
    }

    /// <summary>
    /// Clam  a value between 0 and 1
    /// </summary>
    /// <param name="a_value"></param>
    /// <returns>0 or 1</returns>
    public float Clamp01(float a_value)
    {
        if ((double)a_value < 0.0)
            return 0.0f;
        if ((double)a_value > 1.0)
            return 1f;
        return a_value;
    }

    /// <summary>
    /// This function either returns a node data from our cached list of grids or loads the actual grid data from the file.
    /// </summary>
    /// <param name="a_x">node x pos</param>
    /// <param name="a_y">node y pos</param>
    /// <returns></returns>
    public Node GetOrLoadGrid(short a_x, short a_y)
    {
        if (m_grid[a_x, a_y] == null)
        {
            LoadGrid(a_x, a_y);
        }

        return m_grid[a_x, a_y];
    }


    /// <summary>
    /// This function returns the 4 neighboring nodes 
    /// </summary>
    /// <param name="a_node">Node to get the neighbors for</param>
    /// <returns>list of neighbor nodes</returns>
    public List<Node> GetNeighbours(Node a_node)
    {
        List<Node> neighbours = new List<Node>();

        for (short x = -1; x <= 1; x++)
        {
            for (short y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                short checkX = (short)(a_node.GridX + x);
                short checkY = (short)(a_node.GridY + y);

                if (checkX >= 0 && checkX < m_gridSizeX && checkY >= 0 && checkY < m_gridSizeY)
                {
                    neighbours.Add(GetOrLoadGrid(checkX, checkY));
                }
            }
        }
        return neighbours;
    }

    /// <summary>
    /// Clamp a value inbetween a min and max range
    /// </summary>
    /// <param name="a_value">Value to clam</param>
    /// <param name="a_min">Min range</param>
    /// <param name="a_max">Max range</param>
    /// <returns></returns>
    public int Clamp(int a_value, int a_min, int a_max)
    {
        if (a_value < a_min)
            a_value = a_min;
        if (a_value > a_max)
            a_value = a_max;
        return a_value;
    }


    //** Below is what loads the data.
    public static void CopyTo(Stream a_src, Stream a_dest)
    {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = a_src.Read(bytes, 0, bytes.Length)) != 0)
        {
            a_dest.Write(bytes, 0, cnt);
        }
    }

    public class DataStuff
    {
        /// <summary>
        /// This class is a tempoary serialized class. We use this same class to export client map data This DataStuff is used as a heap item for our pathfinding algorithm
        /// </summary>
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

    /// <summary>
    /// Function to load the actual x and y node data from the compressed data we loaded on startup
    /// </summary>
    /// <param name="a_x"></param>
    /// <param name="a_y"></param>
    public void LoadGrid(short a_x, short a_y)
    {
        lock (m_mapDataFileReader)
        {
            if (m_grid[a_x, a_y] != null)
            {
                return;
            }

            var position = m_mapIndexData[new Tuple<short, short>(a_x, a_y)];
            m_mapDataFileReader.BaseStream.Position = position;

            var walkable = m_mapDataFileReader.ReadBoolean();
            var worldPositionX = m_mapDataFileReader.ReadSingle();
            var worldPositionY = m_mapDataFileReader.ReadSingle();
            var movementPenalty = m_mapDataFileReader.ReadInt32();
            m_grid[a_x, a_y] = new Node(walkable,
                new Vector3(worldPositionX, 0, worldPositionY), a_x, a_y, movementPenalty);
        }
    }

    /// <summary>
    /// This function is started on server startup to load object data containing the x and y poistion along with a walkable flag
    /// </summary>
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
