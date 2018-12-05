using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.UI;

public class Bootstraper
{
    public const int NUM_OF_ENEMIES = 25000;
    public static readonly float[] neededPercentForNextWeapon = new float[] {0.01f, 0.02f, 0.09f};

    public static ComponentType[] spawnerTypes;
    public static ComponentType[] enemyTypes;
    public static ComponentType[] playerTypes;
    public static ComponentType[] bulletTypes;
    public static ComponentType[] uiTypes;
    public static ComponentType[] beamTypes;

    static EntityManager manager;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void init()
    {
        manager = World.Active.GetOrCreateManager<EntityManager>();

        spawnerTypes = new ComponentType[] { typeof(Position), typeof(Spawner)};
        enemyTypes   = new ComponentType[] { typeof(MeshInstanceRenderer), typeof(Position), typeof(Rotation), typeof(Scale), typeof(VisibleLocalToWorld), typeof(Enemy)};
        playerTypes  = new ComponentType[] { typeof(MeshInstanceRenderer), typeof(Position), typeof(Rotation), typeof(Scale), typeof(VisibleLocalToWorld), typeof(Player), typeof(PlayerStats)};
        bulletTypes  = new ComponentType[] { typeof(MeshInstanceRenderer), typeof(Position), typeof(Rotation), typeof(Scale), typeof(VisibleLocalToWorld), typeof(Bullet)};
        beamTypes    = new ComponentType[] { typeof(MeshInstanceRenderer), typeof(Position), typeof(Rotation), typeof(Scale), typeof(VisibleLocalToWorld), typeof(Beam)};
        uiTypes      = new ComponentType[] { typeof(UI)};

        initPlayer();
        initEnemySpawners();
        initUI();
    }

    static void initPlayer()
    {
        var playerRenderer = Bootstraper.getLookFromPrototype("Player");
        
        var entity = manager.CreateEntity(Bootstraper.getArchetype(Bootstraper.playerTypes));
        manager.SetComponentData(entity, new Position(){Value = new float3(1020, 20, 20)});
        manager.SetComponentData(entity, new Scale(){Value = new float3(0.1f, 0.1f, 0.1f)});
        manager.SetComponentData(entity, new Rotation(){Value = quaternion.Euler(0, 180, 0)});
        manager.SetComponentData(entity, new PlayerStats()
        {
            speed = 0.5f,
            agility = 0.2f,
            currnetWeaponIndex = -1,
            enemiesDestroyed = 0,
            shootCooldown = 0
        });
        manager.SetSharedComponentData(entity, playerRenderer);

        GameObject cameraFollow = new GameObject("CameraFolow");
        cameraFollow.transform.position = new Vector3(1020, 20, 20);

        Cinemachine.CinemachineVirtualCamera camera = GameObject.Instantiate(Resources.Load<Cinemachine.CinemachineVirtualCamera>("Camera"));
        camera.Follow = cameraFollow.transform;
        camera.LookAt = cameraFollow.transform;
        
        manager.SetSharedComponentData(entity, new Player()
        {
            cameraPoint = cameraFollow,
            camera = camera
        });
    }

    static void initEnemySpawners()
    {
        float bigRadius = 6000;
        float smallRadius = 2500;
        Unity.Mathematics.Random rand = new Unity.Mathematics.Random(123456789);
        
        for (int i = 0; i < NUM_OF_ENEMIES; i++)
        {
            float delta = bigRadius - smallRadius;
            float length = delta * rand.NextFloat();
            Vector3 pos = UnityEngine.Random.onUnitSphere * (smallRadius + length);
            var spawner = manager.CreateEntity(getArchetype(spawnerTypes));
            manager.SetComponentData<Position>(spawner, new Position(){Value = new float3(pos.x, pos.y, pos.z)});
            manager.SetComponentData<Spawner>(spawner, new Spawner()
            {
                spawnDelay = i < NUM_OF_ENEMIES / 2 ? 0 : (5f + (i - NUM_OF_ENEMIES) / 100f),
            });
        }
    }
    
    static void initUI()
    {
        var ui = manager.CreateEntity(getArchetype(uiTypes));
        manager.SetSharedComponentData(ui, new UI()
        {
            bar = GameObject.Find("Canvas/ExpBar/Bar").GetComponent<Image>()
        });
    }

    public static MeshInstanceRenderer getLookFromPrototype(string protoName)
    {
        var proto = GameObject.Find(protoName);
        var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
        Object.Destroy(proto);
        return result;
    }

    public static EntityArchetype getArchetype(ComponentType[] types)
    {
        return manager.CreateArchetype(types);
    }
}
