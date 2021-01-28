using System.Collections;
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
        item._Index = _CurrentItemCount;
        _Items[_CurrentItemCount] = item;
        SortUp(item);
        _CurrentItemCount++;
    }
    public T RemoveFirst()
    {
        T first = _Items[0];
        _CurrentItemCount--;
        _Items[0] = _Items[_CurrentItemCount];
        _Items[0]._Index = 0;
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
        return Equals(_Items[item._Index], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childLeftIndex = item._Index * 2 + 1;
            int childRightIndex = item._Index * 2 + 2;
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

    void SortUp(T item)
    {
        int parent = (item._Index - 1) / 2;

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

            parent = (item._Index - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        _Items[itemA._Index] = itemB;
        _Items[itemB._Index] = itemA;
        int AIndex = itemA._Index;
        itemA._Index = itemB._Index;
        itemB._Index = AIndex;
    }

}

public interface IItem<T> : IComparable<T>
{
    int _Index
    {
        get;
        set;
    }
}
