using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor; // allows the unity editor .isPlaying to be turned off
#endif

public class optionsMenu : MonoBehaviour {

    Canvas optionsCanvas;

    // Use this for initialization
	void Start ()
    {
        optionsCanvas = GetComponent<Canvas>();
        optionsCanvas.enabled = false;
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
		
	}

    public void Pause()
    {
        optionsCanvas.enabled = !optionsCanvas.enabled;
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        
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
