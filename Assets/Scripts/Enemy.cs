using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : LivingEntity
{
    private NavMeshAgent pathfinder;
    private Material skinMaterial;
    private Color originalColor;
    private Transform target;
    private State currentState;

    private float myCollisionRadius;
    private float targetCollisionRadius;

    private float attackDistanceThreshold = 0.5f;
    private float timeBetweenAttacks = 1;
    private float nextAttackTime;

    protected override void Start()
    {
        base.Start();
    
        pathfinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = State.Chasing;

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (Time.time > nextAttackTime)
        {
            var sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

            if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
            {
                nextAttackTime = Time.time + timeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathfinder.enabled = false;

        var originalPosition = transform.position;
        var dirToTarget = (target.position - transform.position).normalized;
        var attackPosition = target.position - dirToTarget * myCollisionRadius;
        var attackSpeed = 3f;
        var percent = 0f;

        skinMaterial.color = Color.red;

        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;

            var interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    private IEnumerator UpdatePath()
    {
        const float refreshRate = 0.25f;

        while (target != null)
        {
            if (currentState == State.Chasing)
            {
                var dirToTarget = (target.position - transform.position).normalized;
                var targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);

                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }

            yield return new WaitForSeconds(refreshRate);
        }
    }

    private enum State
    {
        Idle,
        Chasing,
        Attacking
    }
}