using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMatch : MonoBehaviour {

    public Image[] MatchColor;
    

	// Use this for initialization
	void Start () {
        MatchColor = transform.Find("/HUD_HorLayout/InfoReadout").GetComponentsInChildren<Image>();
        MatchColor[2].color = MatchColor[1 ].color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
