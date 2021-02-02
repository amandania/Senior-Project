using System;

[Serializable]
public struct DictionarySeralized<Key, Value>
{
    public Key name;
    public Value image;
}