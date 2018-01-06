using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSPanel : MonoBehaviour {

    public Text AltitudeOut;
    public Text LongitudeOut;
    public Text LatitudeOut;

    private ROSManager rosManager;

    
    // Use this for initialization
    void Start () {
        rosManager = ROSManager.getInstance();


		
	}
	
	// Update is called once per frame
	void Update () {
        getROSGPS();

		
	}

    private void getROSGPS()
    {
        if(AltitudeOut != null)
        {
            AltitudeOut.text = "Altitude: " + rosManager.getAltitude().ToString();
        }
        if (LongitudeOut != null)
        {
            LongitudeOut.text = "Longitude: " + rosManager.getLongitude().ToString();
        }
        if (LatitudeOut != null)
        {
            LatitudeOut.text = "Latitude: " + rosManager.getLatitude().ToString();
        }
    }

   
}
