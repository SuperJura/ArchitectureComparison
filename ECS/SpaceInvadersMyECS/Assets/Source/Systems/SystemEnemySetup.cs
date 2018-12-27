using UnityEngine;
using static EntityManager;

public class SystemEnemySetup : ISystem
{
    Component[] targetComponentsFilter = new Component[]
    {
        new ComponentTargetShip(),
        new ComponentTransform()
    };
    Component[] enemiesToSetupFilter = new Component[]
    {
        new ComponentEnemySetup(),
        new ComponentTransform()
    };

    public void update()
    {
        var enemyEntities = getEntities(enemiesToSetupFilter);
        if(enemyEntities.Count == 0) return;
        var entityTarget = getEntities(targetComponentsFilter)[0];
        var targetPosition = getComponent<ComponentTransform>(getComponents(entityTarget)).transform;

        for (int i = 0; i < enemyEntities.Count; i++)
        {
            var components = getComponents(enemyEntities[i]);
            var transform = getComponent<ComponentTransform>(components);
            var setup = getComponent<ComponentEnemySetup>(components);
            transform.transform.position = setup.position;
            transform.transform.LookAt(targetPosition);
            transform.transform.GetComponent<CollisionRule>().thisEntity = enemyEntities[i];

            addComponent(enemyEntities[i], new ComponentEnemyFlyTo()
            {
                position = setup.flyToPosition
            });
            removeComponent(enemyEntities[i], setup);
        }
    }
}