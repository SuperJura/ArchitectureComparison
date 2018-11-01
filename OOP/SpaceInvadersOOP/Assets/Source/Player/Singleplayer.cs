using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Singleplayer : IPlayer
{
    const float MIN_FOV = 60;
    const float MAX_FOV = 80;

    IInput device;
    
    float speed = 0.1f;
    float agility = 0.02f;

    CharacterController playerControler;
    CinemachineVirtualCamera cmCamera;

    float horizontalRotation;
    float verticalRotation;
    float boostSpeed = 1;
    bool shotRight = false;

    GameObject bulletTemplate;
    List<GameObject> bullets;

    public void init(GameObject playerObj, IInput inputDevice)
    {
        this.device = inputDevice;
        playerControler = playerObj.GetComponent<CharacterController>();
        cmCamera = GameObject.Instantiate(Resources.Load<CinemachineVirtualCamera>("Camera"));
        bulletTemplate = Resources.Load<GameObject>("Bullet");
        cmCamera.Follow = playerControler.transform;
        cmCamera.LookAt = playerControler.transform;
        bullets = new List<GameObject>();
    }

    public void move(Vector3 direction)
    {
        playerControler.Move(direction);
    }

    public void shoot()
    {
        Vector3 pos = playerControler.transform.position;
        pos += playerControler.transform.right * (shotRight ? 10 : -10);
        shotRight = !shotRight; 
        GameObject bullet = GameObject.Instantiate(bulletTemplate,  pos, playerControler.transform.rotation);
        bullets.Add(bullet);
    }

    public void updatePlayer()
    {
        if(device.getFire()) shoot();
        if(device.getBoost())
        {
            boostSpeed = Mathf.Clamp(boostSpeed + Time.deltaTime * 2, 10, 100);
        }
        else
        {
            boostSpeed = 1;
        }
        Vector3 moveDir = playerControler.transform.forward * speed * boostSpeed;

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
        foreach (var bullet in bullets)
        {
            bullet.transform.position += bullet.transform.forward * Time.deltaTime * 1000;
        }
    }
}