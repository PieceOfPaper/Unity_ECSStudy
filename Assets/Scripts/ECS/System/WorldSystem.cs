using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct WorldSystem : ISystem
{   
    public void OnCreate(ref SystemState state)
    {
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRW<WorldComponent> worldComponent in SystemAPI.Query<RefRW<WorldComponent>>())
        {
            // if (worldComponent.ValueRO.isSpawnedEnemy == false)
            // {
            //     Entity newEntity = state.EntityManager.Instantiate(worldComponent.ValueRO.enemyPrefab);
            //     state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(float3.zero));
            //     worldComponent.ValueRW.isSpawnedEnemy = true;
            // }
            if (worldComponent.ValueRO.isSpawnedPlayer == false)
            {
                Entity newEntity = state.EntityManager.Instantiate(worldComponent.ValueRO.playerPrefab);
                state.EntityManager.SetComponentData(newEntity, new LocalTransform(){ Position = Vector3.zero, Scale = 1f, Rotation = Quaternion.Euler(0f, 180f, 0f) });
                worldComponent.ValueRW.isSpawnedPlayer = true;
            }
        }
    }
}
