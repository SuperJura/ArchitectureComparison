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

            Vector3 a = transform.transform.position;
            Vector3 b =  flyTo.position;
            float x = a.x > b.x ? (a.x - b.x) : (b.x - a.x);
            float y = a.y > b.y ? (a.y - b.y) : (b.y - a.y);
            float z = a.z > b.z ? (a.z - b.z) : (b.z - a.z);
            float distance = x + y + z;
            if(distance < 500)
            {
                move.speed = 2;
                removeComponent(entities[i], flyTo);
            }
        }
    }
}