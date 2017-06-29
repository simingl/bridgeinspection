using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UBDControl : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.HandleKeyboardControl();
    }

    private void HandleKeyboardControl()
    {
        //rotating
        Vector3 leftaxis = transform.TransformDirection(Vector3.up);
        this.transform.RotateAround(transform.position, leftaxis, Input.GetAxis("Horizontal"));
        //moving forward and backward
        this.transform.Translate(Input.GetAxis("Vertical") * Vector3.left * Time.deltaTime);

        //constructing ROS teleop message
        //float _dx = Input.GetAxis("Horizontal");
        //float _dy = Input.GetAxis("Vertical");
        //float linear = _dy * 0.5f;
        //float angular = -_dx * 0.2f;
        //ROSManager.getInstance().RemoteControl(new Vector3(linear, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, angular));
    }


}
