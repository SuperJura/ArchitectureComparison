using UnityEngine;

public class TouchInputDevice : IInput
{
    private Vector3 startTouchPosition;
    private Vector3 currentTouchPosition;

    public bool getBoost()
    {
        return false;
    }

    public bool getFire()
    {
        return Input.touchCount >= 2;
    }

    public float getHorizontal()
    {
        float amount = (currentTouchPosition - startTouchPosition).x * Time.deltaTime;
        return Mathf.Clamp(amount, -1, 1);
    }

    public float getVertical()
    {
        float amount = (currentTouchPosition - startTouchPosition).y * -Time.deltaTime;
        return Mathf.Clamp(amount, -1, 1);
    }

    public void updateDevice()
    {
        if(Input.touchCount == 0)
        {
            startTouchPosition = currentTouchPosition = Vector3.zero;
            return;
        }
        Touch touch = Input.GetTouch(0);
        if(touch.phase == TouchPhase.Began)
        {
            startTouchPosition = touch.position;
        }
        currentTouchPosition = touch.position;
    }
}