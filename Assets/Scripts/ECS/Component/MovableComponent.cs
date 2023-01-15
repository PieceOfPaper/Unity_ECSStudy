using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct MovableComponent : IComponentData
{
    public Vector3 moveDir;
    public float moveSpeed;
}
