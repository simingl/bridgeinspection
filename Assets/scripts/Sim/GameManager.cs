using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private ROSManager rosManager;
    private UIManager uiManager;

	// Use this for initialization
	void Start () {
        rosManager = ROSManager.getInstance();
        //uiManager = new UIManager();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
