using UnityEngine;

public interface ISpawner
{
    void init(ISpawnData data, float spawnTimeOffset);
    void destroy();
    void update();
}