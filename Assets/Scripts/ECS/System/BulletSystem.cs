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
        new ProcessBulletMoveJob()
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel();
    }
}

public partial struct ProcessBulletMoveJob : IJobEntity
{
    public float deltaTime;

    private void Execute(ref LocalTransform transform, ref BulletComponent bullet)
    {
        transform = transform.Translate(math.mul(transform.Rotation, Vector3.forward) * bullet.Speed * deltaTime);
    }
}
