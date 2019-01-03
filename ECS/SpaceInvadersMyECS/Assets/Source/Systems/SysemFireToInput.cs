using UnityEngine;
using static EntityManager;

public class SystemFireToInput : ISystem
{
    Component[] filter = new Component[]
    {
        new ComponentFireToInput(),
        new ComponentTransform(),
    };

    Component[] globalStatsFilter = new Component[]
    {
        new ComponentGlobalPlayerStats()
    };

    Component[] beamFilter = new Component[]
    {
        new ComponentBeam(),
        new ComponentTransform()
    };

    Component[] enemyFilter = new Component[]
    {
        new ComponentEnemy()
    };

    public void update()
    {
        var globalStats = getFirstComponent<ComponentGlobalPlayerStats>(globalStatsFilter);
        var entities = getEntities(filter);

        for (int i = 0; i < entities.Count; i++)
        {
            var components = getComponents(entities[i]);
            var fireToInput = getComponent<ComponentFireToInput>(components);
            var transform = getComponent<ComponentTransform>(components);

            int weaponIndex = GameHelper.getCurrentWeaponIndex(globalStats.numOfEnemiesDestroyed);
            if(weaponIndex == -1 || weaponIndex == 0)
            {
                fireToInput.shootCooldown -= Time.deltaTime;
                bool canFire = weaponIndex == 0 || (weaponIndex == -1 && fireToInput.shootCooldown <= 0);
                if(Input.GetMouseButton(0) && canFire)
                {
                    var goTransform = transform.transform;
                    createNewBullet(goTransform.position + goTransform.right * 6, transform.transform.rotation);
                    createNewBullet(goTransform.position + goTransform.right * -6, transform.transform.rotation);
                    fireToInput.shootCooldown = 0.1f;
                }
            }
            else if(weaponIndex == 1 || weaponIndex == 2)
            {
                var beams = getEntities(beamFilter);
                if(Input.GetMouseButtonDown(0))
                {
                    if(beams.Count == 0)
                    {
                        GameObject beam = GameObject.Instantiate(Game.BEAM_TEMPLATE);
                        beam.transform.position = transform.transform.position;
                        beam.transform.rotation = transform.transform.rotation;
                        beam.transform.SetParent(transform.transform);
                        beam.transform.localPosition += new Vector3(0, 0, 15);
                        var newEntity = createNewEntity(new Component[]
                        {
                            new ComponentBeam()
                            {
                                timeStarted = 0
                            },
                            new ComponentTransform()
                            {
                                transform = beam.transform
                            },
                            new ComponentBullet()
                            {
                                collision = beam.GetComponentInChildren<CollisionRule>()
                            }
                        });
                    }
                }
                else if(Input.GetMouseButtonUp(0))
                {
                    if(beams.Count > 0)
                    {
                        for (int j = 0; j < beams.Count; j++)
                        {
                            addComponent(beams[i], new ComponentDestroy());
                        }
                    }
                }
                if(beams.Count > 0)
                {
                    for (int j = 0; j < beams.Count; j++)
                    {
                        var beamComponents = getComponents(beams[i]);
                        var beamTransform = getComponent<ComponentTransform>(beamComponents);
                        var beamStats = getComponent<ComponentBeam>(beamComponents);
                        float maxScale = weaponIndex == 1 ? 1.5f : 50f;
                        float scaleSpeed = weaponIndex == 1 ? 1 : 20f;
                        
                        float scaleMultiplyer = Mathf.Clamp(beamStats.timeStarted, 0, maxScale);
                        beamStats.timeStarted += Time.deltaTime * scaleSpeed;
                        beamTransform.transform.localScale = new Vector3(scaleMultiplyer, scaleMultiplyer, 1);

                        if(weaponIndex == 2)
                        {
                            if(Mathf.Approximately(scaleMultiplyer, maxScale))
                            {
                                PerformanceTest.finish();
                                var allEnemies = getEntities(enemyFilter);
                                for (int k = 0; k < allEnemies.Count; k++)
                                {
                                    addComponent(allEnemies[k], new ComponentDestroy());
                                }
                            }
                        }

                        beamTransform.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 10000); //Nesto s nested prefabovima je buggano pa se makne offset childa
                    }
                }
            }
        }

        void createNewBullet(Vector3 position, Quaternion rotation)
        {
            var bulletGO = GameObject.Instantiate(Game.BULLET_TEMPLATE);
            bulletGO.transform.rotation = rotation;
            bulletGO.transform.position = position;
            var bulletEntity = createNewEntity(new Component[]
            {
                new ComponentBullet()
                {
                    collision = bulletGO.GetComponentInChildren<CollisionRule>()
                },
                new ComponentMoveForward()
                {
                    speed = 1000
                },
                new ComponentTransform()
                {
                    transform = bulletGO.transform
                }
            });
        }
    }
}