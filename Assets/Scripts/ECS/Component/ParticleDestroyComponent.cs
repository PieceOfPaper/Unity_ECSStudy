using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public partial struct ParticleDestroyComponent : IComponentData
{
    public float DestroyTimer;
    public bool DestroyOnPlayEnd;


    public float timer;
}
