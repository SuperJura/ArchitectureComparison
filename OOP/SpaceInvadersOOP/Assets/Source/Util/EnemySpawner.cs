using UnityEngine;

public class EnemySpawner : MonoBehaviour, ISpawner
{
    float timeToSpawn = 0;
    ISpawnData data;
    public void init(ISpawnData data)
    {
        this.data = data;
    }

    public void spawn()
    {
        if(timeToSpawn == 0)
        {
            //Instantiate(data.getRandomObject());
            timeToSpawn = Random.Range(5, 10);
        }
        timeToSpawn -= Time.deltaTime;
    }

    public void Update()
    {
        if(timeToSpawn == 0)
        {
            //Instantiate(data.getRandomObject());
            timeToSpawn = Random.Range(5, 10);
        }
        timeToSpawn -= Time.deltaTime;
    }
}