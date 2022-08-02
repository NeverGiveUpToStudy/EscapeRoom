using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour {

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform[] waypoints;

    private int m_CurrentWaypointIndex;

    private void Start() {
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    private void Update() {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance) {
            m_CurrentWaypointIndex = (waypoints.Length + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }
}
