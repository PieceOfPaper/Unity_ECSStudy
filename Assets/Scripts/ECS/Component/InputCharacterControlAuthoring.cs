using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class InputCharacterControlAuthoring : MonoBehaviour
{
}

public class InputCharacterControlBaker : Baker<InputCharacterControlAuthoring>
{
    public override void Bake(InputCharacterControlAuthoring authoring)
    {
        AddComponent<InputCharacterControlComponent>();
    }
}