# ModdingHelper

This was a collection of helper classes for Hollow Knight mods.  
Helper for Unity projects are also included.

## Generating Maps

1. Have a GameObject
2. Put the MapGenerator Component on it
3. Optional: you can also add a MeshRenderer, to look what the mesh looks like
4. Set the desired width, height, seed and fill percentage
5. Go into "play mode" (click the play button at the top of the editor)
6. Right-click the MeshFilter on the GameObject and select "Generate Map"
7. Wait a bit
8. With the optional MeshRenderer, you can now see the generated mesh
9. To save the mesh, right-click the MeshFilter and select "Save Mesh as OBJ..."
10. Now just pick where you want it saved
11. Now you can load that saved mesh as terrain or you can edit it further in another software

## Generating Terrain Collision

1. Have a GameObject
2. Put a MeshFilter Component on it
3. Select a mesh for the MeshFilter
4. Right-click the MeshFilter on the GameObject and select "Create Collision"
5. This will add a PolygonCollider2D on the GameObject
