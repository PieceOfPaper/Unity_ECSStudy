using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;

[UpdateInGroup(typeof(EntitiesInputSystemGroup))]
public partial struct InputEnemyControlSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new InputEnemyControlMovableJob()
        {
            elapsedTime = SystemAPI.Time.ElapsedTime,
        }.ScheduleParallel();

        new InputEnemyControlShootableJob()
        {
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct InputEnemyControlMovableJob : IJobEntity
{
    public double elapsedTime;
    [Unity.Collections.LowLevel.Unsafe.NativeSetThreadIndex] private int threadIndex;

    private void Execute([ChunkIndexInQuery] int chunkIndex, ref MovableComponent movable, ref InputEnemyControlComponent inputEnemyCtrl)
    {
        if (inputEnemyCtrl.nextMovableUpdateTime > elapsedTime)
            return;

        var random = inputEnemyCtrl.random;
        movable.moveDir = Quaternion.Euler(0f, random.NextFloat(360f), 0f) * Vector3.forward;
        inputEnemyCtrl.nextMovableUpdateTime = elapsedTime +  random.NextFloat(0.5f, 2.0f);
        inputEnemyCtrl.random = random;
    }
}

[BurstCompile]
public partial struct InputEnemyControlShootableJob : IJobEntity
{
    public int randSeed;
    public double elapsedTime;

    private void Execute([ChunkIndexInQuery] int chunkIndex, ref ShootableComponent shootable, ref InputEnemyControlComponent inputEnemyCtrl)
    {
        //TODO
    }
}
