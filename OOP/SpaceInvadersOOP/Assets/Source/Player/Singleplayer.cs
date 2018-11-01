using UnityEngine;
using Cinemachine;

public class Singleplayer : IPlayer
{
    const float MIN_FOV = 40;
    const float MAX_FOV = 60;

    IInput device;
    
    float speed = 0.1f;
    float agility = 0.02f;

    CharacterController playerControler;
    CinemachineVirtualCamera cmCamera;

    float horizontalRotation;
    float verticalRotation;

    public void init(GameObject playerObj, IInput inputDevice)
    {
        this.device = inputDevice;
        playerControler = playerObj.GetComponent<CharacterController>();
        cmCamera = GameObject.Instantiate(Resources.Load<CinemachineVirtualCamera>("Camera"));
        cmCamera.Follow = playerControler.transform;
        cmCamera.LookAt = playerControler.transform;
    }

    public void move(Vector3 direction)
    {
        playerControler.Move(direction);
    }

    public void shoot()
    {
        Debug.Log("Shooting");
    }

    public void updatePlayer()
    {
        if(device.getFire()) shoot();
        float boost = device.getBoost() ? 10 : 1;
        Vector3 moveDir = playerControler.transform.forward * speed * boost;

        horizontalRotation = Mathf.MoveTowards(horizontalRotation, device.getHorizontal(), agility);
        verticalRotation = Mathf.MoveTowards(verticalRotation, device.getVertical(), agility);

        playerControler.transform.Rotate(verticalRotation, horizontalRotation, 0);
        move(moveDir);

        float fov = cmCamera.m_Lens.FieldOfView;
        if(device.getBoost())
        {
            fov = Mathf.MoveTowards(fov, MAX_FOV, Time.deltaTime * (MAX_FOV - fov));
        }
        else
        {
            fov = Mathf.MoveTowards(fov, MIN_FOV, Time.deltaTime * 100);
        }
        cmCamera.m_Lens.FieldOfView = fov;
    }
}