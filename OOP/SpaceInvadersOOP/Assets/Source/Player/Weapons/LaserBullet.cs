using UnityEngine;

public class LaserBullet : IBullet
{
    static GameObject laserBulletTemplate;

    GameObject gameObject;
    float spawnTime = -1;
    float cooldown;

    public void init(Vector3 position, Quaternion rotation)
    {
        if(laserBulletTemplate == null)
        {
            laserBulletTemplate = Resources.Load<GameObject>("Bullet");
        }

        gameObject = GameObject.Instantiate(laserBulletTemplate, position, rotation);
        spawnTime = Time.time;
    }

    public void destroy()
    {
        GameObject.Destroy(gameObject);
    }

    public float getLifeTime()
    {
        return 5;
    }

    public float getSpawnTime()
    {
        return spawnTime;
    }

    public void move()
    {
        gameObject.transform.position += gameObject.transform.forward * Time.deltaTime * 1000;
    }

    public IBullet copy()
    {
        return (IBullet)this.MemberwiseClone();
    }

    public GameObject getObject()
    {
        return gameObject;
    }
}