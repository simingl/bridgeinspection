using ROSBridgeLib;
using ROSBridgeLib.sensor_msgs;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.turtlesim;
using System.Collections;
using SimpleJSON;
using UnityEngine;


public class OdometryData : ROSBridgeSubscriber
{
    public new static string GetMessageTopic()
    {
        return "/odom";        
    }

    public new static string GetMessageType()
    {
        return "nav_msgs/Odometry";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new TwistMsg(msg["twist"]["twist"]);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        TwistMsg OdomData = msg as TwistMsg;
        ROSManager.getInstance().setLinear(OdomData.GetLinear());
        ROSManager.getInstance().setAngular(OdomData.GetAngular());
       
        Debug.Log(OdomData.ToYAMLString());


        //GeoPointMsg LocationGPS = msg as GeoPointMsg;
        //ROSManager.getInstance().setLatitude(LocationGPS.GetLatitude());
        //ROSManager.getInstance().setLongitude(LocationGPS.GetLongitude());
        //ROSManager.getInstance().setAltitude(LocationGPS.GetAltitude());
        //Debug.Log(LocationGPS.ToYAMLString());


        //CompressedImageMsg imgMsg = msg as CompressedImageMsg;
        //ROSManager.getInstance().getUBDCam().LoadImage(imgMsg.GetImage());
    }
}
