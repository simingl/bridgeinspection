using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ROSBridgeLib;

public class infoReadout : MonoBehaviour {

    public float Latitude, Longitude, Altitude;
    public Text[] textBox;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Latitude = ROSManager.getInstance().getLatitude();
        Longitude = ROSManager.getInstance().getLongitude();
        Altitude = ROSManager.getInstance().getAltitude();

        textBox = GetComponentsInChildren<Text>();

        textBox[0].text = Latitude.ToString();
        textBox[1].text = Longitude.ToString();
        textBox[2].text = Altitude.ToString();
    }
}
