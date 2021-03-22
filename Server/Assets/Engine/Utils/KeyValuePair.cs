using System;
using UnityEngine;
/// <summary>
/// This is a serialiable class for unity to display a special key value pair on the inspector. Since unity doesnt show a dictionary on inspector i had to create a special keyvalue class to represent a list item
/// </summary>

[Serializable]
public class KeyValuePair
{

    //Refrence key to dictionary
    public string Key;
    
    //Possible object values
    public bool BoolValue;
    public int IntValue;
    public float FloatValue;

    /// <summary>
    /// Constructor for bool value 
    /// </summary>
    /// <param name="key">name of our dictionary key</param>
    /// <param name="value">bool value type</param>
    public KeyValuePair(string key, bool value)
    {
        Key = key;
        BoolValue = value;
    }
   

    /// <summary>
    /// Constructor for int value 
    /// </summary>
    /// <param name="key">name of our dictionary key</param>
    /// <param name="value">int value type</param>
    public KeyValuePair(string key, int value)
    {
        Key = key;
        IntValue = value;
    }
    /// <summary>
    /// Constructor for float value 
    /// </summary>
    /// <param name="key">name of our dictionary key</param>
    /// <param name="value">float value type</param>
    public KeyValuePair(string key, float value)
    {
        Key = key;
        FloatValue = value;
    }
}