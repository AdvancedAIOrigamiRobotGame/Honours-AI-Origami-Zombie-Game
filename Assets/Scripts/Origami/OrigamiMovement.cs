using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class OrigamiMovement : MonoBehaviour
{
    [SerializeField]
   // private Camera Camera = null;
    //[SerializeField]
    //private LayerMask LayerMask;
    //[SerializeField]
    [Range(0, 100)] public float speed;
    [Range(0, 500)] public float radius;
    //private Vector3 CameraOffset;
    private NavMeshAgent Agent;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        if (Agent != null)
        {
            Agent.speed = speed;
            Agent.SetDestination(RandomNavMeshLoaction());
        }
    }

    private void Update()
    {
        /**if (Application.isFocused && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Ray ray = Camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask))
            {
                Agent.SetDestination(hit.point);
            }
        }*/
        if (Agent != null && Agent.remainingDistance <= Agent.stoppingDistance) 
        {
            Agent.SetDestination(RandomNavMeshLoaction());
        }
    }

   /** private void LateUpdate()
    {
        Camera.transform.position = transform.position + CameraOffset;
    }*/
    public Vector3 RandomNavMeshLoaction()
    {
        Vector3 finalposition = Vector3.zero;
        Vector3 randomposition = Random.insideUnitSphere * radius;
        randomposition += transform.position;
        if (NavMesh.SamplePosition(randomposition, out NavMeshHit hit, radius, 1))
        {
            finalposition = hit.position;
        }
        return finalposition;   
    }
}
