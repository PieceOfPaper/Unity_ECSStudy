using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CharacterAuthoring : MonoBehaviour
{
    [Tooltip("체력")] public int HP = 100;
    [Tooltip("총알 수")] public int BulletCount = 6;
    [Tooltip("총알 장전 시간")] public float BulletReloadTime = 1.0f;
    [Tooltip("총알 발사 쿨타임")] public float BulletShotCooltime = 0.2f;
}

public class CharacterBaker : Baker<CharacterAuthoring>
{
    public override void Bake(CharacterAuthoring authoring)
    {
        AddComponent(new CharacterComponent()
        {
            HP = authoring.HP,
            MaxHP = authoring.HP,
            BulletCount = authoring.BulletCount,
            BulletCountMax = authoring.BulletCount,
            BulletReloadTime = authoring.BulletReloadTime,
            BulletReloadTimeMax = authoring.BulletReloadTime,
            BulletShotCooltime = authoring.BulletShotCooltime,
            BulletLastShotTime = 0f,
        });
    }
}