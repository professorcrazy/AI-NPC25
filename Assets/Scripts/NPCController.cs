using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{

    private NavMeshAgent agent;
    public Transform target;
    [SerializeField] float viewAngle = 55f;
    [SerializeField] float visionRange = 25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target == null)
        {
            return;
        }

        Debug.DrawLine(transform.position, target.position, Color.blue);
        Vector3 rightSide = Quaternion.AngleAxis(viewAngle, Vector3.up) * transform.forward;
        Vector3 leftSide = Quaternion.AngleAxis(-viewAngle, Vector3.up) * transform.forward;
        Debug.DrawLine(transform.position, transform.position + (rightSide * visionRange), Color.red);
        Debug.DrawLine(transform.position, transform.position + (leftSide * visionRange), Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, target.position-transform.position, out hit, visionRange))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform == target){
                Debug.Log("Target in range!");
                if (Vector3.Dot((target.position - transform.position), transform.forward) >= Mathf.Cos(viewAngle)){
                    Debug.Log("Target in sight!");
                    agent.SetDestination(target.position);
                }
            }
        }

        //add vision
    }
}
