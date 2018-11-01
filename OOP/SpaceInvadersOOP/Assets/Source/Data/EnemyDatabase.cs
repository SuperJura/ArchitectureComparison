using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemies", menuName = "SpaceInvaders/Enemies")]
public class EnemyDatabase : ScriptableObject, ISpawnData
{
    GameObject[] enemies;

    public GameObject getRandomObject()
    {
        return enemies[Random.Range(0, enemies.Length)];
    }
}