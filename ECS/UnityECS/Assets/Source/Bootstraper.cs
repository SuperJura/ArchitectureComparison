using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

public class Bootstraper
{
    public const int NUM_OF_ENEMIES = 50000;

    public static EntityArchetype spawnerArchtype;
    public static EntityArchetype enemyArchtype;
    public static EntityArchetype playerArchtype;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void init()
    {
        var manager = World.Active.GetOrCreateManager<EntityManager>();

        spawnerArchtype = manager.CreateArchetype(typeof(Position), typeof(Spawner));
        enemyArchtype   = manager.CreateArchetype(typeof(MeshInstanceRenderer), typeof(Position), typeof(Rotation), typeof(Scale), typeof(VisibleLocalToWorld), typeof(Enemy));
        playerArchtype  = manager.CreateArchetype(typeof(MeshInstanceRenderer), typeof(Position), typeof(Rotation), typeof(Scale), typeof(VisibleLocalToWorld), typeof(Player), typeof(PlayerStats));

        initEnemySpawners(manager);
    }
        
    static void initEnemySpawners(EntityManager manager)
    {
        float bigRadius = 4000;
        float smallRadius = 2000;
        Unity.Mathematics.Random rand = new Unity.Mathematics.Random(123456789);
        
        for (int i = 0; i < NUM_OF_ENEMIES; i++)
        {
            float delta = bigRadius - smallRadius;
            float length = delta * rand.NextFloat();
            Vector3 pos = UnityEngine.Random.onUnitSphere * (smallRadius + length);
            var spawner = manager.CreateEntity(spawnerArchtype);
            manager.SetComponentData<Position>(spawner, new Position(){Value = new float3(pos.x, pos.y, pos.z)});
            manager.SetComponentData<Spawner>(spawner, new Spawner()
            {
                spawnDelay = i < NUM_OF_ENEMIES / 2 ? 0 : (5f + (i - NUM_OF_ENEMIES) / 100f),
            });
        }
    }
    
    public static MeshInstanceRenderer getLookFromPrototype(string protoName)
    {
        var proto = GameObject.Find(protoName);
        Debug.Log(proto + " " + protoName);
        var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
        Object.Destroy(proto);
        return result;
    }
}
