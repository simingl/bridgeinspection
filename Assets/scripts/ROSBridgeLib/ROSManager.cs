using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;

public class ROSManager{
	private static ROSManager instance = new ROSManager();

    private Texture2D UBDCam;

    private ROSBridgeWebSocketConnection ros = null;
    private Boolean lineOn = false;

    private string ip;

    public static ROSManager getInstance(){
		return instance;
	}

	private ROSManager(){
        UBDCam = new Texture2D(128, 128);
                
        //ROSConnect();
        
    }


    public void ROSConnect() {
        ros = new ROSBridgeWebSocketConnection("ws://"+ip, 9090);
        Debug.Log("ROSBridge connecting to " + ip);
        ros.AddSubscriber(typeof(RobotImageSensor));
        ros.AddSubscriber(typeof(DroneImageSensor));
        ros.AddPublisher(typeof(RobotTeleop));
        ros.Connect();
        lineOn = true;
    }

    public void ROSRender()
    {
        if (lineOn)
        { 
        this.ros.Render();
        }
    }
    public void RemoteControl(Vector3 linear, Vector3 angular) {
        if (lineOn)
        {
            TwistMsg msg = new TwistMsg(new Vector3Msg(linear.x, linear.y, linear.z), new Vector3Msg(angular.x, angular.y, angular.z));
            ros.Publish(RobotTeleop.GetMessageTopic(), msg);
        }
    }

    public void ROSDisconnect()
    {
        if (ros != null)
            ros.Disconnect();
    }

    public Texture2D getUBDCam() {
        
            return UBDCam;       
    }

    public void setIp(string newip)
    {
        ip = newip;
    }

    public string getIp()
    {
        return ip;
    }
    
}
