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
        if (characterQuery.IsEmpty == false)
        {
            var characterEntities = characterQuery.ToEntityArray(Allocator.Temp);
            var characters = characterQuery.ToComponentDataArray<CharacterComponent>(Allocator.Temp);
            var characterTransforms = characterQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
            var characterHitLayers = characterQuery.ToComponentDataArray<HitLayerComponent>(Allocator.Temp);
            foreach ((var bullet, var bulletTransform, var bulletHitLayer) in SystemAPI.Query<RefRW<BulletComponent>, RefRO<LocalTransform>, RefRO<HitLayerComponent>>())
            {
                for (int characterIndex = 0; characterIndex < characterEntities.Length; characterIndex++)
                {
                    var characterTransform = characterTransforms[characterIndex];
                    var characterHitLayer = characterHitLayers[characterIndex];
                    var checkDistance = math.distancesq(new float2(characterTransform.Position.x, characterTransform.Position.z), new float2(bulletTransform.ValueRO.Position.x, bulletTransform.ValueRO.Position.z)) <= (characterHitLayer.hitboxRadius + bulletHitLayer.ValueRO.hitboxRadius) * (characterHitLayer.hitboxRadius + bulletHitLayer.ValueRO.hitboxRadius);
                    var checkLayer = (bulletHitLayer.ValueRO.attackLayerMask & characterHitLayer.hitLayer) > 0;
                    if (checkDistance && checkLayer)
                    {
                        var character = characters[characterIndex];
                        character.HP = math.max(character.HP - bullet.ValueRO.Damage, 0);
                        state.EntityManager.SetComponentData(characterEntities[characterIndex], character);
                        bullet.ValueRW.isDestroyed = true;
                    }
                }
            }
            characterEntities.Dispose();
            characters.Dispose();
            characterTransforms.Dispose();
            characterHitLayers.Dispose();
        }
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

    private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in BulletComponent bullet, in LocalTransform transform)
    {
        if (bullet.isDestroyed == true)
        {
            if (bullet.HitEffect != Entity.Null)
            {
                var hitEffectEntity = ecb.Instantiate(chunkIndex, bullet.HitEffect);
                ecb.SetComponent(chunkIndex, hitEffectEntity, transform);
            }
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
