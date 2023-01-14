using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public enum HitLayerType
{
    Player = 1,
    Enemy = 2,
}

public partial struct HitLayerComponent : IComponentData
{
    public HitLayerType attackLayerMask;
    public HitLayerType hitLayer;
}
