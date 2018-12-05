using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Cinemachine;

public class PlayerSystem : ComponentSystem
{
    const float MIN_FOV = 60;
    const float MAX_FOV = 80;

    ComponentGroup group;
    ComponentGroup beamGroup;

    static MeshInstanceRenderer bulletRenderer;
    static MeshInstanceRenderer beamRenderer;

    protected override void OnUpdate()
    {
        if (group == null)
        {
            group = GetComponentGroup(Bootstraper.playerTypes);
            beamGroup = GetComponentGroup(Bootstraper.beamTypes);
            bulletRenderer = Bootstraper.getLookFromPrototype("Bullet");
            beamRenderer = Bootstraper.getLookFromPrototype("Beam");
        }

        var playerEntity = group.GetEntityArray()[0];
        var playerPosition = group.GetComponentDataArray<Position>()[0];
        var playerRotation = group.GetComponentDataArray<Rotation>()[0];
        var playerStats = group.GetComponentDataArray<PlayerStats>()[0];
        var player = group.GetSharedComponentDataArray<Player>()[0];
        var beamArray = beamGroup.GetEntityArray();

        float3 moveDir = math.forward(playerRotation.Value);
        quaternion rotation = playerRotation.Value;

        float rotationSpeed = 2;
        Vector3 newEulerRotation = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) { newEulerRotation.y -= rotationSpeed * playerStats.agility * Time.deltaTime; }
        if (Input.GetKey(KeyCode.D)) { newEulerRotation.y += rotationSpeed * playerStats.agility * Time.deltaTime; }
        if (Input.GetKey(KeyCode.W)) { newEulerRotation.x -= rotationSpeed * playerStats.agility * Time.deltaTime; }
        if (Input.GetKey(KeyCode.S)) { newEulerRotation.x += rotationSpeed * playerStats.agility * Time.deltaTime; }

        rotation = math.mul(rotation, quaternion.AxisAngle(math.up(), newEulerRotation.y));
        rotation = math.mul(rotation, quaternion.AxisAngle(new float3(1, 0, 0), newEulerRotation.x));

        playerRotation.Value = rotation;
        EntityManager.SetComponentData(playerEntity, playerRotation);

        bool boost = Input.GetKey(KeyCode.Space);
        float3 newPosition = playerPosition.Value + moveDir * (boost ? 10 : 1);
        EntityManager.SetComponentData(playerEntity, new Position()
        {
            Value = newPosition
        });

        player.cameraPoint.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
        player.cameraPoint.transform.rotation = rotation;

        if (Input.GetMouseButton(0))
        {
            if(playerStats.currnetWeaponIndex == -1)
            {
                playerStats.shootCooldown -= Time.deltaTime;
                if(playerStats.shootCooldown <= 0)
                {
                    float3 forward = math.forward(playerRotation.Value) * 200;
                    shootBullet(newPosition + forward + Math.right(playerRotation.Value) * 50, playerRotation.Value);
                    shootBullet(newPosition + forward + Math.right(playerRotation.Value) * -50, playerRotation.Value);
                    playerStats.shootCooldown = 0.15f;
                }
            }
            else if(playerStats.currnetWeaponIndex == 0)
            {
                float3 forward = math.forward(playerRotation.Value) * 200;
                shootBullet(newPosition + forward + Math.right(playerRotation.Value) * 50, playerRotation.Value);
                shootBullet(newPosition + forward + Math.right(playerRotation.Value) * -50, playerRotation.Value);
            }
            else if(playerStats.currnetWeaponIndex == 1 || playerStats.currnetWeaponIndex == 2)
            {
                var beamEntity = Entity.Null;
                Beam beamStats;
                if (beamArray.Length == 0)
                {
                    beamEntity = EntityManager.CreateEntity(Bootstraper.beamTypes);
                    EntityManager.SetSharedComponentData(beamEntity, beamRenderer);
                    beamStats = new Beam()
                    {
                        timeSinceStarted = 1,
                        maxScale = playerStats.currnetWeaponIndex == 1 ? 3 : 50,
                        scaleSpeed = playerStats.currnetWeaponIndex == 1 ? 1 : 20,
                    };
                    EntityManager.SetComponentData(beamEntity, beamStats);
                }
                else
                {
                    beamEntity = beamArray[0];
                    beamStats = beamGroup.GetComponentDataArray<Beam>()[0];
                }

                EntityManager.SetComponentData(beamEntity, new Position()
                {
                    Value = playerPosition.Value + math.forward(playerRotation.Value) * 2200
                });
                var beamRotation = beamGroup.GetComponentDataArray<Rotation>()[0];
                beamRotation.Value = math.mul(playerRotation.Value, quaternion.RotateX(math.radians(90)));
                EntityManager.SetComponentData(beamEntity, beamRotation);

                float scaleMultiplyer = math.clamp(beamStats.timeSinceStarted, 0, beamStats.maxScale);
                EntityManager.SetComponentData(beamEntity, new Scale()
                {
                    Value = new float3(50 * scaleMultiplyer, 2000, 50 * scaleMultiplyer)
                });
                beamStats.timeSinceStarted += Time.deltaTime * beamStats.scaleSpeed;
                EntityManager.SetComponentData(beamEntity, beamStats);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (beamArray.Length > 0)
            {
                EntityManager.DestroyEntity(beamArray[0]);
            }
        }

        float fov = player.camera.m_Lens.FieldOfView;
        if (boost)
        {
            fov = Mathf.MoveTowards(fov, MAX_FOV, Time.deltaTime * (MAX_FOV - fov));
        }
        else
        {
            fov = Mathf.MoveTowards(fov, MIN_FOV, Time.deltaTime * 100);
        }
        player.camera.m_Lens.FieldOfView = fov;

        float percent = playerStats.enemiesDestroyed / (float)Bootstraper.NUM_OF_ENEMIES;
        int newWeaponIndex = playerStats.currnetWeaponIndex;
        for (int i = 0; i < Bootstraper.neededPercentForNextWeapon.Length; i++)
        {
            if (percent >= Bootstraper.neededPercentForNextWeapon[i])
            {
                newWeaponIndex = i;
            }
        }
        if (newWeaponIndex != playerStats.currnetWeaponIndex)
        {
            playerStats.currnetWeaponIndex = newWeaponIndex;
            if(beamArray.Length == 1)
            {
                EntityManager.DestroyEntity(beamArray[0]);
            }
        }
        EntityManager.SetComponentData(playerEntity, playerStats);
    }

    void shootBullet(float3 position, quaternion rotation)
    {
        var bullet = EntityManager.CreateEntity(Bootstraper.bulletTypes);
        EntityManager.SetComponentData(bullet, new Position()
        {
            Value = position
        });
        EntityManager.SetComponentData(bullet, new Rotation()
        {
            Value = rotation
        });
        EntityManager.SetComponentData(bullet, new Scale()
        {
            Value = new float3(3, 3, 3)
        });
        EntityManager.SetSharedComponentData(bullet, bulletRenderer);
        EntityManager.SetComponentData(bullet, new Bullet()
        {
            speed = 100,
            timeToDestroy = 2f
        });
    }
}