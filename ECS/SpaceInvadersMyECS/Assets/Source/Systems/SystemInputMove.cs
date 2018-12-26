using UnityEngine;

public class SystemInputMove : ISystem
{
    Component[] filter = new Component[]
    {
        new ComponentsTransform(),
        new ComponentMoveToInput()
    };

    public void update()
    {
        var entities = EntityManager.getEntities(filter);
        for (int i = 0; i < entities.Count; i++)
        {
            var components = EntityManager.getComponents(entities[i]);
            var transform = EntityManager.GetComponent<ComponentsTransform>(components);
            var moveStats = EntityManager.GetComponent<ComponentMoveToInput>(components);
            var moveForward = EntityManager.GetComponent<ComponentMoveForward>(components);

            Vector3 moveDir = transform.transform.forward;
            Quaternion rotation = transform.transform.rotation;

            float rotationSpeed = 2;
            Vector3 newEulerRotation = Vector3.zero;
            if (Input.GetKey(KeyCode.A)) { newEulerRotation.y -= rotationSpeed * moveStats.agility * Time.deltaTime; }
            if (Input.GetKey(KeyCode.D)) { newEulerRotation.y += rotationSpeed * moveStats.agility * Time.deltaTime; }
            if (Input.GetKey(KeyCode.W)) { newEulerRotation.x -= rotationSpeed * moveStats.agility * Time.deltaTime; }
            if (Input.GetKey(KeyCode.S)) { newEulerRotation.x += rotationSpeed * moveStats.agility * Time.deltaTime; }

            rotation = rotation * Quaternion.AngleAxis(newEulerRotation.y, Vector3.up);
            rotation = rotation * Quaternion.AngleAxis(newEulerRotation.x, Vector3.right);

            transform.transform.rotation = rotation;

            bool boost = Input.GetKey(KeyCode.Space);
            moveForward.speed = boost ? 500 : 50;
        }
    }
}