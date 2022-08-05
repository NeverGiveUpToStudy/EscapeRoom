using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour {

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform[] waypoints;//幽灵自动寻路点

    private int m_CurrentWaypointIndex = 0;

    private void Start() {
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    private void Update() {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance) {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }
}
