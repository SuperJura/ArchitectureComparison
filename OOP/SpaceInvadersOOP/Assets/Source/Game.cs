using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    IPlayer player;
    IInput device;

    ISpawner[] spawners;

    void Start()
    {
        player = new Singleplayer();
        device = new PCInputDevice();

        player.init(GameObject.Find("Player"), device);
        initEnemySpawners();
    }

    // Update is called once per frame
    void Update()
    {
        device.updateDevice();
        player.updatePlayer();

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].update();
        }
    }

    void initEnemySpawners()
    {
        float bigRadius = 4000;
        float smallRadius = 2000;
        ISpawnData easyEnemies = Resources.Load<EnemyDatabase>("EnemiesDatabases/Easy");
        int numOfSpawners = 5000;
        spawners = new ISpawner[numOfSpawners];
        for (int i = 0; i < numOfSpawners; i++)
        {
            float delta = bigRadius - smallRadius;
            float length = delta * Random.value;
            Vector3 pos = Random.onUnitSphere * (smallRadius + length);
            GameObject go = new GameObject("spawn " + i);
            ISpawner spawner = go.AddComponent<EnemySpawner>();
            spawner.init(easyEnemies, i < 2000 ? 0 : (5f + (i - 2000) / 100f));

            go.transform.position = pos;
            spawners[i] = spawner;
        }
    }
}
