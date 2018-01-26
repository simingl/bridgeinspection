using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crawlerWheelRaycast : MonoBehaviour {

    int magneticLayerMask = 1 << 13;
    private Rigidbody rb;
    public float adjustMagneticForce = 0.2f;

    // Use this for initialization
    void Start () {
        rb = GetComponentInParent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {

        //
        Vector3 down = transform.TransformDirection(Vector3.left);

        RaycastHit hit;

        //if the ray hits a magnetic layer collider 
        if (Physics.Raycast(transform.position, down, out hit, 0.3f, magneticLayerMask))
        {
           

            Debug.DrawRay(transform.position, down, Color.cyan);

            //Vector3 forceVector = -hit.normal * adjustMagneticForce;

            Vector3 forceVector = hit.point ;

            //rb.AddForceAtPosition(transform.position, forceVector);
            //Debug.DrawRay(transform.position, forceVector, Color.blue);

            rb.AddForce(rb.transform.position * adjustMagneticForce);
            Debug.DrawRay(rb.position, forceVector, Color.blue);
        }


    }

   

}
