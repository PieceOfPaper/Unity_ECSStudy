using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ParticleDestroyAuthoring : MonoBehaviour
{
    public float DestroyTimer = 0f;
    public bool DestroyOnPlayEnd = true;
}

public class ParticleDestroyBaker : Baker<ParticleDestroyAuthoring>
{
    public override void Bake(ParticleDestroyAuthoring authoring)
    {
        AddComponent(new ParticleDestroyComponent()
        {
            DestroyTimer = authoring.DestroyTimer,
            DestroyOnPlayEnd = authoring.DestroyOnPlayEnd,
        });
    }
}
