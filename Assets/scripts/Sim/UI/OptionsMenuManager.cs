using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour {

    private GameManager gameManager;

    public GameObject optionsCanvas;


    public Light UBDHeadlightR;
    public Light UBDHeadlightL;

    public RawImage UBDRosCam;

    public Text currentIp;

    public GameObject testValuesPanel;

    private bool active = false;


    // Use this for initialization
    void Start ()
    {
        gameManager = GameManager.getInstance();
        getIPaddress();


        optionsCanvas.SetActive(active);

    
		
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
        active = !active;
        optionsCanvas.SetActive(active);

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
        if ( IsIpaddress(newIp) )
        {
            gameManager.getConfigManager().setROSCoreIP(newIp);
            getIPaddress();
            
        }
        else
        {
            Debug.Log("Invalid IP address "+newIp); 
        }

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

    //error check for IP address
    public bool IsIpaddress(string value)
    {
        IPAddress address;

        if (string.IsNullOrEmpty (value))
        {
            return false;
        }
        string[] splitValues = value.Split('.');
        if(splitValues.Length != 4)
        {
            return false;
        }
        if (IPAddress.TryParse(value, out address))
        {
            return true;
        }

        return false;
    }

    public void tglTestValuePanel(bool value)
    {
        //if true
        if (testValuesPanel.activeSelf)
        {
            //set false
            testValuesPanel.SetActive(false);
        }
        else
        {
            // set true
            testValuesPanel.SetActive(true);
        }
        
    }


}
