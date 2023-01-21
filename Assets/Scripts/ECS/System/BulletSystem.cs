using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;

[UpdateAfter(typeof(MovableSystem))]
public partial struct BulletSystem : ISystem
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
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();

        foreach ((var bullet, var bulletTransform, var bulletHitLayer) in SystemAPI.Query<RefRW<BulletComponent>, RefRO<LocalTransform>, RefRO<HitLayerComponent>>())
        {
            foreach ((var character, var characterTransform, var characterHitLayer) in SystemAPI.Query<RefRW<CharacterComponent>, RefRO<LocalTransform>, RefRO<HitLayerComponent>>())
            {
                if (math.distancesq(new Vector2(characterTransform.ValueRO.Position.x, characterTransform.ValueRO.Position.z), new Vector2(bulletTransform.ValueRO.Position.x, bulletTransform.ValueRO.Position.z)) <= (characterHitLayer.ValueRO.hitboxRadius + bulletHitLayer.ValueRO.hitboxRadius) * (characterHitLayer.ValueRO.hitboxRadius + bulletHitLayer.ValueRO.hitboxRadius) &&
                    bulletHitLayer.ValueRO.attackLayerMask.HasFlag(characterHitLayer.ValueRO.hitLayer) == true)
                {
                    character.ValueRW.HP = math.max(character.ValueRO.HP - bullet.ValueRO.Damage, 0);
                    bullet.ValueRW.isDestroyed = true;
                }
            }
        }

        new ProcessBulletTimeUpdateJob()
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel();

        new ProcessDestroyBulletJob()
        {
            ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
        }.ScheduleParallel();

        new ProcessDestroyCharacterJob()
        {
            ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
        }.ScheduleParallel();

    }
}

[BurstCompile]
public partial struct ProcessBulletTimeUpdateJob : IJobEntity
{
    public float deltaTime;
    private void Execute(ref BulletComponent bullet)
    {
        if (bullet.isDestroyed == true)
            return;

        bullet.time += deltaTime;
        if (bullet.time >= bullet.Duration) bullet.isDestroyed = true;
    }
}

[BurstCompile]
public partial struct ProcessDestroyBulletJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;

    private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in BulletComponent bullet)
    {
        if (bullet.isDestroyed == true)
        {
            ecb.DestroyEntity(chunkIndex, entity);
        }
    }
}

[BurstCompile]
public partial struct ProcessDestroyCharacterJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;

    private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in CharacterComponent character)
    {
        if (character.HP == 0)
        {
            ecb.DestroyEntity(chunkIndex, entity);
        }
    }
}
