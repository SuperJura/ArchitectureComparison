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

    protected override void OnUpdate()
    {
        if(enemyRenderer.mesh == null)
        {
            enemyRenderer = Bootstraper.getLookFromPrototype("Enemy");
            group = GetComponentGroup(Bootstraper.spawnerArchtype.ComponentTypes);
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
                var enemy = EntityManager.CreateEntity(Bootstraper.enemyArchtype);
                EntityManager.SetComponentData(enemy, new Position() {Value = new float3(locationsData[i].Value.x, locationsData[i].Value.y, locationsData[i].Value.z)});
                EntityManager.SetSharedComponentData(enemy, enemyRenderer);
                EntityManager.SetComponentData(enemy, new Scale(){Value = new float3(10, 10, 10)});
                EntityManager.RemoveComponent(entityData[i], typeof(Spawner));
            }
        }
    }
}