using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Singleplayer : IPlayer
{
    const float MIN_FOV = 60;
    const float MAX_FOV = 80;

    IInput device;
    
    float speed = 0.5f;
    float agility = 0.2f;

    CharacterController playerControler;
    CinemachineVirtualCamera cmCamera;

    float horizontalRotation;
    float verticalRotation;
    float boostSpeed = 1;

    IWeapon currentWeapon;

    public void changeWeapon(IWeapon newWeapon)
    {
        currentWeapon.destroy();
        currentWeapon = newWeapon;
    }

    public void changeBullet(IBullet newBullet)
    {
        currentWeapon.changeBullet(newBullet);
    }

    public IInput getDevice()
    {
        return device;
    }

    public GameObject getPlayerObject()
    {
        return playerControler.gameObject;
    }

    public void init(GameObject playerObj, IInput inputDevice)
    {
        this.device = inputDevice;
        playerControler = playerObj.GetComponent<CharacterController>();
        cmCamera = GameObject.Instantiate(Resources.Load<CinemachineVirtualCamera>("Camera"));
        
        cmCamera.Follow = playerControler.transform;
        cmCamera.LookAt = playerControler.transform;

        currentWeapon = new Rifle(0.15f);
        currentWeapon.init();
    }

    public void move(Vector3 direction)
    {
        playerControler.Move(direction);
    }

    public void updatePlayer()
    {
        currentWeapon.updateWeapon(this);
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
    }
}