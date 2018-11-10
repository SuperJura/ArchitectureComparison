using UnityEngine;

public class BeamBullet : IBullet
{
    static GameObject beamTemplate;

    GameObject gameObject;
    float spawnTime;
    
    public void init(Vector3 position, Quaternion rotation)
    {
        if(beamTemplate == null)
        {
            beamTemplate = Resources.Load<GameObject>("Beam");
        }
        gameObject = GameObject.Instantiate(beamTemplate, position, rotation);
        spawnTime = Time.time;
    }

    public IBullet copy()
    {
        return (IBullet)this.MemberwiseClone();
    }

    public void destroy()
    {
        GameObject.Destroy(gameObject);
    }

    public float getLifeTime()
    {
        return 20;
    }

    public float getSpawnTime()
    {
        return spawnTime;
    }

    public void move()
    {
        return;
    }

    public GameObject getObject()
    {
        return gameObject;
    }
}