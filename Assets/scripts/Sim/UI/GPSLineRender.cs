using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.Extensions
{
    public class GPSLineRender : MonoBehaviour
    {

        private ROSManager rosManager;
        public UILineRenderer LineRenderer;

        //multiplied with the raw gps value
        public float gpsValueScale = 2.0f;

        //only the nth gps value is added to the point list
        public int gpsLinePrecisionScale = 100;
        private float precisionCounter = 0;

        //[SerializeField] 
        private float longitude;
        private float latitude;
        private float prevLongitude;
        private float prevLatittude;

        private float longitudeOffset;
        private float latitudeOffset;

        //change the decimal place of the raw gps data by multiplying 
        public float GPSDecimalPrecisionVariable = 100;




        // Use this for initialization
        void Start()
        {
            rosManager = ROSManager.getInstance();

            longitudeOffset = rosManager.getLongitude();
            latitudeOffset = rosManager.getLatitude();

            setOffset();
           
        }


        // Update is called once per frame
        void Update()
        {
            getROSLonLat();

            if(prevLatittude != latitude && prevLongitude != longitude)
            {
                addPointToGPSLine();
            }

        }

        //
        private void setOffset()
        {
            latitudeOffset = latitudeOffset * GPSDecimalPrecisionVariable;
            longitudeOffset = longitudeOffset * GPSDecimalPrecisionVariable;

            //latitudeOffset = Mathf.Round(latitudeOffset);
            //longitudeOffset = Mathf.Round(longitudeOffset); 
        }

        private void getROSLonLat()
        {
            longitude = (rosManager.getLongitude() * GPSDecimalPrecisionVariable) - longitudeOffset;
            latitude = (rosManager.getLatitude() * GPSDecimalPrecisionVariable) - latitudeOffset;

        }

        public void addPointToGPSLine()
        {
            if (precisionCounter % gpsLinePrecisionScale == 0)
            {

                latitude = latitude * gpsValueScale;
                longitude = longitude * gpsValueScale;



                var point = new Vector2() { x = latitude, y = longitude };
                var pointlist = new List<Vector2>(LineRenderer.Points);
                pointlist.Add(point);
                LineRenderer.Points = pointlist.ToArray();

                prevLatittude = latitude;
                prevLongitude = longitude;

                //check that Line Renderer array is less than 8000
                Debug.Log(LineRenderer.Points.Length);
                if(LineRenderer.Points.Length >= 7500)
                {
                    //makeRoomInArray(LineRenderer.Points);
                }


                precisionCounter++;
            }
            else
            {
                precisionCounter++;
            }

        }

        /*
        public Vector2[] makeRoomInArray( Vector2[] LineRendererArray )
        {
            var workingArray = new List<Vector2>(LineRendererArray.Length/2);

            for (int i = 1; i < LineRendererArray.Length; i++)
            {
                workingArray = LineRendererArray[i + 1];
            }
        }
        */

    }
}
