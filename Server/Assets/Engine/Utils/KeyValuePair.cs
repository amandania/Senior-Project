using System;
using UnityEngine;

[Serializable]
public class KeyValuePair
{
    public string Key;
    
    public bool BoolValue;
    public int IntValue;
    public float FloatValue;

    public KeyValuePair(string key, bool value)
    {
        Key = key;
        BoolValue = value;
    }
    public KeyValuePair(string key, int value)
    {
        Key = key;
        IntValue = value;
    }
    public KeyValuePair(string key, float value)
    {
        Key = key;
        FloatValue = value;
    }
}