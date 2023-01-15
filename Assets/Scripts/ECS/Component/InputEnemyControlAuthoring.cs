using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class InputEnemyControlAuthoring : MonoBehaviour
{
}

public class InputEnemyControlBaker : Baker<InputEnemyControlAuthoring>
{
    public override void Bake(InputEnemyControlAuthoring authoring)
    {
        AddComponent(new InputEnemyControlComponent()
        {
        });
    }
}

