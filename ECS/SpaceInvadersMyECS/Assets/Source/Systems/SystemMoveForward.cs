using UnityEngine;
using static EntityManager;

public class SystemMoveForward : ISystem
{
    Component[] filter = new Component[] 
    {
        new ComponentMoveForward(),
        new ComponentTransform()
    };
    public void update()
    {
        var entities = getEntities(filter);
        for (int i = 0; i < entities.Count; i++)
        {
            var components = getComponents(entities[i]);
            var moveForward = getComponent<ComponentMoveForward>(components);
            var transform = getComponent<ComponentTransform>(components);

            transform.transform.position = Vector3.MoveTowards(transform.transform.position, transform.transform.position + transform.transform.forward * moveForward.speed,  moveForward.speed * Time.deltaTime);
        }
    }
}