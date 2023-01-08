using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ShootableAuthoring : MonoBehaviour
{
    [Tooltip("총알 프리팹")] public GameObject BulletPrefab;
    [Tooltip("총알 발사 위치")] public Vector3 ShootOffset;
    [Tooltip("총알 수")] public int BulletCount = 6;
    [Tooltip("총알 장전 시간")] public float BulletReloadTime = 1.0f;
    [Tooltip("총알 발사 쿨타임")] public float BulletShotCooltime = 0.2f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(ShootOffset, 0.1f);
    }
}

public class ShootableBaker : Baker<ShootableAuthoring>
{
    public override void Bake(ShootableAuthoring authoring)
    {
        AddComponent(new ShootableComponent()
        {
            BulletPrefab = GetEntity(authoring.BulletPrefab),
            ShootOffset = authoring.ShootOffset,
            BulletCount = 0,
            BulletCountMax = authoring.BulletCount,
            BulletReloadTime = 0f,
            BulletReloadTimeMax = authoring.BulletReloadTime,
            BulletShotCooltime = authoring.BulletShotCooltime,
            BulletLastShotTime = 0f,
        });
    }
}
