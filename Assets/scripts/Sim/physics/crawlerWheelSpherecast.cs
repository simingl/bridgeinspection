using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crawlerWheelSpherecast : MonoBehaviour {

    int magneticLayerMask = 1 << 13;
    private Rigidbody rb;
    private Transform tf;

    
    public float sc_radius = 1.0f;
    public float sc_distance = 0.5f;
    public float adjustMagneticForce = 0.2f;

    private Vector3 direction;
    private Vector3 origin;
    //private Vector3 down;


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
        origin = tf.position;
        direction = tf.position;
        //down = -transform.right;



        if (Physics.SphereCast(origin, sc_radius, direction, out hit, sc_distance, magneticLayerMask))
        {
            Debug.DrawLine(origin, hit.point, Color.magenta);
            Debug.Log("Spherecast hit magnetic object.");

            rb.AddForce(tf.position - hit.point * adjustMagneticForce);
            Debug.DrawRay(tf.position, tf.position - hit.point, Color.blue);

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
