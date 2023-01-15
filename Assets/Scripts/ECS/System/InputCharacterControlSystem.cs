using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Entities;
using Unity.Burst;

public class EntitiesInputSystemGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(EntitiesInputSystemGroup))]
public partial struct InputCharacterControlSystem : ISystem, MainInputAction.IPlayerActions
{
    static MainInputAction mainInputAction = null;
    static bool onKeyFire = false;
    static Vector2 moveDir = Vector2.zero;

    public void OnCreate(ref SystemState state)
    {
        mainInputAction = new MainInputAction();
        mainInputAction.Enable();
        mainInputAction.Player.SetCallbacks(this);
    }

    public void OnDestroy(ref SystemState state)
    {
        onKeyFire = false;
        moveDir = Vector2.zero;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((var movable, var inputCharCtrl) in SystemAPI.Query<RefRW<MovableComponent>, RefRO<InputCharacterControlComponent>>())
        {
            movable.ValueRW.moveDir = new Vector3(moveDir.x, 0f, moveDir.y);
        }
        foreach ((var shootable, var inputCharCtrl) in SystemAPI.Query<RefRW<ShootableComponent>, RefRO<InputCharacterControlComponent>>())
        {
            if (onKeyFire == true)
                shootable.ValueRW.OnKeyFire = true;
        }
        onKeyFire = false;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        onKeyFire = context.ReadValueAsButton();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }
}
