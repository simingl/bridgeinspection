using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ubdVelocity : MonoBehaviour {

    public Text velocityOut;
    public GameObject ubd;

    private Transform ubdTransform;
    private Rigidbody ubdRigidbody;
    private Vector3 velocityVector;
    private float velocityTemp;
    

	// Use this for initialization
	void Start () {
        
        ubdRigidbody = ubd.GetComponent<Rigidbody>();
        ubdTransform = ubd.GetComponent<Transform>();

        velocityVector = ubdTransform.position;

		
	}
	
	// Update is called once per frame
	void Update () {

        //equation to find speed 
        float movementPerFrame = Vector3.Distance(velocityVector, ubdTransform.position);
        velocityTemp = (movementPerFrame / Time.deltaTime);
        velocityVector = ubdTransform.position;

        velocityOut.text = velocityTemp.ToString();

		
	}
}
