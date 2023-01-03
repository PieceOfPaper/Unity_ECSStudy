using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct CharacterComponent : IComponentData
{
    public int HP;
    public int MaxHP;
    public int BulletCount;
    public int BulletCountMax;
    public float BulletReloadTime;
    public float BulletReloadTimeMax;
    public float BulletShotCooltime;
    public float BulletLastShotTime;
}
