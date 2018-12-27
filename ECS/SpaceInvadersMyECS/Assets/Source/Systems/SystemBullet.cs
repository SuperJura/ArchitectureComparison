using UnityEngine;
using static EntityManager;

public class SystemBullet : ISystem
{
    Component[] filter = new Component[]
    {
        new ComponentBullet()
    };

    Component[] globalStatsFilter = new Component[]
    {
        new ComponentGlobalPlayerStats()
    };

    public void update()
    {
        var entities = getEntities(filter);
        var globalStats = getFirstComponent<ComponentGlobalPlayerStats>(globalStatsFilter);

        for (int i = 0; i < entities.Count; i++)
        {
            var components = getComponents(entities[i]);
            var bullet = getComponent<ComponentBullet>(components);
            globalStats.numOfEnemiesDestroyed += bullet.collision.numOfDestroyedEnemies;
            bullet.collision.numOfDestroyedEnemies = 0;

            bullet.liveTime += Time.deltaTime;
            if(bullet.liveTime > 5)
            {
                EntityManager.addComponent(entities[i], new ComponentDestroy());
            }
        }
    }
}