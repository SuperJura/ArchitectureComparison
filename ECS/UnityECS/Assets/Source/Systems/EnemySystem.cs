using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class EnemySystem : ComponentSystem
{
    ComponentGroup group;
    ComponentGroup bulletsGroup;
    ComponentGroup beamGroup;
    ComponentGroup playerGroup;

    protected override void OnUpdate()
    {
        if(group == null)
        {
            group = GetComponentGroup(Bootstraper.enemyTypes);   
            bulletsGroup = GetComponentGroup(Bootstraper.bulletTypes);
            beamGroup = GetComponentGroup(Bootstraper.beamTypes);
            playerGroup = GetComponentGroup(Bootstraper.playerTypes);
        }
        var enemyEntities = group.GetEntityArray();
        var enemyPositions = group.GetComponentDataArray<Position>();
        var enemyRotations = group.GetComponentDataArray<Rotation>();
        var enemyStats = group.GetComponentDataArray<Enemy>();
        var playerEntity = playerGroup.GetEntityArray()[0];
        var playerStats = playerGroup.GetComponentDataArray<PlayerStats>()[0];

        var bulletPositions = bulletsGroup.GetComponentDataArray<Position>();

        NativeList<Entity> toDestroy = new NativeList<Entity>(Allocator.Temp);
        for (int i = 0; i < enemyEntities.Length; i++)
        {
            bool wentToSpawner = enemyStats[i].wentToSpawner == 1;
            if(!wentToSpawner)
            {
                float spaceshipTargetDistance = math.distance(enemyPositions[i].Value, enemyStats[i].targetLocation);
                float spawnTargetDistance = math.distance(enemyStats[i].spawnerLocation, enemyStats[i].targetLocation);
                if(spaceshipTargetDistance < spawnTargetDistance)
                {
                    Enemy stats = enemyStats[i];
                    stats.wentToSpawner = 1;
                    EntityManager.SetComponentData(enemyEntities[i], stats);
                }
            }

            float speed = wentToSpawner ? enemyStats[i].speed : enemyStats[i].speed * 1000;
            enemyPositions[i] = new Position()
            {
                Value = enemyPositions[i].Value + (math.forward(enemyRotations[i].Value) * speed)
            };
            EntityManager.SetComponentData(enemyEntities[i], enemyPositions[i]);

            for (int j = 0; j < bulletPositions.Length; j++)
            {
                float3 a = enemyPositions[i].Value;
                float3 b =  bulletPositions[j].Value;
                float x = a.x > b.x ? (a.x - b.x) : (b.x - a.x);
                float y = a.y > b.y ? (a.y - b.y) : (b.y - a.y);
                float z = a.z > b.z ? (a.z - b.z) : (b.z - a.z);
                float distance = x + y + z;
                if(distance < 200)
                {
                    toDestroy.Add(enemyEntities[i]);
                    playerStats.enemiesDestroyed++;
                }
            }
        }
        if(toDestroy.Length > 0)
        {
            EntityManager.DestroyEntity(toDestroy);
        }
        toDestroy.Dispose();
        EntityManager.SetComponentData(playerEntity, playerStats);
    }
}