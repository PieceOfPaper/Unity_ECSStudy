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

        var characterQuery = state.EntityManager.CreateEntityQuery(typeof(CharacterComponent), typeof(LocalTransform), typeof(HitLayerComponent));
        var characterEntities = characterQuery.ToEntityArray(Allocator.Temp);
        var characters = characterQuery.ToComponentDataArray<CharacterComponent>(Allocator.Temp);
        var characterTransforms = characterQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
        var characterHitLayers = characterQuery.ToComponentDataArray<HitLayerComponent>(Allocator.Temp);
        foreach ((var bullet, var bulletTransform, var bulletHitLayer) in SystemAPI.Query<RefRW<BulletComponent>, RefRO<LocalTransform>, RefRO<HitLayerComponent>>())
        {
            for (int i = 0; i < characterEntities.Length; i ++)
            {
                if (math.distancesq(new Vector2(characterTransforms[i].Position.x, characterTransforms[i].Position.z), new Vector2(bulletTransform.ValueRO.Position.x, bulletTransform.ValueRO.Position.z)) <= (characterHitLayers[i].hitboxRadius + bulletHitLayer.ValueRO.hitboxRadius) * (characterHitLayers[i].hitboxRadius + bulletHitLayer.ValueRO.hitboxRadius) &&
                    bulletHitLayer.ValueRO.attackLayerMask.HasFlag(characterHitLayers[i].hitLayer) == true)
                {
                    var character = characters[i];
                    character.HP = math.max(character.HP - bullet.ValueRO.Damage, 0);
                    state.EntityManager.SetComponentData(characterEntities[i], character);

                    bullet.ValueRW.isDestroyed = true;
                }
            }
        }
        characterEntities.Dispose();
        characters.Dispose();
        characterTransforms.Dispose();
        characterHitLayers.Dispose();
        characterQuery.Dispose();

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
