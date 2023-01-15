using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

public partial struct WorldSystem : ISystem
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
        foreach (RefRW<WorldComponent> worldComponent in SystemAPI.Query<RefRW<WorldComponent>>())
        {
            if (worldComponent.ValueRO.isSpawnedEnemy == false)
            {
                if (worldComponent.ValueRO.enemyPrefab != Entity.Null)
                {
                    Entity newEntity = state.EntityManager.Instantiate(worldComponent.ValueRO.enemyPrefab);
                    state.EntityManager.SetComponentData(newEntity, new LocalTransform() { Position = new Vector3(1f, 0f, 0f), Scale = 1f, Rotation = Quaternion.Euler(0f, 180f, 0f) });
                }
                worldComponent.ValueRW.isSpawnedEnemy = true;
            }
            if (worldComponent.ValueRO.isSpawnedPlayer == false)
            {
                if (worldComponent.ValueRO.playerPrefab != Entity.Null)
                {
                    Entity newEntity = state.EntityManager.Instantiate(worldComponent.ValueRO.playerPrefab);
                    state.EntityManager.SetComponentData(newEntity, new LocalTransform() { Position = new Vector3(-1f, 0f, 0f), Scale = 1f, Rotation = Quaternion.Euler(0f, 180f, 0f) });
                }
                worldComponent.ValueRW.isSpawnedPlayer = true;
            }
        }
    }
}
