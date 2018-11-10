using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionRule : MonoBehaviour
{
    public CollisionTag tag;

    void OnCollisionEnter(Collision collision)
    {
        CollisionRule other = collision.transform.GetComponent<CollisionRule>();
        submitCollision(this, other);        
    }

    void OnTriggerEnter(Collider collider)
    {
        CollisionRule other = collider.transform.GetComponent<CollisionRule>();
        submitCollision(this, other);
    }

    void submitCollision(CollisionRule first, CollisionRule second)
    {
        if(first == null || second == null) return;
        if(tag == CollisionTag.Enemy) return;

        if(isPair(CollisionTag.Enemy, CollisionTag.Bullet, first, second))
        {
            GameObject.Destroy(getFromPair(CollisionTag.Enemy, first, second).transform.parent.gameObject);
            Game.instance.onEnemyDestroy();
        }
    }

    bool isPair(CollisionTag one, CollisionTag two, CollisionRule first, CollisionRule second)
    {
        return (
            first.tag == one && second.tag == two ||
            second.tag == one && first.tag == two
        );
    }

    CollisionRule getFromPair(CollisionTag tag, CollisionRule first, CollisionRule second)
    {
        if(first.tag == tag) return first;
        if(second.tag == tag) return second;
        return null;
    }
}

public enum CollisionTag
{
    Enemy,
    Bullet
}