using UnityEngine;
using static EntityManager;

public class SystemDestroy : ISystem
{
    Component[] filter = new Component[]
    {
        new ComponentDestroy()
    };

    public void update()
    {
        var entities = getEntities(filter);
        for (int i = 0; i < entities.Count; i++)
        {
            EntityManager.destroyEntity(entities[i]);
        }
    }
}