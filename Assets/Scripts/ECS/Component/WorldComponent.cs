using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct WorldComponent : IComponentData
{
    public Entity playerPrefab;
    public bool isSpawnedPlayer;

    public Entity enemyPrefab;
    public int enemySpawnCount;
    public Vector3 enemySpawnRange;

    public int spawnedEnemyCount;
}