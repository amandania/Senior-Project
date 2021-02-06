using System;

[Serializable]
public class KeyValuePair
{
    public string Key;
    public object Value;

    public KeyValuePair(string key, object value)
    {
        Key = key;
        Value = value;
    }
}