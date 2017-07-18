using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour {

    private GameManager gameManager;

    private Canvas optionsCanvas;


    public Light UBDHeadlightR;
    public Light UBDHeadlightL;

    public RawImage UBDRosCam;

    public Text currentIp;

    



    // Use this for initialization
    void Start ()
    {
        gameManager = GameManager.getInstance();
        getIPaddress();

        optionsCanvas = GetComponent<Canvas>();
        optionsCanvas.enabled = false;

    
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //pause and toggle options menu when esc is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            toggleOptionsCanvas();
        }

    }
    
    //Pause funcion
    public void Pause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    //Toggle options Canvas function
    public void toggleOptionsCanvas()
    {
        optionsCanvas.enabled = !optionsCanvas.enabled;
    }


    public void toggleLights(bool temp)
    {

        UBDHeadlightR.enabled = !UBDHeadlightR.enabled;
        UBDHeadlightL.enabled = !UBDHeadlightL.enabled;        

    }
    

    public void toggleUBDCam(bool temp)
    {
        UBDRosCam.enabled = !UBDRosCam.enabled;
    }

    public void setNewRosBridgeIp(string newIp)
    {
        gameManager.getConfigManager().setROSCoreIP(newIp);
        getIPaddress();
        //gameManager.getROSManager().setIp(newIp);

    }

    public void connectROSBridge()
    {
        string ipTemp = gameManager.getConfigManager().getROSCoreIP();

        gameManager.getROSManager().setIp(ipTemp);
        gameManager.getROSManager().ROSConnect();
    }

    public void disconnectROSBridge()
    {
        gameManager.getROSManager().ROSDisconnect();
    }


    private void getIPaddress()
    {
        currentIp.text = gameManager.getConfigManager().getROSCoreIP();
    }



}
