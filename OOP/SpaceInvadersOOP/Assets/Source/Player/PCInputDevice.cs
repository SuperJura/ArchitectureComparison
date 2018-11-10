using UnityEngine;

public class PCInputDevice : IInput
{
    public bool getFire()
    {
        return Input.GetMouseButton(0);
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
    }

    public bool getBoost()
    {
        return Input.GetKey(KeyCode.Space);
    }
}