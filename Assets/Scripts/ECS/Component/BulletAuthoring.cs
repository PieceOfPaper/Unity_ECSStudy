using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class BulletAuthoring : MonoBehaviour
{
    [Tooltip("속도")] public float Speed = 1.0f;
    [Tooltip("데미지")] public int Damage = 1;
}

public partial class BulletBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        AddComponent(new BulletComponent()
        {
            Speed = authoring.Speed,
            Damage = authoring.Damage,
        });
    }
}