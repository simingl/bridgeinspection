using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UBDROSCam : MonoBehaviour {
    // private GameManager gameManager;
    private ROSManager rosManager;

    private RawImage rawImage;

    // Use this for initialization
    void Start () {
        // gameManager = GameManager.getInstance();
        rosManager = ROSManager.getInstance();

        rawImage = this.gameObject.GetComponent<RawImage>();
    }
	
	// Update is called once per frame
	void Update () {
        rosManager.ROSRender();
    }

    private void OnGUI()
    {
        rawImage.texture = rosManager.getUBDCam();
    }

    public void ToggleUBDCam(bool toggle)
    {
        if (rawImage.enabled)
        {
            rawImage.enabled = false;
        }
        else
        {
            rawImage.enabled = true;
        }
    }
}
