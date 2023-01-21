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
            elapsedTime = SystemAPI.Time.ElapsedTime,
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct InputEnemyControlMovableJob : IJobEntity
{
    public double elapsedTime;

    private void Execute(ref MovableComponent movable, ref InputEnemyControlComponent inputEnemyCtrl)
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
    public double elapsedTime;

    private void Execute(ref ShootableComponent shootable, ref InputEnemyControlComponent inputEnemyCtrl)
    {
        if (inputEnemyCtrl.nextShootableUpdateTime == 0f)
        {
            var random = inputEnemyCtrl.random;
            inputEnemyCtrl.nextShootableUpdateTime = elapsedTime + shootable.BulletShotCooltime + random.NextFloat(-1.0f, 1.5f);
            inputEnemyCtrl.random = random;
        }
        else
        {
            if (inputEnemyCtrl.nextShootableUpdateTime > elapsedTime)
                return;

            if (shootable.OnKeyFire == true)
                return;

            var random = inputEnemyCtrl.random;
            shootable.OnKeyFire = true;
            inputEnemyCtrl.nextShootableUpdateTime = elapsedTime + shootable.BulletShotCooltime + random.NextFloat(-1.0f, 1.5f);
            inputEnemyCtrl.random = random;
        }
    }
}
