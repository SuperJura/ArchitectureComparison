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

    public void update()
    {
        var globalStats = getFirstComponent<ComponentGlobalPlayerStats>(globalStatsFilter);
        var entities = getEntities(filter);

        for (int i = 0; i < entities.Count; i++)
        {
            var components = getComponents(entities[i]);
            var fireToInput = getComponent<ComponentFireToInput>(components);
            var transform = getComponent<ComponentTransform>(components);

            //TODO: nastavi raditi nove weapone
            if(fireToInput.currentWeaponIndex == 0)
            {
                fireToInput.shootCooldown -= Time.deltaTime;
                if(Input.GetMouseButton(0) && fireToInput.shootCooldown <= 0)
                {
                    var goTransform = transform.transform;
                    createNewBullet(goTransform.position + goTransform.right * 6, transform.transform.rotation);
                    createNewBullet(goTransform.position + goTransform.right * -6, transform.transform.rotation);
                    fireToInput.shootCooldown = 0.1f;
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