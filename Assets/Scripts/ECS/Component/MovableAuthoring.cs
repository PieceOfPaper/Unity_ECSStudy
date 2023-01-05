using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MovableAuthoring : MonoBehaviour
{
}

public class MovableBaker : Baker<MovableAuthoring>
{
    public override void Bake(MovableAuthoring authoring)
    {
        AddComponent(new MovableComponent()
        {
        });
    }
}