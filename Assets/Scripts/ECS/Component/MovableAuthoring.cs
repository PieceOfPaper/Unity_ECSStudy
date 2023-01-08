using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MovableAuthoring : MonoBehaviour
{
    [Tooltip("이동 속도")] public float moveSpeed = 1.0f;
}

public class MovableBaker : Baker<MovableAuthoring>
{
    public override void Bake(MovableAuthoring authoring)
    {
        AddComponent(new MovableComponent()
        {
            moveSpeed = authoring.moveSpeed,
        });
    }
}