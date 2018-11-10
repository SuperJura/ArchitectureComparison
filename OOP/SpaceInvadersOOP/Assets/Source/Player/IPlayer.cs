using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    void init(GameObject playerObj, IInput inputDevice);
    void move(Vector3 direction);
    void updatePlayer();
    void changeWeapon(IWeapon newWeapon);
    void changeBullet(IBullet newBullet);
    IInput getDevice();
    GameObject getPlayerObject();
}