using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class angularVelocity : MonoBehaviour {

    public Text angularVelocityOut;
    public GameObject ubd;

    private Transform ubdTransform;
    private Rigidbody ubdRigidbody;
    private Vector3 angularvelocityVector;
    private float velocityTemp;

	// Use this for initialization
	void Start () {
        ubdRigidbody = ubd.GetComponent<Rigidbody>();
        ubdTransform = ubd.GetComponent<Transform>();

        angularvelocityVector = ubdTransform.eulerAngles;

    }
	
	// Update is called once per frame
	void Update () {

        Vector3 angularmovementPerFrame = angularvelocityVector - ubdTransform.eulerAngles;
        velocityTemp = angularmovementPerFrame.y / Time.deltaTime;
        angularvelocityVector = ubdTransform.eulerAngles;

        angularVelocityOut.text = velocityTemp.ToString();
		
	}
}
