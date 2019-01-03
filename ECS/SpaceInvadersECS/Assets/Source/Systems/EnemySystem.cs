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
        var playerRotation = playerGroup.GetComponentDataArray<Rotation>()[0];
        var playerPosition = playerGroup.GetComponentDataArray<Position>()[0];
        var beamArray = beamGroup.GetEntityArray();
        var beamStats = beamGroup.GetComponentDataArray<Beam>();

        var bulletPositions = bulletsGroup.GetComponentDataArray<Position>();

        NativeList<Entity> toDestroy = new NativeList<Entity>(Allocator.Temp);

        bool justDestroy = false;
        if(beamStats.Length == 1 && playerStats.currnetWeaponIndex == 2)
        {
            var stat = beamStats[0];
            if(math.clamp(stat.timeSinceStarted, 0, stat.maxScale) >= stat.maxScale)
            {
                PerformanceTest.finish();
                justDestroy = true;
            }
        }
        for (int i = 0; i < enemyEntities.Length; i++)
        {
            if(justDestroy)
            {
                toDestroy.Add(enemyEntities[i]);
                playerStats.enemiesDestroyed++;
                continue;
            }
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



            float3 dir = math.normalize(enemyPositions[i].Value - playerPosition.Value);
            float dot = math.dot(math.forward(playerRotation.Value), dir);
            if(dot >= 0.997)
            {
                if(beamArray.Length > 0)
                {
                    toDestroy.Add(enemyEntities[i]);
                    playerStats.enemiesDestroyed++;
                }
            }
            else if(dot >= 0.6)
            {
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
                        bool add = true;
                        for (int k = 0; k < toDestroy.Length; k++)
                        {
                            if(toDestroy[k].Index == enemyEntities[i].Index) add = false;
                        }
                        if(add)
                        {
                            toDestroy.Add(enemyEntities[i]);
                            playerStats.enemiesDestroyed++;
                        }
                    }
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