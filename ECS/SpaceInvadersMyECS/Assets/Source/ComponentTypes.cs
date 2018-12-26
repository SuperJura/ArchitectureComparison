using UnityEngine;

public class ComponentMoveForward : Component
{
    public float speed;
}

public class ComponentMoveToInput : Component
{
    public float agility;
}

public class ComponentFireToInput : Component
{
    public int currentWeaponIndex;
    public float shootCooldown;
}

public class ComponentGlobalPlayerStats : Component
{
    public int numOfEnemiesDestroyed;
}

public class ComponentsTransform : Component
{
    public Transform transform;
}

public class ComponentEnemySetup : Component
{
    public Vector3 position;
}

public class ComponentTargetShip : Component
{
    
}