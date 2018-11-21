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
    static MeshInstanceRenderer playerRenderer;

    protected override void OnUpdate()
    {
        if(playerRenderer.mesh == null)
        {
            playerRenderer = Bootstraper.getLookFromPrototype("Player");
            group = GetComponentGroup(Bootstraper.playerArchtype.ComponentTypes);
            
            var entity = EntityManager.CreateEntity(Bootstraper.playerArchtype);
            EntityManager.SetComponentData(entity, new Position(){Value = new float3(20, 20, 20)});
            EntityManager.SetComponentData(entity, new Scale(){Value = new float3(0.1f, 0.1f, 0.1f)});
            EntityManager.SetComponentData(entity, new Rotation(){Value = quaternion.identity});
            EntityManager.SetComponentData(entity, new PlayerStats()
            {
                speed = 0.5f,
                agility = 0.2f
            });
            EntityManager.SetSharedComponentData(entity, playerRenderer);

            GameObject cameraFollow = new GameObject("CameraFolow");
            cameraFollow.transform.position = new Vector3(20, 20, 20);
            
            CinemachineVirtualCamera camera = GameObject.Instantiate(Resources.Load<CinemachineVirtualCamera>("Camera"));
            camera.Follow = cameraFollow.transform;
            camera.LookAt = cameraFollow.transform;
            
            EntityManager.SetSharedComponentData(entity, new Player()
            {
                cameraPoint = cameraFollow,
                camera = camera
            });
        }
        
        var playerEntity = group.GetEntityArray()[0];
        var playerPosition = group.GetComponentDataArray<Position>()[0];
        var playerRotation = group.GetComponentDataArray<Rotation>()[0];
        var playerStats = group.GetComponentDataArray<PlayerStats>()[0];
        var player = group.GetSharedComponentDataArray<Player>()[0];

        float3 moveDir = math.forward(playerRotation.Value);
        quaternion rotation = playerRotation.Value;

        float rotationSpeed = 2;
        Vector3 newEulerRotation = Vector3.zero;
        if(Input.GetKey(KeyCode.A)) { newEulerRotation.y -= rotationSpeed * playerStats.agility * Time.deltaTime; }
        if(Input.GetKey(KeyCode.D)) { newEulerRotation.y += rotationSpeed * playerStats.agility * Time.deltaTime; }
        if(Input.GetKey(KeyCode.W)) { newEulerRotation.x -= rotationSpeed * playerStats.agility * Time.deltaTime; }
        if(Input.GetKey(KeyCode.S)) { newEulerRotation.x += rotationSpeed * playerStats.agility * Time.deltaTime; }

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
        
        float fov = player.camera.m_Lens.FieldOfView;
        if(boost)
        {
            fov = Mathf.MoveTowards(fov, MAX_FOV, Time.deltaTime * (MAX_FOV - fov));
        }
        else
        {
            fov = Mathf.MoveTowards(fov, MIN_FOV, Time.deltaTime * 100);
        }
        player.camera.m_Lens.FieldOfView = fov;

    }
}