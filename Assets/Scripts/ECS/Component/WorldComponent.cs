using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct WorldComponent : IComponentData
{
    public Entity playerPrefab;
    public Entity enemyPrefab;

    public bool isSpawnedPlayer;
    public bool isSpawnedEnemy;
}