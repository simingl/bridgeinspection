using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaypointCluster : MonoBehaviour {
    public GameObject nevCubePrefab;

    [HideInInspector] public static List<List<WayPoint>> waypoints = new List<List<WayPoint>>(); /**< List of ALL waypoints inside the cluster */
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
    }

    private void loadRoute()
    {


        //51880 
        string r50368 = "0,3,5,7,4,8,10,14,12,8,7,24,25,22,20,17,23,25,29,26,22,24,21,25,26,23,20,19,18,21,19,17,18,18,1,5,2,1,18,22,19,23,23,26,28,24,27,31,33,30,30,33,32,31,28,32,29,31,30,28,25,27,30,13,10,7,11,13,16,14,13,13,16,15,11,14,15,12,9,11,8,9,5,8,6,2,0,1,4,2,3,6,9";

        //2-robots 28234 
        //Robot - 83 =>2,0,0,2,1,21,24,24,7,11,11,8,6,6,2,5,0,5,9,6,2,4,7,7,24,25,26,23,19,19,20,23,25,21,21,18,19,19,20,22,26,29,25,25,28,30,27,31,28,32,29,31,31,28,26,26,25,27,24,22,18,1,5,7,7,4,8
        //Robot - 84 =>3,0,0,3,6,6,9,8,5,3,2,2,0,1,4,18,21,19,17,18,18,17,20,19,22,25,28,24,7,8,10,10,13,14,16,15,12,8,11,15,14,10,7,7,10,13,11,14,14,11,9,12,14,14,16,13,30,31,32,33,31,31,33,30
        string r2_28234_1 = "0,2,1,21,24,7,11,8,6,2,5,0,5,9,6,2,4,7,24,25,26,23,19,20,23,25,21,18,19,20,22,26,29,25,28,30,27,31,28,32,29,31,28,26,25,27,24,22,18,1,5,7,4,8";
        string r2_28234_2 = "0,3,6,9,8,5,3,2,0,1,4,18,21,19,17,18,17,20,19,22,25,28,24,7,8,10,13,14,16,15,12,8,11,15,14,10,7,10,13,11,14,11,9,12,14,16,13,30,31,32,33,31,33,30";

        string r2_27446_1 = "18,21,19,20,22,26,25,29,31,33,32,31,33,30,31,29,32,28,31,27,30,13,10,4,1,5,3,0,2,6,8,11,9,6,3,2,5,8,14,10,7,24,25,21,24,28,25,23,19,22,18,17";
        string r2_27446_2 = "7,8,2,4,9,5,8,9,8,4,7,11,13,14,15,12,14,15,16,14,11,15,16,13,30,28,26,29,26,23,25,27,24,7,5,2,1,0,2,6,9,12,8,10,24,22,25,23,20,17,19,18,1";
        //27446(E1014)(E710)(E724)(E2425)(E2125)(E2124)(E2428)(E2528)(E1923)(E1922)(E1822)(E1718)Robot - 84 =>(E78)(E24)(E59)(E89)(E48)(E47)(E711)(E1113)(E1314)(E1215)(E1214)(E1415)(E1516)(E1416)(E1114)(E1115)(E1316)(E2830)(E2628)(E2629)(E2326)(E2527)(E2427)(E57)(E12)(E01)(E02)(E69)(E912)(E812)(E810)(E2224)(E2225)(E2325)(E2023)(E1720)(E1719)(E1819)(E118)Robot - 83 =>(E1821)(E1921)(E1920)(E2022)(E2226)(E2526)(E2529)(E3133)(E3233)(E3132)(E3033)(E3031)(E2931)(E2932)(E2832)(E2831)(E2731)(E2730)(E1330)(E1013)(E14)(E15)(E35)(E03)(E26)(E68)(E811)(E911)(E36)(E23)(E25)(E58)(E1014)
        //27446 
        //
        //Robot - 84 =>7,8,2,4,9,5,8,9,8,4,7,11,13,14,15,12,14,15,16,14,11,15,16,13,30,28,26,29,26,23,25,27,24,7,5,2,1,0,2,6,9,12,8,10,24,22,25,23,20,17,19,18,1,
        //Robot - 83 =>18,21,19,20,22,26,25,29,31,33,32,31,33,30,31,29,32,28,31,27,30,13,10,4,1,5,3,0,2,6,8,11,9,6,3,2,5,8,14,10,7,24,25,21,24,28,25,23,19,22,18,17,

               List<string> wpstr1 = r2_27446_1.Split(',').ToList();
        WayPoint[] wps = this.GetComponentsInChildren<WayPoint>();
        List<WayPoint> route1 = new List<WayPoint>();
        foreach (string str in wpstr1) {
            int point = int.Parse(str);
            WayPoint wp = wps.First(x => x.name == point.ToString());
            route1.Add(wp);
        }
        waypoints.Add(route1);

        List<string> wpstr2 = r2_27446_2.Split(',').ToList();
        List<WayPoint> route2 = new List<WayPoint>();
        foreach (string str in wpstr2)
        {
            int point = int.Parse(str);
            WayPoint wp = wps.First(x => x.name == point.ToString());
            route2.Add(wp);
        }
        waypoints.Add(route2);

        //create prefab for moving cubes each with random color
        foreach (List<WayPoint> route in waypoints)
        {
            Color randCol = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            GameObject go = Instantiate(nevCubePrefab, route[0].transform.position, Quaternion.identity);
            go.GetComponent<predefinedPath>().route = route.ToArray();
            go.GetComponent<MeshRenderer>().material.color = randCol;
            go.GetComponent<TrailRenderer>().material.color = randCol;
        }
    }
    private void Update()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            List<WayPoint> route = waypoints[i];
            for (int j = 0; j < route.Count - 1; j++)
            {
                DrawArrow.ForDebug(route[j].transform.position, route[j + 1].transform.position - route[j].transform.position, i==0?Color.blue:Color.red);
            }
        }
    }


	/* Creates a link between source and destiny */
	public static void link(WayPoint source, WayPoint destiny) {
		source.addOutWayPoint(destiny);
	}
}
