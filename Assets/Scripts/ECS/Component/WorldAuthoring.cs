using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class WorldAuthoring : MonoBehaviour
{
    public GameObject playerPefab;
    public GameObject enemyPrefab;
    public int enemySpawnCount = 1;
    public Vector3 enemySpawnRange = new Vector3(10f, 0f, 10f);
}

public class WorldBaker : Baker<WorldAuthoring>
{
    public override void Bake(WorldAuthoring authoring)
    {
        AddComponent(new WorldComponent()
        {
            playerPrefab = GetEntity(authoring.playerPefab),
            enemyPrefab = GetEntity(authoring.enemyPrefab),
            enemySpawnCount = authoring.enemySpawnCount,
            enemySpawnRange = authoring.enemySpawnRange,
    });
    }
}
