using UnityEngine;

public class SystemEnemySetup : ISystem
{
    Component[] targetComponentsFilter = new Component[]
    {
        new ComponentTargetShip(),
        new ComponentsTransform()
    };
    Component[] enemiesToSetupFilter = new Component[]
    {
        new ComponentEnemySetup(),
        new ComponentsTransform()
    };

    public void update()
    {
        var enemyEntities = EntityManager.getEntities(enemiesToSetupFilter);
        if(enemyEntities.Count == 0) return;
        var entityTarget = EntityManager.getEntities(targetComponentsFilter)[0];
        var targetPosition = EntityManager.GetComponent<ComponentsTransform>(EntityManager.getComponents(entityTarget)).transform;

        for (int i = 0; i < enemyEntities.Count; i++)
        {
            var components = EntityManager.getComponents(enemyEntities[i]);
            var transform = EntityManager.GetComponent<ComponentsTransform>(components);
            var setup = EntityManager.GetComponent<ComponentEnemySetup>(components);
            transform.transform.position = setup.position;
            transform.transform.LookAt(targetPosition);
            EntityManager.removeComponent(enemyEntities[i], setup);
        }
    }
}