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
            if (worldComponent.ValueRO.isSpawnedPlayer == false)
            {
                if (worldComponent.ValueRO.playerPrefab != Entity.Null)
                {
                    Entity newEntity = state.EntityManager.Instantiate(worldComponent.ValueRO.playerPrefab);
                    state.EntityManager.SetComponentData(newEntity, new LocalTransform() { Position = new Vector3(0f, 0f, 0f), Scale = 1f, Rotation = Quaternion.Euler(0f, 180f, 0f) });
                }
                worldComponent.ValueRW.isSpawnedPlayer = true;
            }

            while (worldComponent.ValueRO.spawnedEnemyCount < worldComponent.ValueRO.enemySpawnCount)
            {
                if (worldComponent.ValueRO.enemyPrefab != Entity.Null)
                {
                    Entity newEntity = state.EntityManager.Instantiate(worldComponent.ValueRO.enemyPrefab);
                    var randPos = new Vector3(
                        UnityEngine.Random.Range(-1.0f, 1.0f) * worldComponent.ValueRO.enemySpawnRange.x * 0.5f,
                        UnityEngine.Random.Range(-1.0f, 1.0f) * worldComponent.ValueRO.enemySpawnRange.y * 0.5f,
                        UnityEngine.Random.Range(-1.0f, 1.0f) * worldComponent.ValueRO.enemySpawnRange.z * 0.5f
                    );
                    state.EntityManager.SetComponentData(newEntity, new LocalTransform() { Position = randPos, Scale = 1f, Rotation = Quaternion.Euler(0f, 180f, 0f) });

                    var inputEnemyCtrl = state.EntityManager.GetComponentData<InputEnemyControlComponent>(newEntity);
                    inputEnemyCtrl.random = Unity.Mathematics.Random.CreateFromIndex((uint)((System.DateTime.UtcNow.Ticks + worldComponent.ValueRO.spawnedEnemyCount) % uint.MaxValue));
                    state.EntityManager.SetComponentData(newEntity, inputEnemyCtrl);
                }
                worldComponent.ValueRW.spawnedEnemyCount++;
            }
        }
    }
}
