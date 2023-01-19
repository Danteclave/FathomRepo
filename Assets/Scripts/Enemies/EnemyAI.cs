using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    //private float coneAngle = Mathf.Deg2Rad * 60;
    private float coneAngle = 60;
    protected void UpdateSeeing()
    {
        playerInSightRange = false;
        playerInAttackRange = false;
        /*Collider[] cols = Physics.OverlapSphere(transform.position, sightRange, whatIsPlayer.value);
        cols = cols.Where(e => e.gameObject.CompareTag("Player")).ToArray();
        if(cols.Length > 0)
        {
            //atan (y/x)
            var projected = (player.position - transform.position);
            projected.y = 0;
            //projected = transform.TransformDirection(projected);
            var fwd = transform.forward;
            fwd.y = 0;
            //var pangle = Mathf.Atan2(projected.z, projected.x);
            var pangle = Vector2.Angle(projected, transform.forward);
            //var langle = Mathf.Atan2(fwd.z, fwd.x) + coneAngle;
            //var rangle = langle - 2 * coneAngle;
            //if (langle >= pangle && pangle >= rangle)
            {
                Debug.Log("co jes");
                for (int i = -45; i <= 45; i++)
                {
                    Physics.Raycast(transform.position,
                        //new Vector3(-Mathf.Cos(pangle + i * Mathf.Deg2Rad), 0, Mathf.Sin(pangle + i * Mathf.Deg2Rad)),
                        Quaternion.Euler(0, i, 0) * fwd,
                        out RaycastHit hit, 999, ~LayerMask.NameToLayer("Ignore Raycast"));
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.CompareTag("Player"))
                        {
                            playerInSightRange = hit.distance <= sightRange;
                            playerInAttackRange = hit.distance <= attackRange;
                        }
                    }
                }
            }
        }*/
        Collider col = Physics.OverlapSphere(transform.position, sightRange, whatIsPlayer.value).FirstOrDefault();
        if(col == null)
        {
            return;
        }

        for(int i = (int)(-coneAngle/2); i<=coneAngle/2; i++)
        {
            Physics.Raycast(transform.position+new Vector3(0, (float)(GetComponent<CapsuleCollider>().height * 0.5),0), Quaternion.Euler(0, i, 0) * transform.forward, out RaycastHit hit, 999, ~LayerMask.NameToLayer("Ignore Raycast"));
            if(hit.collider != null && hit.collider.gameObject.GetComponent<PlayerController>() != null)
            {
                playerInSightRange = hit.distance <= sightRange;
                playerInAttackRange = hit.distance <= attackRange;
            }
        }
    }

    private Animator animator;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if (agent.velocity.magnitude >= 0.1f)
        {
            animator.SetTrigger("Walk");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
        UpdateSeeing();

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

    protected private void AttackPlayer()
    {
        //agent.SetDestination(transform.position);
        //transform.LookAt(player); 

        if (!alreadyAttacked)
        {
            animator.SetTrigger(Random.Range(0, 100) >= 50 ? "Attack01" : "Attack02");
            GameEventSystem.Instance.PlayerGetDamage(1);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    protected private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    public enum State
    {
        Idle, // standing still
        Patrol, // moving between points in order
        Wandering, // doing completely random shit
        Hunting, // actively looking for player
        Chase, // chasing the player
        Search, // searching area
        Script // following a scripted path or doing scripted shit
    }
    public State state = State.Patrol;

    protected void Idle()
    {
        // transition table not present here intentionally
    }


    const float patrolThreshold = 1.5f;
    public List<Transform> patrolNodes = new List<Transform>();
    private int _patrolNodeIndex = 0;
    public int PatrolNodeIndex
    {
        get
        {
            if (Vector3.Distance(transform.position, patrolNodes[_patrolNodeIndex].position) <= patrolThreshold)
            {
                _patrolNodeIndex = (_patrolNodeIndex+1) % patrolNodes.Count;
            }
            return _patrolNodeIndex;
        }
        set => _patrolNodeIndex = value;
    }

    protected void Patrol()
    {
        if (patrolNodes.Count <= 0)
        {
            state = State.Wandering;
            return;
        }
        agent.SetDestination(patrolNodes[PatrolNodeIndex].position);
        // transition table
        if (playerInSightRange)
        {
            state = State.Chase;
            StartCoroutine(soundpeepeepoopoo());
        }
    }

    protected void Wandering()
    {
        if (!walkPointSet)
        {
            walkPoint = new Vector3(transform.position.x + 15*Random.Range(-walkPointRange, walkPointRange),
                                    transform.position.y,
                                    transform.position.z + 15 * Random.Range(-walkPointRange, walkPointRange));
            if (Physics.Raycast(walkPoint, -transform.up, 10f, whatIsGround))
            {
                walkPointSet = true;
            }
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }

        if(agent.velocity.magnitude <= 0.01f)
        {
            walkPointSet = false;
        }

        // transition table
        if (playerInSightRange)
        {
            state = State.Chase;
            StartCoroutine(soundpeepeepoopoo());
        }
    }

    public float huntingTime = 0.0f;
    public float maxHunt = 5.0f;
    protected void Hunting()
    {
        huntingTime -= Time.deltaTime;
        
        // transition table
        if (playerInSightRange)
        {
            state = State.Chase;
            StartCoroutine(soundpeepeepoopoo());
        }
        else if (huntingTime <= 0.0f || Vector3.Distance(agent.destination, transform.position) <= walkPointRange)
        {
            FindObjectOfType<SoundManager>().Stop(player.gameObject, "chase");
            EnterSearch();
        }
    }


    public float memoryTime = 2.0f;
    private float outsideSightChaseTime = 0;

    protected void Chase()
    {
        if (playerInSightRange)
        {
            agent.SetDestination(player.position);
            outsideSightChaseTime = memoryTime;
        }
        else
        {
            outsideSightChaseTime -= Time.deltaTime;
        }

        if (playerInAttackRange)
        {
            AttackPlayer();
        }

        // transition table
        if (outsideSightChaseTime <= 0) EnterHunting();
    }

    protected Vector3 searchCenter;
    public float searchRange = 10.0f;
    protected Vector3 currentSelection;

    protected void PickSearchSpot()
    {
        for(int i=0; i<1000; i++)
        {
            currentSelection = new Vector3(
                searchCenter.x + 15 * Random.Range(-searchRange, searchRange),
                searchCenter.y /*+ (float)Random.NextDouble() * searchRange*/,
                searchCenter.z + 15 * Random.Range(-searchRange, searchRange)
                );
            if (Physics.Raycast(currentSelection, -transform.up, 10f, whatIsGround)) return;
        }
    }

    protected void EnterSearch()
    {
        state = State.Search;
        searchCenter = transform.position;
        PickSearchSpot();
        searchTimer = maxSearch;
    }

    protected float searchTimer = 0.0f;
    protected float maxSearch = 5.0f;
    protected void Search()
    {
        searchTimer -= Time.deltaTime;

        if(Vector3.Distance(transform.position, currentSelection) <= walkPointRange || agent.velocity.magnitude <= 0.1f)
        {
            PickSearchSpot();
            agent.SetDestination(currentSelection);
        }

        // transition table
        if (playerInSightRange)
        {
            Chase();
        }
        else if (searchTimer <= 0.0)
        {
            state = State.Patrol;
        }
    }

    protected virtual void Script()
    {
        // broken shit don't even touch this

        // transition table not present here (yet?)
    }


    protected void EnterHunting()
    {
        huntingTime = maxHunt;
        state = State.Hunting;
        agent.SetDestination(player.position);
        
    }
    private IEnumerator soundpeepeepoopoo()
    {
        FindObjectOfType<SoundManager>().PlayMusic("alerted");
        yield return new WaitForSeconds(2);
        if(state == State.Chase || state == State.Hunting)
        {
            FindObjectOfType<SoundManager>().PlayMusic("chase");
        }
    } 

}
