using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class re_engageGravity : MonoBehaviour {

    public crawlerWheelSpherecast[] wheels;
    public int numberWheelsNeedAttached;

    [SerializeField]
    private int numberAttached;    

    private Rigidbody rb;


    // Use this for initialization
    void Start () {
        rb = GetComponentInParent<Rigidbody>();

        rb.useGravity = false;

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        numberAttached = 0;
        foreach (crawlerWheelSpherecast wheel in wheels)
        {
            if (wheel.attached)
            {
                numberAttached++ ;
            }
        }

        if(numberAttached <= numberWheelsNeedAttached)
        {
            rb.useGravity = true;
        }
        else
        {
            rb.useGravity = false;
        }

		
	}
}
