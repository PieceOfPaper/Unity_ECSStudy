using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CameraTargetAuthoring : MonoBehaviour
{
}

public class CameraTargetBaker : Baker<CameraTargetAuthoring>
{
    public override void Bake(CameraTargetAuthoring authoring)
    {
        AddComponent(new CameraTargetComponent()
        {
        });
    }
}