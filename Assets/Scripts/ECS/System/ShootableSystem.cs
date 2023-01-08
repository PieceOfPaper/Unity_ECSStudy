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
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, shootable) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<ShootableComponent>>())
        {
            //Reloading
            if (shootable.ValueRO.BulletCount < shootable.ValueRO.BulletCountMax)
            {
                if (shootable.ValueRO.BulletReloadTime < shootable.ValueRO.BulletReloadTimeMax)
                {
                    shootable.ValueRW.BulletReloadTime += SystemAPI.Time.DeltaTime;
                }
                else
                {
                    shootable.ValueRW.BulletReloadTime -= shootable.ValueRO.BulletReloadTimeMax;
                    shootable.ValueRW.BulletCount++;
                    Debug.Log("On Reload " + shootable.ValueRO.BulletCount);

                    if (shootable.ValueRO.BulletCount >= shootable.ValueRO.BulletCountMax)
                    {
                        shootable.ValueRW.BulletReloadTime = 0f;
                    }
                }
            }

            //Fire
            if (m_OnKeyFire == true)
            {
                if (shootable.ValueRW.BulletCount > 0 &&
                    SystemAPI.Time.ElapsedTime >= shootable.ValueRO.BulletLastShotTime + shootable.ValueRO.BulletShotCooltime)
                {
                    shootable.ValueRW.BulletCount--;
                    shootable.ValueRW.BulletLastShotTime = (float)SystemAPI.Time.ElapsedTime;

                    Debug.Log("On Fire");
                }
                m_OnKeyFire = false;
            }
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