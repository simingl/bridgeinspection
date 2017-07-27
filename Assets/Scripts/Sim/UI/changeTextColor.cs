using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class changeTextColor : MonoBehaviour {

    private Text bttnText;
    public Color firstState;
    public Color secondState;
    
    // Use this for initialization
	void Start () {
        bttnText = this.gameObject.GetComponentInChildren<Text>();		
	}
	
	
    public void toggleTextColor()
    {
        if(bttnText.color == firstState)
        {
            bttnText.color = secondState;
        }
        else
        {
            bttnText.color = firstState;
        }


    }
}
