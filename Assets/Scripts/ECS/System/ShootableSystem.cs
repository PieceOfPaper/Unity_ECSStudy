using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public partial struct ShootableSystem : ISystem, MainInputAction.IPlayerActions
{
    static bool m_OnKeyFire = false;

    public void OnCreate(ref SystemState state)
    {
        var mainInputAction = new MainInputAction();
        mainInputAction.Enable();
        mainInputAction.Player.SetCallbacks(this);
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

        if (m_OnKeyFire == true)
        {
            m_OnKeyFire = false;

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            new ProcessShootbleFireJob()
            {
                ecb = ecb.AsParallelWriter(),
                elapsedTime = SystemAPI.Time.ElapsedTime,
            }.ScheduleParallel();
        }
    }


    public void OnFire(InputAction.CallbackContext context)
    {
        m_OnKeyFire = true;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnMove(InputAction.CallbackContext context)
    {
    }
}

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
            Debug.Log("On Reload " + shootable.BulletCount);

            if (shootable.BulletCount >= shootable.BulletCountMax)
            {
                shootable.BulletReloadTime = 0f;
            }
        }
    }
}

public partial struct ProcessShootbleFireJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;
    public double elapsedTime;

    private void Execute([ChunkIndexInQuery] int chunkIndex, in LocalTransform transform, ref ShootableComponent shootable)
    {
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
        // ecb.SetComponent(chunkIndex, bulletEntity, new BulletComponent()
        // {
        //     Owner = entity,
        //     Speed = 1.0f,
        //     Damage = 100,
        // });

        // var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        // var bullet = entityManager.GetComponentData<BulletComponent>(bulletEntity);
        // entityManager.GetCreatedAndDestroyedEntitiesAsync shootable
    }
}