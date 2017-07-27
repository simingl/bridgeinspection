using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UBDSpeedPlotTransient : MonoBehaviour {
    public bool TakeScreenShots = false;
    NGraph mGraph;
    NGraphDataSeriesXyLiveTransient mSeries1;
    NGraphDataSeriesXyLiveTransient mSeries2;

    void Awake()
    {
        mGraph = gameObject.GetComponent<NGraph>();
        if (mGraph == null)
        {
            Debug.LogWarning("NGraph component not found.  Aborting.");
            return;
        }
        mGraph.setRanges(-10, 0, -8, 8);

        List<Vector2> data1 = new List<Vector2>();
        List<Vector2> data2 = new List<Vector2>();

        mSeries1 = mGraph.addDataSeries<NGraphDataSeriesXyLiveTransient>("Transient", Color.yellow);
        mSeries1.PlotThickness = 0.5f;
        mSeries1.Data = data1;
        mSeries1.UpdateRate = 0.05f;

        mSeries2 = mGraph.addDataSeries<NGraphDataSeriesXyLiveTransient>("Transient", Color.green);
        mSeries2.Data = data2;
        mSeries2.UpdateRate = 0.05f;

    }

    float mLastShot = 0;
    private int screenshotCount = 0;
    public void Update()
    {
        if (mGraph == null)
            return;

        mSeries1.UpdateValue = Mathf.Sin(Time.time) * 7 + Random.value;
        mSeries2.UpdateValue = Mathf.Cos(Time.time) * 3 ;

        if (!TakeScreenShots)
            return;

        mLastShot += Time.deltaTime;
        if (mLastShot > 0.5f)
        {
            mLastShot = 0;
            string screenshotFilename;
            do
            {
                screenshotCount++;
                screenshotFilename = "transient_" + screenshotCount + ".png";
            } while (System.IO.File.Exists(screenshotFilename));

            Application.CaptureScreenshot(screenshotFilename);
        }
    }

}
