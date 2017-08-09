using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class droneGraphScript : MonoBehaviour {

    NGraph droneGraph;

    NGraphDataSeriesXyLiveTransient linearVelocity;

    NGraphDataSeriesXyLiveTransient angularVelocity;

    NGraphDataSeriesXyLiveTransient altitude;

    public GameObject drone;

    private Rigidbody droneRigidbody;
    private Transform droneTransform;
    private Vector3 velocityVector;
    private float velocityTemp;


    private void Awake()
    {
        droneGraph = gameObject.GetComponent<NGraph>();
        if (droneGraph == null)
        {
            Debug.LogWarning("NGraph component not found.  Aborting.");
            return;
        }

        //(-x, x, -y , y)
        droneGraph.setRanges(-2, 3, -2, 2);

        List<Vector2> velData = new List<Vector2>();

        linearVelocity = droneGraph.addDataSeries<NGraphDataSeriesXyLiveTransient>("Transient", Color.cyan);
        linearVelocity.Data = velData;
        linearVelocity.UpdateRate = 0.005f;

        droneRigidbody = drone.GetComponent<Rigidbody>();
        droneTransform = drone.GetComponent<Transform>();
        velocityVector = droneTransform.position;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(droneGraph == null)
        {
            return;
        }

        float movementPerFrame = Vector3.Distance(velocityVector, droneTransform.position);
        velocityTemp = movementPerFrame / Time.deltaTime;
        velocityVector = droneTransform.position;

        linearVelocity.UpdateValue = velocityTemp;
		
	}
}
