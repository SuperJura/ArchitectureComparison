using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class BulletSystem : ComponentSystem
{
    ComponentGroup group;

    protected override void OnUpdate()
    {
        if(group == null)
        {
            group = GetComponentGroup(Bootstraper.bulletTypes);
        }

        var entities = group.GetEntityArray();
        var positions = group.GetComponentDataArray<Position>();
        var rotations = group.GetComponentDataArray<Rotation>();
        var bulletStats = group.GetComponentDataArray<Bullet>();

        NativeList<Entity> toDestroy = new NativeList<Entity>(Allocator.TempJob);
        for (int i = 0; i < entities.Length; i++)
        {
            var newPosition = positions[i].Value + math.forward(rotations[i].Value) * bulletStats[i].speed;
            EntityManager.SetComponentData(entities[i], new Position()
            {
                Value = newPosition
            });
            var stats = bulletStats[i];
            stats.timeToDestroy -= UnityEngine.Time.deltaTime;
            EntityManager.SetComponentData(entities[i], stats);
            if(stats.timeToDestroy <= 0)
            {
                toDestroy.Add(entities[i]);
            }
        }
        EntityManager.DestroyEntity(toDestroy);
        toDestroy.Dispose();
    }
}