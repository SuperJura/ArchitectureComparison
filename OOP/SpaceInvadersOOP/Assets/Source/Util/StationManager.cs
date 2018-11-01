using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    GameObject shield;
    Vector2 offset = Vector2.zero;
    // Start is called before the first frame update
    void Awake()
    {
        shield= transform.Find("Shield").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        shield.transform.Rotate(0, 0, Time.deltaTime * 10);
    }
}
