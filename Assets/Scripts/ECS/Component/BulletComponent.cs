using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public partial struct BulletComponent : IComponentData
{
    public int Damage;
    public float Duration;


    public float time;
    public bool isDestroyed;
}
