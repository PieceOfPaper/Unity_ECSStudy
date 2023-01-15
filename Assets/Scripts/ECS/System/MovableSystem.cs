using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;

[UpdateAfter(typeof(EntitiesInputSystemGroup))]
public partial struct MovableSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        new ProcessMovableJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct ProcessMovableJob : IJobEntity
{
    public float deltaTime;

    private void Execute(ref LocalTransform transform, in MovableComponent movable)
    {
        if (movable.moveDir == Vector3.zero) return;

        transform.Rotation = Quaternion.Euler(0f, 90f - math.atan2(movable.moveDir.z, movable.moveDir.x) * Mathf.Rad2Deg, 0f);
        transform = transform.Translate(movable.moveDir * movable.moveSpeed * deltaTime);
    }
}