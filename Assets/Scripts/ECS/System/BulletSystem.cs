using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public partial struct BulletSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        new ProcessBulletMoveJob()
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel();

        // new ProcessBulletTriggerEventJob
        // {
        // }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }
}

public partial struct ProcessBulletMoveJob : IJobEntity
{
    public float deltaTime;

    private void Execute(ref LocalTransform transform, in BulletComponent bullet)
    {
        transform = transform.Translate(math.mul(transform.Rotation, Vector3.forward) * bullet.Speed * deltaTime);
    }
}

// public partial struct ProcessBulletTriggerEventJob : ITriggerEventsJob
// {
//     public void Execute(TriggerEvent collisionEvent)
//     {
//         var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
//         var layerA = entityManager.GetComponentData<HitLayerComponent>(collisionEvent.EntityA);
//         var layerB = entityManager.GetComponentData<HitLayerComponent>(collisionEvent.EntityB);
//         if (layerB.attackLayerMask.HasFlag(layerA.hitLayer) == true &&
//             entityManager.HasComponent<BulletComponent>(collisionEvent.EntityB) == true &&
//             entityManager.HasComponent<CharacterComponent>(collisionEvent.EntityA) == true)
//         {
//             var bullet = entityManager.GetComponentData<BulletComponent>(collisionEvent.EntityB);
//             var character = entityManager.GetComponentData<CharacterComponent>(collisionEvent.EntityA);
//             character.HP = math.max(character.HP - bullet.Damage, 0);
//             entityManager.SetComponentData(collisionEvent.EntityA, character);
//             entityManager.DestroyEntity(collisionEvent.EntityB);
//         }
//     }
// }
