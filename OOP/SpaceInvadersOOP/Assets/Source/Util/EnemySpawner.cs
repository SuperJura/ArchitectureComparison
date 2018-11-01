using UnityEngine;

public class EnemySpawner : MonoBehaviour, ISpawner
{
    float timeToSpawn = 0;
    float speed;
    ISpawnData data;

    bool wentToSpawner = false;
    GameObject enemy;

    public void init(ISpawnData data, float spawnTimeOffset)
    {
        this.data = data;
        speed = Random.Range(5f, 15f);
        timeToSpawn = spawnTimeOffset;
    }

    public void update()
    {
        if(timeToSpawn <= 0 && enemy == null)
        {
            enemy = Instantiate(data.getRandomObject());
            enemy.transform.SetParent(transform);
            Vector3 dir = transform.position - StationManager.getInstance().transform.position;
            enemy.transform.position = (transform.transform.position + dir) * 2;
            enemy.transform.LookAt(StationManager.getInstance().transform.position);
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