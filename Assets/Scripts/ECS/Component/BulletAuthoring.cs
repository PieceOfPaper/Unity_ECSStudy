using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class BulletAuthoring : MonoBehaviour
{
    [Tooltip("데미지")] public int Damage = 1;
    [Tooltip("시간")] public float Duration = 1.0f;
}

public partial class BulletBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        AddComponent(new BulletComponent()
        {
            Damage = authoring.Damage,
            Duration = authoring.Duration,
        });
    }
}