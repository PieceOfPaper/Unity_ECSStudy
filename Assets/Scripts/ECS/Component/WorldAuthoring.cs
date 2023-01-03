using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class WorldAuthoring : MonoBehaviour
{
    public GameObject playerPefab;
    public GameObject enemyPrefab;
}

public class WorldBaker : Baker<WorldAuthoring>
{
    public override void Bake(WorldAuthoring authoring)
    {
        AddComponent(new WorldComponent()
        {
            playerPrefab = GetEntity(authoring.playerPefab),
            enemyPrefab = GetEntity(authoring.enemyPrefab),
        });
    }
}
