using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

[UpdateAfter(typeof(EntitiesInputSystemGroup))]
public partial struct ShootableSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        new ProcessShootbleReloadJob()
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel();

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        new ProcessShootbleFireJob()
        {
            ecb = ecb.AsParallelWriter(),
            elapsedTime = SystemAPI.Time.ElapsedTime,
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct ProcessShootbleReloadJob : IJobEntity
{
    public float deltaTime;

    private void Execute(ref ShootableComponent shootable)
    {
        if (shootable.BulletCount >= shootable.BulletCountMax)
            return;
            
        if (shootable.BulletReloadTime < shootable.BulletReloadTimeMax)
        {
            shootable.BulletReloadTime += deltaTime;
        }
        else
        {
            shootable.BulletReloadTime -= shootable.BulletReloadTimeMax;
            shootable.BulletCount++;
            // Debug.Log("On Reload " + shootable.BulletCount);

            if (shootable.BulletCount >= shootable.BulletCountMax)
            {
                shootable.BulletReloadTime = 0f;
            }
        }
    }
}

[BurstCompile]
public partial struct ProcessShootbleFireJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;
    public double elapsedTime;

    private void Execute([ChunkIndexInQuery] int chunkIndex, in LocalTransform transform, ref ShootableComponent shootable)
    {
        if (shootable.OnKeyFire == false)
            return;

        shootable.OnKeyFire = false;
        if (shootable.BulletCount <= 0 || elapsedTime < shootable.BulletLastShotTime + shootable.BulletShotCooltime)
            return;

        shootable.BulletCount--;
        shootable.BulletLastShotTime = (float)elapsedTime;

        if (shootable.BulletPrefab == Entity.Null)
            return;

        var bulletEntity = ecb.Instantiate(chunkIndex, shootable.BulletPrefab);
        var bulletTransform = new LocalTransform() { Position = transform.Position, Scale = 1f, Rotation = transform.Rotation };
        bulletTransform.Position += math.mul(transform.Rotation, shootable.ShootOffset);
        ecb.SetComponent(chunkIndex, bulletEntity, bulletTransform);
        ecb.SetComponent(chunkIndex, bulletEntity, new MovableComponent() { moveSpeed = 2.0f, moveDir = math.mul(transform.Rotation, Vector3.forward) });
    }
}