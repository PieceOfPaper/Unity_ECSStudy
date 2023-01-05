using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CharacterAuthoring : MonoBehaviour
{
    [Tooltip("체력")] public int HP = 100;
}

public class CharacterBaker : Baker<CharacterAuthoring>
{
    public override void Bake(CharacterAuthoring authoring)
    {
        AddComponent(new CharacterComponent()
        {
            HP = authoring.HP,
            MaxHP = authoring.HP,
        });
    }
}