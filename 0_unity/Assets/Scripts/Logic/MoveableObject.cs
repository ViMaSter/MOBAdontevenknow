using UnityEngine;
using System.Collections;

public class MoveableObject : MonoBehaviour
{
    public float MaxMovementSpeed = 1f;

    private Vector2 currentTarget = Vector2.zero;
    public Vector2 CurrentTarget
    {
        get
        {
            return currentTarget;
        }
        set {
            currentTarget = value;
        }
    }

    void Update()
    {
        Vector2 newPosition = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.z), currentTarget, MaxMovementSpeed);
        transform.position = new Vector3(
            newPosition.x,
            transform.position.y,
            newPosition.y
        );
    }
}
