using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct MovableSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, movable) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MovableComponent>>())
        {
            Vector3 dir = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow)) dir += Vector3.forward;
            if (Input.GetKey(KeyCode.DownArrow)) dir += Vector3.back;
            if (Input.GetKey(KeyCode.LeftArrow)) dir += Vector3.left;
            if (Input.GetKey(KeyCode.RightArrow)) dir += Vector3.right;
            if (dir == Vector3.zero) continue;

            dir = dir.normalized;
            var transformValue = transform.ValueRW;
            transformValue.Rotation = Quaternion.Euler(0f, 90f - Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg, 0f);
            transformValue = transformValue.Translate(dir * movable.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime);
            transform.ValueRW = transformValue;
        }
    }
}