using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public int SceneIndex = 0;
    Button StartButton, SceneSelect, Options;
    
	// Use this for initialization
	void Start () {
        Button[] buttons = this.GetComponentsInChildren<Button>();
        StartButton = buttons[0];
        SceneSelect = buttons[1];
        Options = buttons[2];

        StartButton.onClick.Invoke();
        StartButton.Select();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void SceneIndexSelection() {
        SceneIndex = transform.Find("SceneSelectSubPanel/LeftPanel/SceneSelectDD").GetComponent<Dropdown>().value;
        Debug.Log(SceneIndex);
    }

    public void LoadSceneByIndex() { 
        SceneManager.LoadScene(SceneIndex);
    }
}