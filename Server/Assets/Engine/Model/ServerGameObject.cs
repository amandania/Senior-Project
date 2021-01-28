using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerGameObject {

    public int ObjectId { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 ObjectSize { get; set; }
    public int ColliderType { get; set; }
    public Vector3 CenterCollider { get; set; }
    public float Radius { get; set; }
    public float Height { get; set; }

	public ServerGameObject(int objectId, Vector3 position, Vector3 size, int colliderType)
    {
        ObjectId = objectId;
        Position = position;
        ObjectSize = size;
        ColliderType = colliderType;
    }
    public ServerGameObject(int objectId, Vector3 position, Vector3 size, int colliderType, Vector3 centerCollider)
    {

        ObjectId = objectId;
        Position = position;
        ObjectSize = size;
        ColliderType = colliderType;
        CenterCollider = centerCollider;
    }

    public ServerGameObject(int objectId, Vector3 position, Vector3 size, int colliderType, Vector3 centerCollider, float radius)
    {
        ObjectId = objectId;
        Position = position;
        ObjectSize = size;
        ColliderType = colliderType;
        CenterCollider = centerCollider;
        Radius = radius;
    }
    public ServerGameObject(int objectId, Vector3 position, Vector3 size, int colliderType, Vector3 centerCollider, float radius, float height)
    {
        ObjectId = objectId;
        Position = position;
        ObjectSize = size;
        ColliderType = colliderType;
        CenterCollider = centerCollider;
        Radius = radius;
        Height = height;
    }
}
