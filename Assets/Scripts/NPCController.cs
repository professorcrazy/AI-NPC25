using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{

    private NavMeshAgent agent;
    public Transform target;
    [SerializeField] float viewAngle = 55f;
    [SerializeField] float visionRange = 25f;
    [SerializeField] float attackrange = 3f;
    [SerializeField] int curWaypointIndex = 0;
    [SerializeField] Transform[] waypoints;

    enum State
    {
        Patrolling,
        Chasing,
        Searching,
        Attacking,
        Dying,
        Dead
    }

    [SerializeField] State currentState = State.Patrolling;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Patrol();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentState == State.Dying)
        {
            agent.isStopped = true;
            currentState = State.Dead;
            Destroy(gameObject, 2f);
        }
        if (target == null)
        {
            return;
        }

        if (currentState == State.Patrolling)
        {
            Patrol();
            if (LookForPlayer())
            {
                currentState = State.Chasing;
            }
        }
        if (currentState == State.Chasing)
        {
            if (!LookForPlayer())
            {
                currentState = State.Patrolling;
            }
        }

    }

    void Patrol()
    {
        if (waypoints.Length == 0)
        {
            Debug.Log("No waypoints set");
            return;
        }
        if (!agent.hasPath)
        {
            Debug.Log("Setting new destination");
            agent.SetDestination(waypoints[curWaypointIndex].position);
        }
        if (agent.remainingDistance < 0.25f)
        {
            Debug.Log("Reached waypoint, setting next");
            
            curWaypointIndex = (curWaypointIndex + 1) % waypoints.Length;
//            curWaypointIndex = Random.Range(0, waypoints.Length);
            agent.SetDestination(waypoints[curWaypointIndex].position);
        }
    }

    bool LookForPlayer()
    {
        Debug.DrawLine(transform.position, target.position, Color.blue);
        Vector3 rightSide = Quaternion.AngleAxis(viewAngle, Vector3.up) * transform.forward;
        Vector3 leftSide = Quaternion.AngleAxis(-viewAngle, Vector3.up) * transform.forward;
        Debug.DrawLine(transform.position, transform.position + (rightSide * visionRange), Color.red);
        Debug.DrawLine(transform.position, transform.position + (leftSide * visionRange), Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, target.position - transform.position, out hit, visionRange))
        {
            if (hit.transform == target)
            {
                Debug.Log("Target in range!");
                if (Vector3.Dot((target.position - transform.position), transform.forward) >= Mathf.Cos(viewAngle))
                {
                    Debug.Log("Target in sight!");
                    agent.SetDestination(target.position);
                    
                    if (agent.remainingDistance < attackrange)
                    {
                        Debug.Log("Attack!");
                    }

                    return true;
                }
            }
        }
        return false;
    }

}
