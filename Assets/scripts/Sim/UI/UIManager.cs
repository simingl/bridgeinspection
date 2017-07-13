using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private GameManager gameManager;
    //private ROSManager rosManager;
    //private ConfigManager configManager;

    public Text currentIP;


	// Use this for initialization
	void Start () {
        
        //getting IP address from xml file
        gameManager = GameManager.getInstance();
        getIPaddress();


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void getIPaddress()
    {
        currentIP.text = gameManager.getConfigManager().getROSCoreIP();
    }

    public void Set_IPaddress(string newIP)
    {

        //configManager.setROSCoreIP(newIP); 
    }
}
