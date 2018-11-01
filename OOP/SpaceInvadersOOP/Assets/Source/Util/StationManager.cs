using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    static StationManager instance;

    public static StationManager getInstance()
    {
        return instance;
    }

    GameObject shield;
    Vector2 offset = Vector2.zero;
    void Awake()
    {
        shield= transform.Find("Shield").gameObject;
        instance = this;
    }

    void Update()
    {
        shield.transform.Rotate(0, 0, Time.deltaTime * 10);
    }
}
