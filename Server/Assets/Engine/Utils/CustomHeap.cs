using System;
/// <summary>
/// This class is was made to demonstrate my understanding on what a Heap is and how to properly use them to get optimal sorting algorithm. 
/// Although this class is now deprecated. The main purpose was to be able to update heap items based on their respect heap index.
/// We used this for our pathfinding class to add node positions in our list possible nodes to target.
/// <see cref="PathFinding.FindPath(UnityEngine.Vector3, UnityEngine.Vector3)"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public class CustomHeap<T> where T : IItem<T>
{
    //List of items in our heap
    public T[] _Items;

    //Item count tracker
    public int _CurrentItemCount;

    /// <summary>
    /// Constructor to create a fixed heap size list
    /// </summary>
    /// <param name="maxHeapSize"></param>
    public CustomHeap(int maxHeapSize)
    {
        _Items = new T[maxHeapSize];
    }

    /// <summary>
    /// This function will add an item to our heap and sortup from the insert position. 
    /// </summary>
    /// <param name="item"></param>
    public void Add(T item)
    {
        item.m_index = _CurrentItemCount;
        _Items[_CurrentItemCount] = item;
        SortUp(item);
        _CurrentItemCount++;
    }

    /// <summary>
    /// Pop the first node of the list out and sort the elemnts that come after it to match new first node.
    /// </summary>
    /// <returns></returns>
    public T RemoveFirst()
    {
        T first = _Items[0];
        _CurrentItemCount--;
        _Items[0] = _Items[_CurrentItemCount];
        _Items[0].m_index = 0;
        SortDown(_Items[0]);
        return first;
    }

    /// <summary>
    /// Sortup based on current heap item.
    /// </summary>
    /// <param name="item">Item in heap</param>
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return _CurrentItemCount;
        }
    }

    /// <summary>
    /// Function to check if our item is in our heap
    /// </summary>
    /// <param name="item">Item to check against</param>
    /// <returns>true if its item exist or false</returns>
    public bool Contains(T item)
    {
        return Equals(_Items[item.m_index], item);
    }

    /// <summary>
    /// This function will sort based on our current item.
    /// We check the left and right nodes based on current index which in our list we know its current index + 1 for left node and  + 2 for right.
    /// We use our Node.CompareTo function to check the condtion to sort by. In pathfinding its used for fcost distance check. Distance to to goal
    /// We perform swaps if match the condition to check against.
    /// </summary>
    /// <param name="item">Item to sort down by.</param>
    private void SortDown(T item)
    {
        while (true)
        {
            int childLeftIndex = item.m_index * 2 + 1;
            int childRightIndex = item.m_index * 2 + 2;
            int swapId = 0;

            if (childLeftIndex < _CurrentItemCount)
            {
                swapId = childLeftIndex;

                if (childRightIndex < _CurrentItemCount)
                {
                    if (_Items[childLeftIndex].CompareTo(_Items[childRightIndex]) < 0)
                    {
                        swapId = childRightIndex;
                    }
                }

                if (item.CompareTo(_Items[swapId]) < 0)
                {
                    Swap(item, _Items[swapId]);
                }
                else
                {
                    return;
                }

            }
            else
            {
                return;
            }
        }
    }

    /// <summary>
    /// Similar to our sortdown this is doing the same thing, but going up the list starting at the leaf nodes
    /// </summary>
    /// <param name="item">Item to sort down by.</param>
    private void SortUp(T item)
    {
        int parent = (item.m_index - 1) / 2;

        while (true)
        {
            T parentItem = _Items[parent];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parent = (item.m_index - 1) / 2;
        }
    }

    /// <summary>
    /// Node swapping function with copy creations 
    /// </summary>
    /// <param name="itemA">Node A</param>
    /// <param name="itemB">Node B</param>
    private void Swap(T itemA, T itemB)
    {
        _Items[itemA.m_index] = itemB;
        _Items[itemB.m_index] = itemA;
        int AIndex = itemA.m_index;
        itemA.m_index = itemB.m_index;
        itemB.m_index = AIndex;
    }

}
/// <summary>
/// Temporay item class for Heap Items
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IItem<T> : IComparable<T>
{
    int m_index
    {
        get;
        set;
    }
}
