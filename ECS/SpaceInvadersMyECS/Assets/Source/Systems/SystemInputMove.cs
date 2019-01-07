using UnityEngine;
using static EntityManager;

public class SystemInputMove : ISystem
{
    Component[] filter = new Component[]
    {
        new ComponentTransform(),
        new ComponentMoveToInput()
    };

    public void update()
    {
        var entities = getEntities(filter);
        for (int i = 0; i < entities.Count; i++)
        {
            var components = getComponents(entities[i]);
            var transform = getComponent<ComponentTransform>(components);
            var moveStats = getComponent<ComponentMoveToInput>(components);
            var moveForward = getComponent<ComponentMoveForward>(components);

            Vector3 moveDir = transform.transform.forward;
            Quaternion rotation = transform.transform.rotation;

            float rotationSpeed = 2;
            Vector3 newEulerRotation = Vector3.zero;
#if UNITY_ANDROID && !UNITY_EDITOR
            if(Input.touchCount == 0)
            {
                moveStats.startTouchPosition = moveStats.currentTouchPosition = Vector3.zero;
                return;
            }
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                moveStats.startTouchPosition = touch.position;
            }
            moveStats.currentTouchPosition = touch.position;

            float horizontal = (moveStats.currentTouchPosition - moveStats.startTouchPosition).x * Time.deltaTime;
            horizontal = Mathf.Clamp(horizontal, -1, 1);
            
            float vertical = (moveStats.currentTouchPosition - moveStats.startTouchPosition).y * Time.deltaTime;
            vertical = Mathf.Clamp(vertical, -1, 1);

            newEulerRotation.x += vertical * moveStats.agility * -Time.deltaTime;
            newEulerRotation.y += horizontal * moveStats.agility * Time.deltaTime;
#else
            if (Input.GetKey(KeyCode.A)) { newEulerRotation.y -= rotationSpeed * moveStats.agility * Time.deltaTime; }
            if (Input.GetKey(KeyCode.D)) { newEulerRotation.y += rotationSpeed * moveStats.agility * Time.deltaTime; }
            if (Input.GetKey(KeyCode.W)) { newEulerRotation.x -= rotationSpeed * moveStats.agility * Time.deltaTime; }
            if (Input.GetKey(KeyCode.S)) { newEulerRotation.x += rotationSpeed * moveStats.agility * Time.deltaTime; }
#endif

            rotation = rotation * Quaternion.AngleAxis(newEulerRotation.y, Vector3.up);
            rotation = rotation * Quaternion.AngleAxis(newEulerRotation.x, Vector3.right);

            transform.transform.rotation = rotation;

            bool boost = Input.GetKey(KeyCode.Space);
            moveForward.speed = boost ? 500 : 50;
        }
    }
}