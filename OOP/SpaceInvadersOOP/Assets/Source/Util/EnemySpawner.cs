using UnityEngine;

public class EnemySpawner : MonoBehaviour, ISpawner
{
    float timeToSpawn = 0;
    float speed;
    ISpawnData data;

    bool wentToSpawner = false;
    GameObject enemy;
    bool spawned = false;

    public void init(ISpawnData data, float spawnTimeOffset)
    {
        this.data = data;
        speed = Random.Range(5f, 10f);
        timeToSpawn = spawnTimeOffset;
    }

    public void update()
    {
        if(timeToSpawn <= 0 && enemy == null && !spawned)
        {
            enemy = Instantiate(data.getRandomObject());
            enemy.transform.SetParent(transform);
            Vector3 dir = transform.position - StationManager.getInstance().transform.position;
            enemy.transform.position = (transform.transform.position + dir) * 2;
            enemy.transform.LookAt(StationManager.getInstance().transform.position);
            spawned = true;
        }
        timeToSpawn -= Time.deltaTime;
        if(enemy != null)
        {
            if(!wentToSpawner)
            {
                enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, transform.position, Time.deltaTime * speed * 300);
                if(Vector3.Distance(enemy.transform.position, transform.position) < 0.2f) wentToSpawner = true;
            }
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, StationManager.getInstance().transform.position, Time.deltaTime * speed);
        }
    }
}