using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnet : MonoBehaviour {

    private Rigidbody rb;
    const float K = 8.987551f; //Coulomb's constant
    public float myCharge = 1.0f;
    
    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        
        magneticObject[] magneticObjects = FindObjectsOfType<magneticObject>();
        foreach(magneticObject magneticObject in magneticObjects)
        {
            //this might be a good place to check against distance
            Attract(magneticObject);
        }

        
    }

    void Attract(magneticObject objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;
        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        float forceMagnitude = K * (myCharge * objToAttract.charge) / Mathf.Pow(distance, 2);

        Vector3 magforce = direction.normalized * forceMagnitude;

        Debug.DrawLine(rb.position , magforce, Color.green);
        //add force to myself
        rb.AddForce(magforce);
    }
}
