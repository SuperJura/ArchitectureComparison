using UnityEngine;

public class Game : MonoBehaviour 
{
    public const int NUM_OF_ENEMIES = 200;

    void Start()
    {
        EntityManager.init();
        initEnemyShips();
        initTargetShip();
        initPlayer();
        initSystems();
    }

    void Update()
    {
        EntityManager.update();
    }

    void initTargetShip()
    {
        GameObject target = GameObject.Find("Station");
        var entity = EntityManager.createNewEntity(new Component[]
        {
            new ComponentsTransform()
            {
                transform = target.transform
            },
            new ComponentTargetShip()
        });
    }

    void initEnemyShips()
    {
        float bigRadius = 6000;
        float smallRadius = 2500;
        GameObject enemyTemplete = Resources.Load<GameObject>("Enemy");
        for (int i = 0; i < NUM_OF_ENEMIES; i++)
        {
            GameObject enemy = Instantiate(enemyTemplete);

            float delta = bigRadius - smallRadius;
            float length = delta * Random.value;
            Vector3 pos = UnityEngine.Random.onUnitSphere * (smallRadius + length);
            
            var entity = EntityManager.createNewEntity(new Component[]{
                new ComponentMoveForward()
                {
                    speed = 5
                },
                new ComponentsTransform()
                {
                    transform = enemy.transform
                },
                new ComponentEnemySetup()
                {
                    position = pos
                }
            });
        }
    }

    void initPlayer()
    {
        var player = Instantiate(Resources.Load<GameObject>("Player"));
        var entity = EntityManager.createNewEntity(new Component[]
        {
            new ComponentsTransform()
            {
                transform = player.transform
            },
            new ComponentMoveToInput()
            {
                agility = 10f,
            },
            new ComponentGlobalPlayerStats()
            {
                numOfEnemiesDestroyed = 0
            },
            new ComponentFireToInput()
            {
                currentWeaponIndex = 1,
                shootCooldown = 0
            },
            new ComponentMoveForward()
            {
                speed = 0.5f
            }
        });
        
        Cinemachine.CinemachineVirtualCamera camera = GameObject.Instantiate(Resources.Load<Cinemachine.CinemachineVirtualCamera>("Camera"));
        camera.Follow = player.transform;
        camera.LookAt = player.transform;
    }

    void initSystems()
    {
        EntityManager.registerSystem(new SystemMoveForward());
        EntityManager.registerSystem(new SystemEnemySetup());
        EntityManager.registerSystem(new SystemInputMove());
    }
}