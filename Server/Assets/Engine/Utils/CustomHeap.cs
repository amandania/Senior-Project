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
    /// <param name="a_heapSize"></param>
    public CustomHeap(int a_heapSize)
    {
        _Items = new T[a_heapSize];
    }

    /// <summary>
    /// This function will add an item to our heap and sortup from the insert position. 
    /// </summary>
    /// <param name="a_item"></param>
    public void Add(T a_item)
    {
        a_item.m_index = _CurrentItemCount;
        _Items[_CurrentItemCount] = a_item;
        SortUp(a_item);
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
    /// <param name="a_item">Item in heap</param>
    public void UpdateItem(T a_item)
    {
        SortUp(a_item);
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
    /// <param name="a_item">Item to check against</param>
    /// <returns>true if its item exist or false</returns>
    public bool Contains(T a_item)
    {
        return Equals(_Items[a_item.m_index], a_item);
    }

    /// <summary>
    /// This function will sort based on our current item.
    /// We check the left and right nodes based on current index which in our list we know its current index + 1 for left node and  + 2 for right.
    /// We use our Node.CompareTo function to check the condtion to sort by. In pathfinding its used for fcost distance check. Distance to to goal
    /// We perform swaps if match the condition to check against.
    /// </summary>
    /// <param name="a_item">Item to sort down by.</param>
    private void SortDown(T a_item)
    {
        while (true)
        {
            int childLeftIndex = a_item.m_index * 2 + 1;
            int childRightIndex = a_item.m_index * 2 + 2;
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

                if (a_item.CompareTo(_Items[swapId]) < 0)
                {
                    Swap(a_item, _Items[swapId]);
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
    /// <param name="a_item">Item to sort down by.</param>
    private void SortUp(T a_item)
    {
        int parent = (a_item.m_index - 1) / 2;

        while (true)
        {
            T parentItem = _Items[parent];
            if (a_item.CompareTo(parentItem) > 0)
            {
                Swap(a_item, parentItem);
            }
            else
            {
                break;
            }

            parent = (a_item.m_index - 1) / 2;
        }
    }

    /// <summary>
    /// Node swapping function with copy creations 
    /// </summary>
    /// <param name="a_itemA">Node A</param>
    /// <param name="a_itemB">Node B</param>
    private void Swap(T a_itemA, T a_itemB)
    {
        _Items[a_itemA.m_index] = a_itemB;
        _Items[a_itemB.m_index] = a_itemA;
        int AIndex = a_itemA.m_index;
        a_itemA.m_index = a_itemB.m_index;
        a_itemB.m_index = AIndex;
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
