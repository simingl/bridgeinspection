using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rosIPmanager : MonoBehaviour {
    private GameManager gameManager;
    private ROSManager rosManager;

    public Text ipAddress;

	// Use this for initialization
	void Start () {
        gameManager = GameManager.getInstance();
        rosManager = gameManager.getROSManager();
        ipAddress.text = rosManager.getIp();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void displayIP()
    {
        ipAddress.text = rosManager.getIp();
    }

}
