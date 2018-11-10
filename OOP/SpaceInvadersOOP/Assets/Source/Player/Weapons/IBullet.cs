using UnityEngine;

public interface IBullet
{
    void init(Vector3 position, Quaternion rotation);
    void move();
    void destroy();
    float getLifeTime();
    float getSpawnTime();
    IBullet copy();
    GameObject getObject();
}