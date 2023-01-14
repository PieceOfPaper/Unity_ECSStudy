using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class HitLayerAuthoring : MonoBehaviour
{
    public int attackLayerMask = 0;
    public HitLayerType hitLayer = 0;
}

public class HitLayerBaker : Baker<HitLayerAuthoring>
{
    public override void Bake(HitLayerAuthoring authoring)
    {
        AddComponent(new HitLayerComponent()
        {
            attackLayerMask = (HitLayerType)authoring.attackLayerMask,
            hitLayer = authoring.hitLayer,
        });
    }
}