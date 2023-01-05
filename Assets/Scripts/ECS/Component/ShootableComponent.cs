using System.Collections;
using System.Collections.Generic;
using Unity.Entities;

public struct ShootableComponent : IComponentData
{
    public int BulletCount;
    public int BulletCountMax;
    public float BulletReloadTime;
    public float BulletReloadTimeMax;
    public float BulletShotCooltime;
    public float BulletLastShotTime;
}

