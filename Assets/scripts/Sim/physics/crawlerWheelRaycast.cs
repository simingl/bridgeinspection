using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crawlerWheelRaycast : MonoBehaviour {

    int magneticLayerMask = 1 << 13;
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponentInParent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {

        Vector3 down = transform.TransformDirection(Vector3.left);

        RaycastHit hit;

        //if the ray hits a magnetic layer collider 
        if (Physics.Raycast(transform.position, down, out hit, 1f, magneticLayerMask))
        {
           

            Debug.DrawRay(transform.position, down, Color.cyan);

            //Vector3 forceVector =  

            rb.AddForce(hit.point);

            Debug.DrawRay(rb.position, hit.point, Color.blue);
        }


    }

   

}
