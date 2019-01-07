using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EntityManager
{
    static bool inited = false;
    static Dictionary<Entity, int> entitiesCache;
    static Dictionary<Entity, HashSet<Component>> entitiesData;
    static List<ISystem> systems;

    static Dictionary<Type, int> componentCodes;
    static int nextCode = 1;

    public static void init()
    {
        if(inited) return;
        
        entitiesCache = new Dictionary<Entity, int>();
        entitiesData = new Dictionary<Entity, HashSet<Component>>();
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
        entitiesCache.Add(entity, 0);
        entitiesData.Add(entity, new HashSet<Component>());

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
        entitiesCache.Remove(entity);
        entitiesData.Remove(entity);
    }

    public static bool addComponent(Entity entity, Component component)
    {
        var componentType = component.GetType();
        if(!componentCodes.ContainsKey(componentType))
        {
			addNewComponentCache(component);
        }
        int componentId = componentCodes[componentType];
        if((entitiesCache[entity] & componentId) == 0)
        {
            entitiesCache[entity] += componentId;
            entitiesData[entity].Add(component);
        }
        else
        {
            return false;
        }

        return true;
    }

    public static bool removeComponent(Entity entity, Component component)
    {
        if(!entitiesCache.ContainsKey(entity)) return false;
        int componentId = componentCodes[component.GetType()];
        if((entitiesCache[entity] & componentId) == 0) return false;
        entitiesCache[entity] -= componentId;
        entitiesData[entity].Remove(component);
        return true;
    }

    public static HashSet<Component> getComponents(Entity entity)
    {
        if(!entitiesCache.ContainsKey(entity)) return null;
        return entitiesData[entity];
    }

    public static T getComponent<T>(HashSet<Component> components) where T : Component
    {
		var type = typeof(T);
		foreach (var component in components)
        {
            if(component.GetType() == type) return (T)component;
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
        List<Entity> output = new List<Entity>(entitiesCache.Count);
        int filterCode = 0;
        for (int i = 0; i < filter.Length; i++)
        {
			var componentType = filter[i].GetType();
			if (!componentCodes.ContainsKey(componentType)) return output;
            filterCode += componentCodes[filter[i].GetType()];
        }

        foreach (var pair in entitiesCache)
        {
            if((pair.Value & filterCode) == filterCode)
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

    public static void cancelSystem(ISystem system)
    {
        systems.Remove(system);
    }

	static void addNewComponentCache(Component component)
	{
		var componentType = component.GetType();
		componentCodes.Add(componentType, nextCode);
		nextCode *= 2;
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