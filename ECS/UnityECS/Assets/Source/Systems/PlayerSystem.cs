using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Cinemachine;

public class PlayerSystem : ComponentSystem
{
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
            EntityManager.SetComponentData(entity, new PlayerStats()
            {
                speed = 0.5f,
                agility = 0.2f
            });
            EntityManager.SetSharedComponentData(entity, playerRenderer);
            GameObject cameraFollow = new GameObject("CameraFolow");
            cameraFollow.transform.position = new Vector3(20, 20, 20);
            EntityManager.SetSharedComponentData(entity, new Player()
            {
                cameraPoint = cameraFollow
            });

            CinemachineVirtualCamera camera = GameObject.Instantiate(Resources.Load<CinemachineVirtualCamera>("Camera"));
            camera.Follow = cameraFollow.transform;
            camera.LookAt = cameraFollow.transform;
        }

        float3 moveDir = Vector3.forward;
        float3 rotation = Vector3.zero;
        if(Input.GetKeyDown(KeyCode.A)) { rotation.x += 1; }
        if(Input.GetKeyDown(KeyCode.D)) { rotation.x += -1; }
        if(Input.GetKeyDown(KeyCode.W)) { rotation.y += -1; }
        if(Input.GetKeyDown(KeyCode.S)) { rotation.y += -1; }

        var playerEntity = group.GetEntityArray()[0];
        var playerPosition = group.GetComponentDataArray<Position>()[0];
        var playerRotation = group.GetComponentDataArray<Rotation>()[0];
        var playerStats = group.GetComponentDataArray<PlayerStats>()[0];
        var player = group.GetSharedComponentDataArray<Player>()[0];

        float3 newPosition = playerPosition.Value + moveDir;
        EntityManager.SetComponentData(playerEntity, new Position()
        {
            Value = newPosition
        });
        player.cameraPoint.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
    }
}