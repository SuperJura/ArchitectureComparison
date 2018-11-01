using UnityEngine;

public class PCInputDevice : IInput
{
    float fireCooldown = 0;

    public bool getFire()
    {
        if(fireCooldown <= 0) return Input.GetMouseButtonDown(0);
        return false;
    }

    public float getHorizontal()
    {
        if(Input.GetKey(KeyCode.A)) return -1;
        if(Input.GetKey(KeyCode.D)) return 1;

        return 0;
    }

    public float getVertical()
    {
        if(Input.GetKey(KeyCode.W)) return -1;
        if(Input.GetKey(KeyCode.S)) return 1;

        return 0;
    }

    public void updateDevice()
    {
        if(fireCooldown > 0) fireCooldown -= Time.deltaTime;
    }

    public bool getBoost()
    {
        return Input.GetKey(KeyCode.Space);
    }
}