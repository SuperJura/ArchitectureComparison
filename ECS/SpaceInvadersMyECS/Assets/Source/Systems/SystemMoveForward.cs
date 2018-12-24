using System.Collections.Generic;
using UnityEngine;

public class SystemMoveForward : ISystem
{
    Component[] filter;
    public void update()
    {
        if(filter == null)
        {
            filter = new Component[] 
            {
                new ComponentMoveForward(),
                new ComponentsTransform()
            };
        }

        var entities = EntityManager.getEntities(filter);
        for (int i = 0; i < entities.Count; i++)
        {
            var components = EntityManager.getComponents(entities[i]);
            var moveForward = EntityManager.GetComponent<ComponentMoveForward>(components);
            var transform = EntityManager.GetComponent<ComponentsTransform>(components);

            transform.transform.position = Vector3.MoveTowards(transform.transform.position, transform.transform.position + transform.transform.forward,  moveForward.speed * Time.deltaTime);
        }
    }
}