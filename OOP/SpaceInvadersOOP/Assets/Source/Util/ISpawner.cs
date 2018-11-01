using UnityEngine;

public interface ISpawner
{
    void init(ISpawnData data);
    void spawn();
}