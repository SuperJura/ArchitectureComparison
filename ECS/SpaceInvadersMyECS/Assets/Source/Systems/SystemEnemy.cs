using UnityEngine;
using static EntityManager;

public class SystemEnemy : ISystem
{
    Component[] filter = new Component[]
    {
        new ComponentTransform(),
        new ComponentEnemyFlyTo(),
        new ComponentMoveForward()
    };

    public void update()
    {
        var entities = getEntities(filter);
        for (int i = 0; i < entities.Count; i++)
        {
            var components = getComponents(entities[i]);
            var transform = getComponent<ComponentTransform>(components);
            var flyTo = getComponent<ComponentEnemyFlyTo>(components);
            var move = getComponent<ComponentMoveForward>(components);

            if(Vector3.Distance(transform.transform.position, flyTo.position) < 500f)
            {
                move.speed = 2;
                removeComponent(entities[i], flyTo);
            }
        }
    }
}