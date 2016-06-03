using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;

    private float speed = 10;
    private float damage = 1;

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private void Update()
    {
        var moveDistance = speed * Time.deltaTime;

        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    private void CheckCollisions(float moveDistance)
    {
        var ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    private void OnHitObject(RaycastHit hit)
    {
        //print(hit.collider.gameObject.name);

        var damageableObject = hit.collider.GetComponent<IDamageable>();

        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }

        GameObject.Destroy(gameObject);
    }
}