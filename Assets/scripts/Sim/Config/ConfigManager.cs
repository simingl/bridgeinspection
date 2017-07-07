using UnityEngine;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class ConfigManager {
	private static SettingsContainer settings;
	private static ConfigManager instance = new ConfigManager();

	private XmlDocument doc = new XmlDocument();
	private ConfigManager(){
		if (settings == null) {
			settings = SettingsContainer.readData ();
			//settings.writeData();
		}
	}

	public static ConfigManager getInstance(){
		return instance;
	}

	public bool ShowHUDUBDCamera(){
		return settings.hud.ShowUBDCamera;
	}

    public void setHUDUBDCamera(bool show) {
        settings.hud.ShowUBDCamera = show;
        settings.writeData();
    }
    public string getROSCoreIP() {
        return settings.ros.ROSCoreIP;
    }

    public void setROSCoreIP(string ip) {
        settings.ros.ROSCoreIP = ip;
        settings.writeData();
    }
}
