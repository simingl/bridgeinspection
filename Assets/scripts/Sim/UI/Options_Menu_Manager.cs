using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Options_Menu_Manager : MonoBehaviour {

    Canvas optionsCanvas;

    private GameObject ubd; 


    Light headlightR;

    
    // Use this for initialization
    void Start() {
        optionsCanvas = GetComponent<Canvas>();
        optionsCanvas.enabled = false;
        ubd = GameObject.Find("UBD").GetComponent<GameObject>();
        
        //headlightR = GameObject.Find("UBD").GetComponentInChildren<Light>();

    }

    

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            Toggle_Canvas();
        }

    }

    //Pause funcion
    public void Pause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    //Toggle options Canvas function
    public void Toggle_Canvas()
    {
        optionsCanvas.enabled = !optionsCanvas.enabled;
    }

    public void Toggle_Headlights()
    {
       
        headlightR.enabled = !headlightR.enabled; 


    }
   

}
