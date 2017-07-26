#pragma strict

// Ensure that "Map Player Position" and "Map Arrow" are both in the "Map Only" layer!

var playerObject : GameObject;

function Start () {
	// Set the variable.
	if (!playerObject)
		playerObject = GameObject.Find("PlayerObject");
}

function LateUpdate () {
	// Keep the arrow object at the player position and rotation.
	transform.position = playerObject.transform.position;
	transform.rotation = playerObject.transform.rotation;
}