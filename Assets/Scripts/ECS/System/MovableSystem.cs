using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public partial struct MovableSystem : ISystem, MainInputAction.IPlayerActions
{
    static Vector2 moveDir;
    static CinemachineVirtualCameraTarget vcamTarget;

    public void OnCreate(ref SystemState state)
    {
        var mainInputAction = new MainInputAction();
        mainInputAction.Enable();
        mainInputAction.Player.SetCallbacks(this);

        vcamTarget = GameObject.FindObjectOfType<CinemachineVirtualCameraTarget>();
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, movable) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MovableComponent>>())
        {
            Vector3 dir = new Vector3(moveDir.x, 0f, moveDir.y);
            if (dir == Vector3.zero) continue;

            dir = dir.normalized;
            var transformValue = transform.ValueRW;
            transformValue.Rotation = Quaternion.Euler(0f, 90f - Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg, 0f);
            transformValue = transformValue.Translate(dir * movable.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime);
            transform.ValueRW = transformValue;

            if (vcamTarget != null) vcamTarget.SetPosition(transformValue.Position);
        }
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