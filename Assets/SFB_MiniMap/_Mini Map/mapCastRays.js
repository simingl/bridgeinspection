#pragma strict

// This script should be placed on your player object.  It will, at the rate of refresh, look for all
// objects in the "Map Hidden" layer within the distance.  Those objects must have a non-trigger
// collider on them to be seen.  maxHeight and minHeight are used to ensure that only objects within a
// specified Y range will be seen.  This helps keep objects that may be above or below a floor or ceiling
// from being "Seen" before the player gets there.

var distance 		: float = 8;	// How far from the player will the rays be cast.
var refresh			: float	= 1;	// 1 = 1 second.
var maxHeight		: float	= 10.0;	// How much higher can an object be to be seen?
var minHeight		: float	= 2.0;	// How much lower can an object be to be seen?
var searchMask		: LayerMask;	// Layermask for the search.  We want to only search "Map Hidden"

function Start () {
	InvokeRepeating("FindHiddenMap", 0, refresh);		// Will look for new map objects every second.
}

function FindHiddenMap(){
	// Gather a list of GameObjects in the "Map Hidden" layer
	var mapObjects = Physics.OverlapSphere(transform.position, distance, searchMask);
	// For each of the objects in the list...
	for (var i = 0; i < mapObjects.Length; i++) {
		// Make sure the Y position of the Map Object is within the maxHeight and minHeight boundaries
		if (mapObjects[i].transform.position.y > transform.position.y - minHeight && mapObjects[i].transform.position.y < transform.position.y + maxHeight)
			mapObjects[i].gameObject.layer		= LayerMask.NameToLayer("Map Seen");	// Switch the map layer to Map Seen
	}
}


// OLD METHOD IS BELOW.  We're keeping this here just for the sake of upgrades etc.
/*
function FindHiddenMap() : GameObject[] {
    var goArray 	= FindObjectsOfType(GameObject);
    var goList 		= new System.Collections.Generic.List.<GameObject>();
    for (var i = 0; i < goArray.Length; i++) {
		if (goArray[i].layer == LayerMask.NameToLayer("Map Hidden")) {
			if ((transform.position - goArray[i].transform.position).sqrMagnitude < distance * distance && goArray[i].layer != LayerMask.NameToLayer("Map Seen"))
			{
				// For all maps that are "Map Hidden" in range, set layer to "Map Seen".
				goArray[i].layer = LayerMask.NameToLayer("Map Seen");
			}
		}
	}
}*/