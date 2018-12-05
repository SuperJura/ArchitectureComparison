using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public const int NUM_OF_ENEMIES = 11000;
    IPlayer player;
    IInput device;
    InterfaceManager uiManager;

    ISpawner[] spawners;

    public static Game instance;

    public int enemyDestroyed = 0;
    
    public readonly float[] neededPercentForNextWeapon = new float[] {0.01f, 0.2f, 0.9f};
    public int currnetWeaponIndex = -1;

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
        // Debug.Log(enemyDestroyed);
        uiManager.update();
    }

    void initEnemySpawners()
    {
        float bigRadius = 4000;
        float smallRadius = 2000;
        ISpawnData easyEnemies = Resources.Load<EnemyDatabase>("EnemiesDatabases/Easy");
        spawners = new ISpawner[NUM_OF_ENEMIES];
        for (int i = 0; i < NUM_OF_ENEMIES; i++)
        {
            float delta = bigRadius - smallRadius;
            float length = delta * Random.value;
            Vector3 pos = Random.onUnitSphere * (smallRadius + length);
            GameObject go = new GameObject("spawn " + i);
            ISpawner spawner = go.AddComponent<EnemySpawner>();
            spawner.init(easyEnemies, i < NUM_OF_ENEMIES / 2 ? 0 : (5f + (i - NUM_OF_ENEMIES) / 100f));

            go.transform.position = pos;
            spawners[i] = spawner;
        }
    }

    public void onEnemyDestroy()
    {
        enemyDestroyed++;
        float percent = enemyDestroyed / (float)NUM_OF_ENEMIES;
        int newWeaponIndex = currnetWeaponIndex;
        for (int i = 0; i < neededPercentForNextWeapon.Length; i++)
        {
            if(percent >= neededPercentForNextWeapon[i])
            {
                newWeaponIndex = i;
            }
        }
        if(newWeaponIndex != currnetWeaponIndex)
        {
            if(newWeaponIndex == 0)
            {
                player.changeWeapon(new Rifle(0.001f));
            }
            else if(newWeaponIndex == 1)
            {
                player.changeWeapon(new Beam());
            }
            else if(newWeaponIndex == 2)
            {
                player.changeWeapon(new Beam(50, () =>
                {
                    foreach(var spawner in spawners)
                    {
                        spawner.destroy();
                    }
                }));
            }

            currnetWeaponIndex = newWeaponIndex;
        }
    }
}