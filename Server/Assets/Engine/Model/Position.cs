using System;
using UnityEngine;

public class Position {

   
    public Position(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    private float X;
    private float Y;
    private float Z;
    private Vector3 Rotation;

    public float x {
        get { return this.X; }
        set { this.X = value; }
    }
    public float y
    {
        get { return this.Y; }
        set { this.Y = value; }
    }
    public float z
    {
        get { return this.Z; }
        set { this.Z = value; }
    }
    public Vector3 rotation
    {
        get { return this.Rotation; }
        set { this.Rotation = value; }
    }


    public Vector3 GetVector3()
    {
        return new Vector3(x, y, z);
    }
    public bool isWithinDistance(Vector3 position, float range, out float distance)
    {
        float deltaX = position.x - this.x;
        float deltaZ = position.z - this.z;
        distance = (float)Math.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
        
        return distance <= range;
      
    }
    
}
