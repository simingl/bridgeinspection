using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaypointCluster : MonoBehaviour {
	[HideInInspector] public static List<WayPoint> waypoints = new List<WayPoint>(); /**< List of ALL waypoints inside the cluster */
    [HideInInspector] private List<int> route = new List<int>(); /**< List of ALL waypoints inside the cluster */
    [HideInInspector]
    public GameObject cluster = null;		 /**< Cluster object where the waypints will be added */
    private uint currentID = 0;              /**< Numeric id assigned to the waypoint */

    private GameObject movingCube = null;

    void Start()
    {
        WayPoint[] wps = this.GetComponentsInChildren<WayPoint>();
        waypoints.Clear();
        loadRoute();
        foreach (int wpi in route) {
            WayPoint wp = wps.First(x => x.name == wpi.ToString());
            waypoints.Add(wp);
        }
    }

    private void loadRoute()
    {
        //51880 
        //51880(E03)(E35)(E59)(E912)(E812)(E68)(E36)(E23)(E26)(E69)(E89)(E78)(E710)(E1013)(E1330)(E1113)(E911)(E1115)(E1516)(E1416)(E1214)(E1215)(E1415)(E1316)(E1314)(E1114)(E711)(E15)(E58)(E811)(E1014)(E810)(E48)(E24)(E02)(E01)(E118)(E2125)(E2325)(E2023)(E1920)(E1922)(E1822)(E1821)(E1921)(E1719)(E1718)(E1819)(E1923)(E2326)(E2830)(E2730)(E2731)(E3133)(E3132)(E3233)(E3033)(E3031)(E2428)(E2427)(E2527)(E2529)(E2629)(E2628)(E2832)(E2932)(E2931)(E2831)(E2528)(E2425)(E2124)(E57)(E25)(E12)(E14)(E47)(E724)(E2224)(E2226)(E2526)(E2225)(E2022)(E1720)(E03)
        //0,3,5,9,12,8,6,3,2,6,9,8,7,10,13,30,13,11,9,11,15,16,14,12,15,14,16,13,14,11,7,1,5,8,11,14,10,8,4,2,0,1,18,21,25,23,20,19,22,18,21,19,17,18,19,23,26,28,30,27,31,33,31,32,33,30,31,28,24,27,25,29,26,28,32,29,31,28,25,24,21,7,5,2,1,4,7,24,22,26,25,22,20,17
        string routestr= "0,3,5,9,12,8,6,3,2,6,9,8,7,10,13,30,13,11,9,11,15,16,14,12,15,14,16,13,14,11,7,1,5,8,11,14,10,8,4,2,0,1,18,21,25,23,20,19,22,18,21,19,17,18,19,23,26,28,30,27,31,33,31,32,33,30,31,28,24,27,25,29,26,28,32,29,31,28,25,24,21,7,5,2,1,4,7,24,22,26,25,22,20,17";
        List<string> wpstr = routestr.Split(',').ToList();
        WayPoint[] wps = this.GetComponentsInChildren<WayPoint>();
        foreach (string str in wpstr) {
            int point = int.Parse(str);
            WayPoint wp = wps.First(x => x.name == point.ToString());
            waypoints.Add(wp);
        }
    }
    private void Update()
    {
        for(int i=0;i< waypoints.Count -1; i++) {
            DrawArrow.ForDebug(waypoints[i].transform.position, waypoints[i + 1].transform.position - waypoints[i].transform.position, Color.blue);
        }
    }


    /** Removes all instances of a waypoint */
    public void remove(WayPoint w) {
		waypoints.Remove(w);
	}

	/*Creates a new waypoint*/
	public WayPoint CreateWaypoint(Vector3 point) {
		GameObject waypointAux = Resources.Load("Waypoint")  as GameObject;
		GameObject waypointInstance = Instantiate(waypointAux) as GameObject;
		waypointInstance.transform.position = point;
		waypointInstance.transform.parent = cluster.transform;
        waypointInstance.name = currentID.ToString();
        ++currentID;
		waypoints.Add(waypointInstance.GetComponent<WayPoint>());
		waypointInstance.GetComponent<WayPoint>().setParent(this);
		return waypointInstance.GetComponent<WayPoint>();
	}
	
	/* Creates a link between source and destiny */
	public static void link(WayPoint source, WayPoint destiny) {
		source.addOutWayPoint(destiny);
	}
}
