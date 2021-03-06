using UnityEngine;
using UnityEngine.UI;

public class ComponentMoveForward : Component
{
    public float speed;
}

public class ComponentMoveToInput : Component
{
    public float agility;
    public Vector3 startTouchPosition;
    public Vector3 currentTouchPosition;
}

public class ComponentFireToInput : Component
{
    public float shootCooldown;
    public bool shootRight;
}

public class ComponentBullet : Component
{
    public CollisionRule collision;
    public float liveTime;
}

public class ComponentBeam : Component
{
    public float timeStarted;
}

public class ComponentDestroy : Component
{}

public class ComponentGlobalPlayerStats : Component
{
    public int numOfEnemiesDestroyed;
}

public class ComponentTransform : Component
{
    public Transform transform;
}

public class ComponentEnemySetup : Component
{
    public Vector3 position;
    public Vector3 flyToPosition;
}

public class ComponentEnemy : Component
{}

public class ComponentEnemyFlyTo : Component
{
    public Vector3 position;
}

public class ComponentTargetShip : Component
{}

public class ComponentUI : Component
{
    public Image expBar;
}