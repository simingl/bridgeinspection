/***********************************************************************
/*      Copyright Niugnep Software, LLC 2013 All Rights Reserved.      *
/*                                                                     *
/*     THIS WORK CONTAINS TRADE SECRET AND PROPRIETARY INFORMATION     *
/*     WHICH IS THE PROPERTY OF NIUGNEP SOFTWARE, LLC OR ITS           *
/*             LICENSORS AND IS SUBJECT TO LICENSE TERMS.              *
/**********************************************************************/

using UnityEngine;
using System.Collections;

public class NGraphSimpleUpdateGraphScript : MonoBehaviour
{
   public NGraphSimplePlayerScript mPlayer;
   Transform mPlayerTransform;
   
   NGraph mGraph;
   
   // The live updating plot for the X axis
   NGraphDataSeriesXyLiveTransient mPlotX;
   // The live updating plot for the Z axis
   NGraphDataSeriesXyLiveTransient mPlotZ;
   
   void Awake()
   {
      // Setup the graph
      mGraph = GetComponent<NGraph>();
      mGraph.setRanges(-10, 10, -10, 10);
      
      // Capture the player's transform
      mPlayerTransform = mPlayer.gameObject.transform;
      
      // Make the two plots
      mPlotX = mGraph.addDataSeries<NGraphDataSeriesXyLiveTransient>("X", Color.green);
      mPlotZ = mGraph.addDataSeries<NGraphDataSeriesXyLiveTransient>("Z", Color.blue);
      
      // Change thier update rates to be quicker
      mPlotX.UpdateRate = 0.01f;
      mPlotZ.UpdateRate = 0.01f;
   }
   
   // Update is called once per frame
   void Update ()
   {
      mPlotX.UpdateValue = mPlayerTransform.position.x;
      mPlotZ.UpdateValue = mPlayerTransform.position.z;
   }
}
