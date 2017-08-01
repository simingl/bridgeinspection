using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementControl : MonoBehaviour {

    

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        this.handleKeyboardControl();

    }

    private void handleKeyboardControl()
    {
        if(this.gameObject.name == "UBD")
        {
            ubdMovement();
        }
        else
        {
            flyingMovement();
        }
    }


    private void ubdMovement()
    {
        //Angular movement
        //get axis to rotate around
        Vector3 leftaxis = transform.TransformDirection(Vector3.up);
        //rotate
        this.transform.RotateAround(transform.position, leftaxis, Input.GetAxis("Horizontal"));

        //Linear movement
        //Vector3.left is used because UBD transform is off 
        this.transform.Translate(Input.GetAxis("Vertical") * Vector3.left * Time.deltaTime);

    }

    private void flyingMovement()
    {
        int increaseSpeed = 1;

        //increase the speed of the free cam
        if (this.gameObject.name == "FreeCamObject")
        {
            increaseSpeed = 4;
        }
        
        //Angular movement
        //get axis to rotate around
        Vector3 leftaxis = transform.TransformDirection(Vector3.up);
        //rotate
        this.transform.RotateAround(transform.position, leftaxis, Input.GetAxis("Horizontal"));

        //Linear movement forward and back
        this.transform.Translate(Input.GetAxis("Vertical") * Vector3.forward * increaseSpeed * Time.deltaTime);

        //Linear movement up and down
        this.transform.Translate(Input.GetAxis("Jump") * Vector3.up * Time.deltaTime * increaseSpeed );
    }
}
