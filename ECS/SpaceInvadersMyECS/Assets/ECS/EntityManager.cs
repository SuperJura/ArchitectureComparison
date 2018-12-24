using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntityManager
{
    static bool inited = false;
    static Dictionary<Entity, HashSet<Component>> entities;
    static List<ISystem> systems;

    public static void init()
    {
        if(inited) return;
        
        entities = new Dictionary<Entity, HashSet<Component>>();
        systems = new List<ISystem>();

        inited = true;
    }

    public static void update()
    {
        for (int i = 0; i < systems.Count; i++)
        {
            systems[i].update();
        }
    }

    public static Entity createNewEntity(params Component[] components)
    {
        Entity entity = new Entity();
        entities.Add(entity, new HashSet<Component>(new ComponentComparer()));

        for (int i = 0; i < components.Length; i++)
        {
            addComponent(entity, components[i]);
        }

        return entity;
    }

    public static bool addComponent(Entity entity, Component component)
    {
        if(!entities[entity].Contains(component))
        {
            entities[entity].Add(component);
        }
        else
        {
            return false;
        }

        return true;
    }

    public static bool removeComponent(Entity entity, Component component)
    {
        if(!entities.ContainsKey(entity)) return false;
        if(!entities[entity].Contains(component)) return false;
        entities[entity].Remove(component);
        return true;
    }

    public static HashSet<Component> getComponents(Entity entity)
    {
        if(!entities.ContainsKey(entity)) return null;
        return entities[entity];
    }

    public static T GetComponent<T>(HashSet<Component> components) where T : Component
    {
        foreach (var component in components)
        {
            if(component.GetType() == typeof(T)) return (T)component;
        }
        return null;
    }

    public static List<Entity> getEntities(params Component[] components)
    {
        List<Entity> output = new List<Entity>(entities.Count);
        foreach (var pair in entities)
        {
            bool add = true;
            for (int i = 0; i < components.Length; i++)
            {
                if(!pair.Value.Contains(components[i]))
                {
                    add = false;
                    break;
                }
            }
            if(add)
            {
                output.Add(pair.Key);
            }
        }

        return output;
    }

    public static void registerSystem(ISystem system)
    {
        systems.Add(system);
    }

    class ComponentComparer : IEqualityComparer<Component>
    {
        public bool Equals(Component x, Component y)
        {
            return x.GetType() == y.GetType();
        }

        public int GetHashCode(Component obj)
        {
            return base.GetHashCode();
        }
    }
}