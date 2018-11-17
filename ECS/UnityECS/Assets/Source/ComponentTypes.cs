using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct Spawner : IComponentData 
{
    public float spawnDelay;
    public byte spawned;
}

public struct Enemy : IComponentData
{
    public float3 targetLocation;
    public float speed;
}

public struct Player : ISharedComponentData
{
    public UnityEngine.GameObject cameraPoint;
}

public struct PlayerStats : IComponentData
{
    public float agility;
    public float speed;
}