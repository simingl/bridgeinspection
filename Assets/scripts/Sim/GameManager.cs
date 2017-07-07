using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour {
    private static GameManager instance = new GameManager();

    private UIManager uiManager;

    public static GameManager getInstance()
    {
        return instance;
    }

    // Use this for initialization
    void Start () {
        ConfigManager.getInstance();
    }
	
	// Update is called once per frame
	void Update () {
        ROSManager.getInstance().ROSRender();
        
	}

    public ROSManager getROSManager() {
        return ROSManager.getInstance();
    }
}
