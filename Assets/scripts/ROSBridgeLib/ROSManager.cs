using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;

public class ROSManager{
	private static ROSManager instance = new ROSManager();

    private ROSBridgeWebSocketConnection ros = null;
    private Boolean lineOn;

    public static ROSManager getInstance(){
		return instance;
	}

	private ROSManager(){
        ROSConnect();
	}

    public void ROSConnect() {
        ros = new ROSBridgeWebSocketConnection("ws://134.197.87.18", 9090);
        ros.AddSubscriber(typeof(RobotImageSensor));
        ros.AddSubscriber(typeof(DroneImageSensor));
        ros.AddPublisher(typeof(RobotTeleop));
        ros.Connect();
        lineOn = true;
    }

    public void ROSRender() {
        this.ros.Render();
    }
    public void RemoteControl(Vector3 linear, Vector3 angular) { 
        TwistMsg msg = new TwistMsg (new Vector3Msg(linear.x, linear.y, linear.z), new Vector3Msg(angular.x, angular.y, angular.z));
        ros.Publish (RobotTeleop.GetMessageTopic (), msg);
    }

    public void ROSDisconnect()
    {
        if (ros != null)
            ros.Disconnect();
    }


}
