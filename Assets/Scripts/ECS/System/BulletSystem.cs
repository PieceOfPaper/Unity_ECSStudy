using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct BulletSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, bullet) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<BulletComponent>>())
        {
            var transformValue = transform.ValueRW;
            transformValue = transformValue.Translate(math.mul(transformValue.Rotation, Vector3.forward) * bullet.ValueRO.Speed * SystemAPI.Time.DeltaTime);
            transform.ValueRW = transformValue;
        }
    }
}
