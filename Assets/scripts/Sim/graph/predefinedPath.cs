using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class predefinedPath : MonoBehaviour {
    public WayPoint[] route = null;

    public float speed = 1f;
    private Transform t;
    public int totalDistance = 0;

    /* Initialization */
    void Start ()
    {
        t = this.gameObject.transform;
    }

    /* Move the square */
    int i = 0;
	void Update () {
        if (route == null || i >= route.Length) return;
        //Distance between the square and the current target waypoint
        float distance = Vector3.Distance(t.position, route[i].transform.position);
        //Move the square towards the current target
        t.position = Vector3.Lerp(t.position, route[i].transform.position, speed * Time.deltaTime / distance);
        //In case the square arrived to the target waypoint (very small distance)
        if (distance < 0.1)
        {
            //Change the current target to the next defined waypoint
            if (i > 0)
            {
                int previous = int.Parse(route[i - 1].name);
                int current = int.Parse(route[i].name);
                this.totalDistance += route[i].getParent().getDistance(previous, current);
            }
            i++;            
        }
	}

}
