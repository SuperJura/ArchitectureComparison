using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Unity.Collections;
using System.Collections.Generic;
using Unity.Rendering;
using Unity.Jobs;
using UnityEngine.Jobs;

public class SpawnerSystem : ComponentSystem
{
    ComponentGroup group;
    static MeshInstanceRenderer enemyRenderer;
    static float3 endPosition;
    Unity.Mathematics.Random rng;

    protected override void OnUpdate()
    {
        if(enemyRenderer.mesh == null)
        {
            enemyRenderer = Bootstraper.getLookFromPrototype("Enemy");
            group = GetComponentGroup(Bootstraper.spawnerTypes);
            endPosition = GameObject.Find("SpaceStation").transform.position;
            rng = new Unity.Mathematics.Random(234567891);
        }

        var entities = group.GetEntityArray();
        var entityData = new NativeArray<Entity>(entities.Length, Allocator.Temp);
        for (int i = 0; i < entities.Length; i++)
        {
            entityData[i] = entities[i];
        }

        var spawners = group.GetComponentDataArray<Spawner>();
        var spawnerData = new NativeArray<Spawner>(spawners.Length, Allocator.Temp);
        for (int i = 0; i < spawners.Length; i++)
        {
            spawnerData[i] = spawners[i];
        }
        
        var locations = group.GetComponentDataArray<Position>();
        var locationsData = new NativeArray<Position>(locations.Length, Allocator.Temp);
        for (int i = 0; i < locations.Length; i++)
        {
            locationsData[i] = locations[i];
        }

        for (int i = 0; i < entityData.Length; i++)
        {
            if(spawnerData[i].spawnDelay > 0)
            {
                EntityManager.SetComponentData(entityData[i], new Spawner()
                {
                    spawnDelay = spawnerData[i].spawnDelay - Time.deltaTime
                });
            }
            else
            {
                var enemy = EntityManager.CreateEntity(Bootstraper.getArchetype(Bootstraper.enemyTypes));
                float3 spawnerPosition = new float3(locationsData[i].Value.x, locationsData[i].Value.y, locationsData[i].Value.z);
                float3 dir = spawnerPosition - endPosition;
                float3 farawayPosition = (spawnerPosition + dir) * 2;
                EntityManager.SetComponentData(enemy, new Position() 
                {
                    Value = farawayPosition
                });
                EntityManager.SetComponentData(enemy, new Rotation()
                {
                    Value = quaternion.LookRotationSafe(endPosition - spawnerPosition, math.up())
                });
                EntityManager.SetComponentData(enemy, new Enemy()
                {
                    speed = rng.NextFloat(0.1f, 0.5f),
                    targetLocation = endPosition,
                    spawnerLocation = spawnerPosition,
                    wentToSpawner = 0
                });
                EntityManager.SetSharedComponentData(enemy, enemyRenderer);
                EntityManager.SetComponentData(enemy, new Scale(){Value = new float3(15, 15, 15)});
                EntityManager.RemoveComponent(entityData[i], typeof(Spawner));
            }
        }
    }
}