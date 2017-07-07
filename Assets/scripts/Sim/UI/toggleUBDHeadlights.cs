using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleUBDHeadlights : MonoBehaviour {

    public Light ubdHeadlightR;
    public Light ubdHeadlightL;
    


    // Use this for initialization
    void Start ()
    {
        //ubdHeadlightR = GameObject.Find("HeadLightR").GetComponentInChildren<Light>();

        //ubdHeadlightL = GameObject.Find("HeadLightL").GetComponentInChildren<Light>();


    }
	
	public void toggleLights(bool toggle)
    {
        
        if(ubdHeadlightL.enabled || ubdHeadlightR.enabled)
        {
            //shut off both headlights
            ubdHeadlightL.enabled = false;
            ubdHeadlightR.enabled = false;
                        
        }
        else
        {
            ubdHeadlightL.enabled = true;
            ubdHeadlightR.enabled = true;
        }

    }
}
