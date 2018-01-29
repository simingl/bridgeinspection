using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class GPSPanel : MonoBehaviour
    {

        public Text AltitudeOut;
        public Text LongitudeOut;
        public Text LatitudeOut;
        public GameObject GPSLineRenderer;



        private ROSManager rosManager;

    private void Awake()
    {
        GPSLineRenderer.SetActive(false);
    }



    // Use this for initialization
    void Start()
        {
            rosManager = ROSManager.getInstance();            

        }


        // Update is called once per frame
        void Update()
        {
            getROSGPS();

            if(rosManager.getLongitude() > 0 || rosManager.getLatitude() > 0)
            {
                drawGPSLine();
            }


        }

        private void getROSGPS()
        {
            if (AltitudeOut != null && rosManager.getAltitude() > 0)
            {
                AltitudeOut.text = "Altitude: " + rosManager.getAltitude().ToString();
            }
            if (LongitudeOut != null && rosManager.getLongitude() > 0)
            {
                LongitudeOut.text = "Longitude: " + rosManager.getLongitude().ToString();
            }
            if (LatitudeOut != null && rosManager.getLatitude() > 0)
            {
                LatitudeOut.text = "Latitude: " + rosManager.getLatitude().ToString();
            }
            

        }

        private void drawGPSLine()
        {
            GPSLineRenderer.SetActive(true);

        }


    }

