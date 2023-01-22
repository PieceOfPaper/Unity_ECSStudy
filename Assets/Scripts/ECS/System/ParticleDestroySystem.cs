using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Jobs;

public partial struct ParticleDestroySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((var particle, var particleDestroy, var entity) in SystemAPI.Query<SystemAPI.ManagedAPI.UnityEngineComponent<UnityEngine.ParticleSystem>, RefRW<ParticleDestroyComponent>>().WithEntityAccess())
        {
            bool willDestroy = false;

            if (willDestroy == false &&
                particleDestroy.ValueRO.DestroyTimer > 0f)
            {
                particleDestroy.ValueRW.timer += SystemAPI.Time.DeltaTime;
                if (particleDestroy.ValueRO.timer >= particleDestroy.ValueRO.DestroyTimer)
                    willDestroy = true;
            }

            if (willDestroy == false && 
                particleDestroy.ValueRO.DestroyOnPlayEnd == true && 
                particle.Value.isPlaying == false)
                willDestroy = true;

            if (willDestroy == true)
            {
                // state.EntityManager.DestroyEntity(entity);
                ecb.DestroyEntity(entity);
            }
        }
    }
}