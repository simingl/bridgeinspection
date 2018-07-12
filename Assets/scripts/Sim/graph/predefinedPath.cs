using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class predefinedPath : MonoBehaviour {

    private WayPoint[] route = null;

    public float speed = 1f;
    private Transform t;

    /* Initialization */
    void Start ()
    {
        t = this.gameObject.transform;
        route = WaypointCluster.waypoints.ToArray();
    }

    /* Move the square */
    int i = 0;
	void Update () {
        //leave in case we reached our destination
        if (route == null || route.Length <= 0) {
            route = WaypointCluster.waypoints.ToArray();
        }
        if (i >= route.Length) return;
        //Distance between the square and the current target waypoint
        float distance = Vector3.Distance(t.position, route[i].transform.position);
        //Move the square towards the current target
        t.position = Vector3.Lerp(t.position, route[i].transform.position, speed * Time.deltaTime / distance);
        //In case the square arrived to the target waypoint (very small distance)
        if (distance < 0.1)
            //Change the current target to the next defined waypoint
            i++;
	}

}
