using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotPatrolBehaviour : MonoBehaviour {

	public NavMeshAgent agent;
	private Transform[] waypoints;
	private int currentWaypointInt;


	void Start()
	{	
		GetWaypoints();
	}

	void GetWaypoints()
	{
		waypoints = GameObject.Find("Waypoints").GetComponent<WaypointManager>().waypoints;
		currentWaypointInt = -1;
		SetNextWaypoint();
	}

	void Update()
	{
		if(agent.remainingDistance <= agent.stoppingDistance)
		{
			SetNextWaypoint();
		}
	}

	void SetNextWaypoint()
	{
		currentWaypointInt += 1;
		if(currentWaypointInt >= waypoints.Length)
		{
			currentWaypointInt = 0;
		}

		agent.SetDestination(waypoints[currentWaypointInt].position);
	}

}
