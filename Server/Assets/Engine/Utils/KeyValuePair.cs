using System;

[Serializable]
public class KeyValuePair
{
    public string Key;
    public int Value;

    public KeyValuePair(string key, int value)
    {
        Key = key;
        Value = value;
    }
}