using ROSBridgeLib;
using ROSBridgeLib.sensor_msgs;
using ROSBridgeLib.geographic_msgs;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.turtlesim;
using System.Collections;
using SimpleJSON;
using UnityEngine;

/**
 * This is a toy example of the Unity-ROS interface talking to the TurtleSim 
 * tutorial (circa Groovy). Note that due to some changes since then this will have
 * to be slightly re-written, but as its a test ....
 * 
 * This defines the callback that links the color_sensor message and its callback
 * 
 * @author Michael Jenkin, Robert Codd-Downey and Andrew Speers
 * @version 3.0
 **/

public class GPSMessage : ROSBridgeSubscriber {
	
	public new static string GetMessageTopic() {
        // return "/iRobot/camera/image_raw/compressed";
        return "/gps";
    }  
	
	public new static string GetMessageType() {
		return "sensor_msgs/NavSatFix";
	}
	
	public new static ROSBridgeMsg ParseMessage(JSONNode msg) {
        return new GeoPointMsg(msg);
	}
	
	public new static void CallBack(ROSBridgeMsg msg) {
        GeoPointMsg LocationGPS = msg as GeoPointMsg;
        ROSManager.getInstance().setLatitude(LocationGPS.GetLatitude());
        ROSManager.getInstance().setLongitude(LocationGPS.GetLongitude());
        ROSManager.getInstance().setAltitude(LocationGPS.GetAltitude());
        Debug.Log(LocationGPS.ToYAMLString());
    }
}
