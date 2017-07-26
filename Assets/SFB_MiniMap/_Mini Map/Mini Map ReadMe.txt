Mini Map Copyright 2015 S.F. Bay Studios, Inc.

-------------------
IMPORTANT!  Please watch our instruction video!
https://youtu.be/b3lW4zBrnE8
It's 9 minutes long, but well worth the time :D
-------------------

This package creates a mini map in the top right corner of the screen.  The map can show either a real-world image of your game, or a hand-drawn depiction.  The map can be pre-drawn, or can be drawn as the player progresses through the game.

INITIAL SETUP:

* The scripts expect your player to object to be named "PlayerObject" in the hierarchy.  You can change this in the scripts if you'd like.  Look for the line in the Start() function.

1.  Attach the "mapCastRays" script to your PlayerObject.  This script sends out rays as the player progresses through a map.  You may turn this off if the map is pre-drawn.

2.  Set up three new layers named:  "Map Seen" "Map Hidden" "Map Only"

3.  Ensure that "Map Light" Culling Mask is set to "Map Only"

4.  Ensure that "Map Player Position" and it's child "Map Arrow" are in the "Map Only" Layer

5.  Ensure that "Map Camera" Culling Mask is set to "Map Only" & "Map Seen"

6.  Ensure that the "Depth" of Map Camera is greater than that of other cameras.


PRE-DRAWN, REAL-WORLD SETUP:

* This is perhaps favorable for an "Overworld" map, with a terrain, that is not dark.

1.  Turn off "mapCastRays" from the PlayerObject.

2.  Map Camera (Prefab or Object):
     - Turn off both "Edge Detect Effect Normals" scripts
     - Adjust "Clipping Planes" to "Near" 0 and "Far" 50.  (may need to adjust more if you have a very tall map)
     - Change "Culling Mask" to "Everything"

PRE-DRAWN, HAND-DRAWN SETUP:

* This is best for dungeons or dark maps, where the mini map will show the outline of the map, rather than the actual map itself.

1.  Turn off "mapCastRays" from the PlayerObject.

2.  Set all Map Object Layers that you want to be shown in the mini map to "Map Seen".

3.  Map Camera (Prefab or Object):
     - Turn on one of the "Edge Detect Effects Normals" scripts.  Choose whichever you prefer the look of.
     - Ensure "Clipping Planes" is set to "Near" 26 and "Far" 34.
     - Ensure "Culling Mask" is set to "Map Seen".

HAND-DRAWN, HIDDEN START SETUP:

* This is also great for dungeons or dark maps.  This setup will start hidden and will be revealed as the player walks through the map.

1.  Turn on "mapCastRays" from the PlayerObject.

2.  Set all Map Object Layers that you want to be shown in the mini map to "Map Hidden".

3.  Map Camera (Prefab or Object):
     - Turn on one of the "Edge Detect Effects Normals" scripts.  Choose whichever you prefer the look of.
     - Ensure "Clipping Planes" is set to "Near" 26 and "Far" 34.
     - Ensure "Culling Mask" is set to "Map Seen".


OTHER SETTINGS:

* You can remove or change the material of "Map Player Position" to change the arrow in the mini map.

* You can adjust the settings in the "Edge Detect Effects Normals" scripts, but be sure to copy so you don't destroy the original settings.