using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EntityManager
{
    static bool inited = false;
    static Dictionary<Entity, HashSet<Component>> entities;
    static List<ISystem> systems;

    static Dictionary<Type, int> componentCodes;
    static int nextCode = 1;

    public static void init()
    {
        if(inited) return;
        
        entities = new Dictionary<Entity, HashSet<Component>>();
        systems = new List<ISystem>();
        componentCodes = new Dictionary<Type, int>();

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

    public static void destroyEntity(Entity entity)
    {
        ComponentTransform transform = getComponent<ComponentTransform>(getComponents(entity));
        if(transform != null)
        {
            GameObject.Destroy(transform.transform.gameObject);
        }
        entities.Remove(entity);
    }

    public static bool addComponent(Entity entity, Component component)
    {
        var componentType = component.GetType();
        if(!componentCodes.ContainsKey(componentType))
        {
            componentCodes.Add(componentType, nextCode);
            nextCode *= 2;
        }
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

    public static T getComponent<T>(HashSet<Component> components) where T : Component
    {
        foreach (var component in components)
        {
            if(component.GetType() == typeof(T)) return (T)component;
        }
        return null;
    }

    public static T getFirstComponent<T>(Component[] filter) where T : Component
    {
        var entities = getEntities(filter);
        var components = getComponents(entities[0]);
        return getComponent<T>(components);
    }

    public static List<Entity> getEntities(params Component[] filter)
    {
        List<Entity> output = new List<Entity>(entities.Count);
        foreach (var pair in entities)
        {
            bool add = true;
            for (int i = 0; i < filter.Length; i++)
            {
                if(!pair.Value.Contains(filter[i]))
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