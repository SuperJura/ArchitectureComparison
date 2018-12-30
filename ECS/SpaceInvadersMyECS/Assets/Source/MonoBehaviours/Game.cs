using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour 
{
    public const int NUM_OF_ENEMIES = 15000;

    public static GameObject ENEMY_TEMPLATE;
    public static GameObject BULLET_TEMPLATE;
    public static GameObject BEAM_TEMPLATE;
    public static readonly float[] neededPercentForNextWeapon = new float[] {0.01f, 0.02f, 0.06f};

    GameObject targetShip;

    void Start()
    {
        EntityManager.init();
        initTemplates();
        initTargetShip();
        initEnemyShips();
        initPlayer();
        initUI();
        initSystems();
    }

    void Update()
    {
        EntityManager.update();
    }

    void initTemplates()
    {
        ENEMY_TEMPLATE = Resources.Load<GameObject>("Enemy");
        BULLET_TEMPLATE = Resources.Load<GameObject>("Bullet");
        BEAM_TEMPLATE = Resources.Load<GameObject>("Beam");
    }

    void initTargetShip()
    {
        targetShip = GameObject.Find("Station");
        var entity = EntityManager.createNewEntity(new Component[]
        {
            new ComponentTransform()
            {
                transform = targetShip.transform
            },
            new ComponentTargetShip()
        });
    }

    void initEnemyShips()
    {
        float bigRadius = 6000;
        float smallRadius = 2500;
        for (int i = 0; i < NUM_OF_ENEMIES; i++)
        {
            GameObject enemy = Instantiate(ENEMY_TEMPLATE);

            float delta = bigRadius - smallRadius;
            float length = delta * Random.value;
            Vector3 pos = UnityEngine.Random.onUnitSphere * (smallRadius + length);
            Vector3 dir = pos - targetShip.transform.position;
            
            var entity = EntityManager.createNewEntity(new Component[]{
                new ComponentMoveForward()
                {
                    speed = 5000
                },
                new ComponentTransform()
                {
                    transform = enemy.transform
                },
                new ComponentEnemySetup()
                {
                    position = pos + dir,
                    flyToPosition = pos
                },
                new ComponentEnemy()
            });
        }
    }

    void initPlayer()
    {
        var player = Instantiate(Resources.Load<GameObject>("Player"));
        var entity = EntityManager.createNewEntity(new Component[]
        {
            new ComponentTransform()
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

    void initUI()
    {
        Image bar = GameObject.Find("Canvas/ExpBar/Bar").GetComponent<Image>();
        var entity = EntityManager.createNewEntity(new Component[]
        {
            new ComponentUI()
            {
                expBar = bar
            }
        });
    }

    void initSystems()
    {
        EntityManager.registerSystem(new SystemMoveForward());
        EntityManager.registerSystem(new SystemEnemySetup());
        EntityManager.registerSystem(new SystemEnemy());
        EntityManager.registerSystem(new SystemInputMove());
        EntityManager.registerSystem(new SystemFireToInput());
        EntityManager.registerSystem(new SystemBullet());
        EntityManager.registerSystem(new SystemUI());
        EntityManager.registerSystem(new SystemDestroy());
    }
}