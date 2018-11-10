using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    IPlayer player;
    IInput device;
    InterfaceManager uiManager;

    ISpawner[] spawners;

    public static Game instance;

    public int enemyDestroyed = 0;
    public int needToDestroyForNextWeapon = 50; 

    void Start()
    {
        player = new Singleplayer();
        device = new PCInputDevice();
        uiManager = new InterfaceManager();

        player.init(GameObject.Find("Player"), device);
        uiManager.init();
        initEnemySpawners();
        instance = this;
    }

    void Update()
    {
        device.updateDevice();
        player.updatePlayer();

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].update();
        }
        Debug.Log(enemyDestroyed);
        uiManager.update();
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
            spawner.init(easyEnemies, i < numOfSpawners / 2 ? 0 : (5f + (i - numOfSpawners) / 100f));

            go.transform.position = pos;
            spawners[i] = spawner;
        }
    }

    public void onEnemyDestroy()
    {
        enemyDestroyed++;
        if(enemyDestroyed == needToDestroyForNextWeapon)
        {
            if(needToDestroyForNextWeapon == 50)
            {
                player.changeWeapon(new Rifle(0.001f));
            }
            needToDestroyForNextWeapon = 500;
        }
    }
}