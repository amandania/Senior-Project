using System;

public class CustomHeap<T> where T : IItem<T>
{

    public T[] _Items;
    public int _CurrentItemCount;

    public CustomHeap(int maxHeapSize)
    {
        _Items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.m_index = _CurrentItemCount;
        _Items[_CurrentItemCount] = item;
        SortUp(item);
        _CurrentItemCount++;
    }
    public T RemoveFirst()
    {
        T first = _Items[0];
        _CurrentItemCount--;
        _Items[0] = _Items[_CurrentItemCount];
        _Items[0].m_index = 0;
        SortDown(_Items[0]);
        return first;
    }

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
    //When are these methods called? when 
    public bool Contains(T item)
    {
        return Equals(_Items[item.m_index], item);
    }

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

    private void Swap(T itemA, T itemB)
    {
        _Items[itemA.m_index] = itemB;
        _Items[itemB.m_index] = itemA;
        int AIndex = itemA.m_index;
        itemA.m_index = itemB.m_index;
        itemB.m_index = AIndex;
    }

}

public interface IItem<T> : IComparable<T>
{
    int m_index
    {
        get;
        set;
    }
}
