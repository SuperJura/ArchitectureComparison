using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.UI;

public struct Spawner : IComponentData 
{
    public float spawnDelay;
    public byte spawned;
}

public struct Enemy : IComponentData
{
    public float3 targetLocation;
    public float3 spawnerLocation;
    public float speed;
    public byte wentToSpawner;
}

public struct Player : ISharedComponentData
{
    public UnityEngine.GameObject cameraPoint;
    public Cinemachine.CinemachineVirtualCamera camera;
}

public struct PlayerStats : IComponentData
{
    public float agility;
    public float speed;
    public float shootCooldown;
    public int enemiesDestroyed;
    public int currnetWeaponIndex;
}

public struct Bullet : IComponentData
{
    public float speed;
    public float timeToDestroy;
}

public struct Beam : IComponentData { }

public struct UI : ISharedComponentData
{
    public Image bar;
}