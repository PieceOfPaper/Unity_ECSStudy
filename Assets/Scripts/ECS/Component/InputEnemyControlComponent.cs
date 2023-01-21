using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public partial struct InputEnemyControlComponent : IComponentData
{
    public Unity.Mathematics.Random random;
    public double nextMovableUpdateTime;
    public double nextShootableUpdateTime;
}
