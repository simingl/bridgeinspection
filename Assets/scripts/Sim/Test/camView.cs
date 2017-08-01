using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class camView : MonoBehaviour {
    public Camera mainCamera;

    public Transform viewOne;
    public Transform viewTwo;
    public Transform viewThree;

    private int viewPosition;

    private void Awake()
    {
        //viewPosition is used as reference for playerControlSelection
        //so it needed to be initialized in Awake
        viewPosition = 1;
    }

    // Use this for initialization
    void Start () {
        
        getPosition(viewPosition);

		
	}
	
	// Update is called once per frame
	void Update () {

        getPosition(viewPosition);

    }

    private void getPosition(int position)
    {
        switch (position)
        {
            case 1:
                camViewOne();
                break;
            case 2:
                camViewTwo();
                break;
            case 3:
                camViewThree();
                break;
        }
    }

    private void camViewOne()
    {
        mainCamera.transform.SetPositionAndRotation(viewOne.position, viewOne.rotation);
    }
    private void camViewTwo()
    {
        mainCamera.transform.SetPositionAndRotation(viewTwo.position, viewTwo.rotation);
    }
    private void camViewThree()
    {
        mainCamera.transform.SetPositionAndRotation(viewThree.position, viewThree.rotation);
    }

    public void setCamViewPosition(int newposition)
    {
        viewPosition = newposition;
    }

    public int getViewPositionInt()
    {
        return viewPosition;
    }

}
