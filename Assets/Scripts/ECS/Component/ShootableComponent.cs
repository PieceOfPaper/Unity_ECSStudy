using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

public struct ShootableComponent : IComponentData
{
    public Entity BulletPrefab;
    public float3 ShootOffset;
    public int BulletCount;
    public int BulletCountMax;
    public float BulletReloadTime;
    public float BulletReloadTimeMax;
    public float BulletShotCooltime;
    public float BulletLastShotTime;

    public bool OnKeyFire;
}

