using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    void init(GameObject playerObj, IInput inputDevice);
    void shoot();
    void move(Vector3 direction);
    void updatePlayer();
}