using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class WaypointCluster : MonoBehaviour {
    public GameObject nevCubePrefab;

    [HideInInspector] public static List<List<WayPoint>> waypoints = new List<List<WayPoint>>(); /**< List of ALL waypoints inside the cluster */
    [HideInInspector] private List<int> route = new List<int>(); /**< List of ALL waypoints inside the cluster */
    [HideInInspector]
    public GameObject cluster = null;		 /**< Cluster object where the waypints will be added */

    private List<string> routesFromGA = new List<string>();

    private const int NUMBER_POINTS = 34;
    private const int NUMBER_EDGES = 83;
    private const string graphfile = "graph-raw.csv";
    List<List<int>> graph = new List<List<int>>();

    void Start()
    {
        loadGraph();
        WayPoint[] wps = this.GetComponentsInChildren<WayPoint>();
        foreach (WayPoint wp in wps) {
            wp.setParent(this);
        }
        waypoints.Clear();
        loadRoute();
    }

    private void loadGraph()
    {
        using (var reader = new StreamReader(@"C:\Users\simingl\Google Drive\0Research\BridgeInspection\cigarW\graph-raw.csv"))
        {

            while (!reader.EndOfStream) { 
                var line = reader.ReadLine();
                string[] values = line.Split(',');
                List<int> row = new List<int>();
                for (int c = 0; c < values.Length; c++) {
                    row.Add(int.Parse(values[c]));
                }
                graph.Add(row);
            }
        }
    }

    public int getDistance(int start, int end) {
        return graph[start][end];
    }

    private void loadRoute()
    {
        this.generateTestRoutes();

        foreach (string rtstr in routesFromGA) {
            List<string> wpstr = rtstr.Split(',').ToList();
            WayPoint[] wps = this.GetComponentsInChildren<WayPoint>();
            List<WayPoint> route = new List<WayPoint>();
            for (int i=0;i<wpstr.Count;i++)
            {
                int point = int.Parse(wpstr[i]);
                WayPoint wp = wps.First(x => x.name == point.ToString());
                route.Add(wp);
            }
            waypoints.Add(route);
        }
        
        CreateNavCubePrefab();
    }

    private void generateTestRoutes() {
        //From GA
        string r51974 = "13,16,14,15,12,14,11,7,8,6,2,4,1,2,0,1,18,21,18,19,20,17,18,22,20,23,19,22,26,25,28,24,25,23,19,17,19,21,24,22,25,21,30,13,10,8,9,12,9,5,7,4,8,11,15,16,14,13,11,9,6,3,5,2,3,0,1,5,8,12,14,10,7,24,27,25,29,26,23,26,28,32,29,31,28,30,27,31,33,32,31,30,33";
        //this.routesFromGA.Add(r51974);



        string r26340_1 = "0,1,18,21,19,17,19,18,17,20,19,21,24,7,4,8,6,9,12,14,15,16,14,13,30,27,31,28,32,33,30,31,32,29,31,28,30,27,25,29,26,25,23,19,22,18,1,2,5,9,11";
        string r26340_2 = "33,31,28,26,23,20,22,24,7,5,8,8,10,14,11,7,8,12,15,14,13,16,15,11,8,6,2,3,5,1,4,2,0,3,6,9,8,7,10,13,27,24,28,25,26,22,25,21,24,25";
        this.routesFromGA.Add(r26340_1);
        this.routesFromGA.Add(r26340_2);
    }
    private void CreateNavCubePrefab() {
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
}
