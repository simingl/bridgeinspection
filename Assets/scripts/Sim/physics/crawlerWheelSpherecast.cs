using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crawlerWheelSpherecast : MonoBehaviour {

    public bool attached = false;

    int magneticLayerMask = 1 << 13;
    private Rigidbody rb;
    private Transform tf;

    
    public float sc_radius = 1.0f;
    public float sc_distance = 0.5f;
    public float adjustMagneticForce = 0.2f;

    private Vector3 direction;
    private Vector3 origin;
    //private Vector3 down;
    //private Vector3 originOffset = origin.x ;
    //private bool attached =false;


    // Use this for initialization
    void Start ()
    {
        rb = GetComponentInParent<Rigidbody>();
        tf = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        RaycastHit hit;
        origin = tf.position + tf.right;
        
        //-tf.right (-x) is down
        direction = - tf.right;
        

        //show wheel down direction
        //Debug.DrawRay(tf.position, - tf.right, Color.yellow);



        if (Physics.SphereCast(origin , sc_radius, direction, out hit, sc_distance, magneticLayerMask))
        {
            //set attached to true
            attached = true;

            Debug.DrawLine(origin, hit.point, Color.magenta);
            //Debug.Log("Spherecast hit magnetic object.");

            //rb.AddForce(hit.point - tf.position * adjustMagneticForce);
            //Debug.DrawRay(tf.position, hit.point - tf.position , Color.blue);

            rb.AddForceAtPosition((hit.point - tf.position) * adjustMagneticForce, hit.point);
            Debug.DrawRay(tf.position, hit.point - tf.position, Color.blue);

            /*
            //add more forward force
            if (Input.GetKeyDown("w"))
            {
                rb.AddForce(rb.transform.forward);
                Debug.DrawRay(rb.transform.position, rb.transform.forward, Color.blue);
            }
            */



        }
        else
        {
            attached = false;
        }
    }

    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(origin, sc_radius);
    }
    */
}
