using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class HitLayerAuthoring : MonoBehaviour
{
    public int attackLayerMask = 0;
    public HitLayerType hitLayer = 0;
    public float hitboxRadius = 1.0f;

#if UNITY_EDITOR
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(transform.localPosition, hitboxRadius);
    }

#endif
}

public class HitLayerBaker : Baker<HitLayerAuthoring>
{
    public override void Bake(HitLayerAuthoring authoring)
    {
        AddComponent(new HitLayerComponent()
        {
            attackLayerMask = (HitLayerType)authoring.attackLayerMask,
            hitLayer = authoring.hitLayer,
            hitboxRadius = authoring.hitboxRadius,
        });
    }
}