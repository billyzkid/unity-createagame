﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    private NavMeshAgent pathfinder;
    private Transform target;

    protected override void Start()
    {
        base.Start();

        pathfinder = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
    }

    private IEnumerator UpdatePath()
    {
        const float refreshRate = 0.25f;

        while (target != null)
        {
            var targetPosition = new Vector3(target.position.x, 0, target.position.z);

            if (!dead)
            {
                pathfinder.SetDestination(targetPosition);
            }

            yield return new WaitForSeconds(refreshRate);
        }
    }
}