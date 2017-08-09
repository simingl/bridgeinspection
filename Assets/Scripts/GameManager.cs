using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor; // allows the unity editor .isPlaying to be turned off
#endif



public class GameManager : MonoBehaviour {
    private static GameManager instance = new GameManager();

    private UIManager uiManager;
    //private ConfigManager configManager;
    //private ROSManager rosManager;

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

    public ConfigManager getConfigManager()
    {
        return ConfigManager.getInstance();
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false; // in editor turn off play mode
        #else
        Application.Quit();// if an application then quit
        #endif
    }


}
