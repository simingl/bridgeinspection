#pragma strict

var playerObject 	: GameObject;				// The player, what we want to stay above
var height			: float			= 30.0;		// Height above the player

function Start () {
	// Assign variable if it isn't already assigned..  Change the name to your player if needs be.
	if (!playerObject)
		playerObject = GameObject.Find("PlayerObject");
}

function LateUpdate () {
	// Keep the camera above the player.
	// (Clipping Planes may need changing too if you change the height!)
	transform.position = playerObject.transform.position + Vector3(0,height,0);
}