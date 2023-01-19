using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2AI : EnemyAI
{
    protected override void Start()
    {
        base.Start();
        fuelTank = FindObjectOfType<FuelTank>();
    }

    FuelTank fuelTank = default!;

    protected override void Update()
    {
        UpdateSeeing();

        if(fuelTank != null)
        {
            if(fuelTank.touchingRefuellingStation)
            {
                state = State.Script;
            }
        }

        switch (state)
        {
            case State.Idle: Idle(); break;
            case State.Patrol: Patrol(); break;
            case State.Wandering: Wandering(); break;
            case State.Hunting: Hunting(); break;
            case State.Chase: Chase(); break;
            case State.Search: Search(); break;
            case State.Script: Script(); break;
        }
    }

    protected override void Script()
    {
        base.Script();
        if (playerInAttackRange)
        {
            AttackPlayer();
        }
        agent.SetDestination(fuelTank.transform.position);
        if (!fuelTank.touchingRefuellingStation)
        {
            state = State.Patrol;
            agent.SetDestination(patrolNodes[PatrolNodeIndex].transform.position);
        }
        if (agent.remainingDistance <= 0.5f)
        {
            fuelTank.GetComponent<Rigidbody>().velocity = new Vector3(10, 5, -20);
            state = State.Patrol;
            agent.SetDestination(patrolNodes[PatrolNodeIndex].transform.position);
        }
    }
}
