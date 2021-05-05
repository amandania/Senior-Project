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
    /// <param name="a_key">name of our dictionary key</param>
    /// <param name="a_value">bool value type</param>
    public KeyValuePair(string a_key, bool a_value)
    {
        Key = a_key;
        BoolValue = a_value;
    }
   

    /// <summary>
    /// Constructor for int value 
    /// </summary>
    /// <param name="a_key">name of our dictionary key</param>
    /// <param name="a_value">int value type</param>
    public KeyValuePair(string a_key, int a_value)
    {
        Key = a_key;
        IntValue = a_value;
    }
    /// <summary>
    /// Constructor for float value 
    /// </summary>
    /// <param name="a_key">name of our dictionary key</param>
    /// <param name="a_value">float value type</param>
    public KeyValuePair(string a_key, float a_value)
    {
        Key = a_key;
        FloatValue = a_value;
    }
}