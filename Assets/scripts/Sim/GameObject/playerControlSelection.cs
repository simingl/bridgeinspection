using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControlSelection : MonoBehaviour {

    public GameObject[] playerSelect;

    GameObject currentPlayer;
   
    private int controlIndex;

    //used for reference to the camera position
    public camView positionReference;

    // Use this for initialization
    void Start () {

        enableCurrentPlayerMovementControl();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void getCurrentPlayerObject()
    {
        controlIndex = positionReference.getViewPositionInt();

        currentPlayer = playerSelect[controlIndex - 1];
    }

    private void disableAllPlayerControls()
    {
        for (int i = 0; i < playerSelect.Length; i++)
        {
            playerSelect[i].GetComponent<movementControl>().enabled = false;
        }
    }

    public void enableCurrentPlayerMovementControl()
    {
        disableAllPlayerControls();

        getCurrentPlayerObject();

        currentPlayer.GetComponent<movementControl>().enabled = true;

    }
}
