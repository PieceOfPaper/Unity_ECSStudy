using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public partial struct BulletComponent : IComponentData
{
    public float Speed;
    public int Damage;
}
