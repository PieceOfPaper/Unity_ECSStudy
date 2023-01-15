using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using Unity.Burst;

public partial struct MovableSystem : ISystem, MainInputAction.IPlayerActions
{
    static Vector2 moveDir;

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
        if (moveDir == Vector2.zero) return;

        new ProcessMovableJob
        {
            dir = new Vector3(moveDir.x, 0f, moveDir.y),
            deltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel();
    }


    public void OnFire(InputAction.CallbackContext context)
    {
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }
}

[BurstCompile]
public partial struct ProcessMovableJob : IJobEntity
{
    public Vector3 dir;
    public float deltaTime;

    private void Execute(ref LocalTransform transform, in MovableComponent movable)
    {
        transform.Rotation = Quaternion.Euler(0f, 90f - Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg, 0f);
        transform = transform.Translate(dir * movable.moveSpeed * deltaTime);
    }
}