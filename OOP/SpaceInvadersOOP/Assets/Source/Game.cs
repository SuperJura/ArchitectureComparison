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

        // for (int i = 0; i < spawners.Length; i++)
        // {
        //     spawners[i].spawn();
        // }
    }

    void initEnemySpawners()
    {
        float bigRadius = 5000;
        float smallRadius = 400;
        ISpawnData easyEnemies = Resources.Load<EnemyDatabase>("EnemiesDatabases/Easy");
        int numOfSpawners = 100000;
        spawners = new ISpawner[numOfSpawners];
        for (int i = 0; i < numOfSpawners; i++)
        {
            float delta = bigRadius - smallRadius;
            float length = delta * Random.value;
            Vector3 pos = Random.onUnitSphere * (smallRadius + length);
            GameObject go = new GameObject("spawn " + i);
            ISpawner spawner = go.AddComponent<EnemySpawner>();
            spawner.init(easyEnemies);

            go.transform.position = pos;
            spawners[i] = spawner;
        }
    }
}
