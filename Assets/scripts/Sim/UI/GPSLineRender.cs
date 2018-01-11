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
        public int gpsLinePrecisionScale = 50;
        private float precisionCounter = 0;

        //[SerializeField] 
        [SerializeField] private float longitude;
        [SerializeField] private float latitude;
        [SerializeField] private float prevLongitude;
        [SerializeField] private float prevLatittude;

        [SerializeField] private float longitudeOffset;
        [SerializeField] private float latitudeOffset;

        [SerializeField] private int linePointCount;
        public int arrayMaxCount = 7000;



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
                linePointCount = LineRenderer.Points.Length;

                if(LineRenderer.Points.Length >= arrayMaxCount)
                {
                    LineRenderer.Points = makeRoomInArray(LineRenderer.Points);
                }


                precisionCounter++;
            }
            else
            {
                precisionCounter++;
            }

        }

        
        public Vector2[] makeRoomInArray( Vector2[] LineRendererArray )
        {
            int index = 0;
            var workingList = new List<Vector2>(0);
            Vector2[] workingArray;

            while(index < LineRendererArray.Length)
            {
                workingList.Add(LineRendererArray[index]);

                index += 2;
            }

            workingArray = workingList.ToArray();

            return workingArray;
        }
        

    }
}
