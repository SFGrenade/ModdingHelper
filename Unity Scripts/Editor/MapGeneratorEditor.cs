using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor 
{
	[MenuItem("CONTEXT/MeshFilter/Generate Map")]
    public static void GenerateMap(MenuCommand menuCommand)
    {
        Debug.Log("GenerateMap called");
        MapGenerator mg = (menuCommand.context as MeshFilter).gameObject.GetComponent<MapGenerator>();
        Debug.Log("Generating Map");
        mg.GenerateMap();
        Debug.Log("Building Mesh started");
        mg.StartCoroutine(buildMesh(mg));
        Debug.Log("GenerateMap ended");
    }

    private static IEnumerator buildMesh(MapGenerator mg) {
        if (mg == null) yield break;
        if (mg.map == null) yield break;

        Debug.Log("Setting up variables");

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> UVs = new List<Vector2>();
        List<int> triangles = new List<int>();

        Debug.Log("Calculating Vertices and Triangles");
        
        var map = mg.map;
        for (int x = 0; x < mg.width; x ++) {
            for (int y = 0; y < mg.height; y ++) {
                if (mg.map[x,y] == 1) {
                    // Collision
                    #region Vertices
                    bool v1e = false;
                    bool v2e = false;
                    bool v3e = false;
                    bool v4e = false;
                    Vector3 te;
                    try {
                        te = vertices.First(t => ((t.x == x) && (t.y == y)));
                    } catch (Exception) {
                        v1e = true;
                    }
                    try {
                        te = vertices.First(t => ((t.x == x) && (t.y == y+1)));
                    } catch (Exception) {
                        v2e = true;
                    }
                    try {
                        te = vertices.First(t => ((t.x == x+1) && (t.y == y+1)));
                    } catch (Exception) {
                        v3e = true;
                    }
                    try {
                        te = vertices.First(t => ((t.x == x+1) && (t.y == y)));
                    } catch (Exception) {
                        v4e = true;
                    }
                    if (v1e)
                        vertices.Add(new Vector3(x,   y));
                    if (v2e)
                        vertices.Add(new Vector3(x,   y+1));
                    if (v3e)
                        vertices.Add(new Vector3(x+1, y+1));
                    if (v4e)
                        vertices.Add(new Vector3(x+1, y));
                    #endregion
                    #region Triangles
                    Vector3 v1 = vertices.First(t => ((t.x == x) && (t.y == y)));
                    Vector3 v2 = vertices.First(t => ((t.x == x) && (t.y == y+1)));
                    Vector3 v3 = vertices.First(t => ((t.x == x+1) && (t.y == y+1)));
                    Vector3 v4 = vertices.First(t => ((t.x == x+1) && (t.y == y)));
                    int t1 = vertices.IndexOf(v1);
                    int t2 = vertices.IndexOf(v2);
                    int t3 = vertices.IndexOf(v3);
                    int t4 = vertices.IndexOf(v4);
                    triangles.Add(t1);
                    triangles.Add(t2);
                    triangles.Add(t3);
                    triangles.Add(t1);
                    triangles.Add(t3);
                    triangles.Add(t4);
                    #endregion
                }
                else {
                    // Nothing
                }
            }
            yield return null;
        }

        Debug.Log("Creating Mesh");
        
        Mesh mesh = new Mesh();
        mg.gameObject.GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vertices.ToArray();
        mesh.uv = UVs.ToArray();
        mesh.triangles = triangles.ToArray();

        Debug.Log("Creating Mesh Done");
    }
    
    public override void OnInspectorGUI()
    {
        MapGenerator mg = (MapGenerator)target;
    
        mg.width = EditorGUILayout.IntField("Width", mg.width);
        mg.height = EditorGUILayout.IntField("Height", mg.height);
        mg.seed = EditorGUILayout.TextField("Seed", mg.seed);
        mg.useRandomSeed = EditorGUILayout.Toggle("Use random Seed", mg.useRandomSeed);
        mg.randomFillPercent = EditorGUILayout.IntSlider("Random fill percent", mg.randomFillPercent, 40, 60);
    }

	[MenuItem("CONTEXT/MeshFilter/Create Collision")]
    public static void GenerateCollision(MenuCommand menuCommand)
    {
        Debug.Log("GenerateMap called");

        if ((menuCommand.context as MeshFilter).gameObject.GetComponent<PolygonCollider2D>() == null) {
            (menuCommand.context as MeshFilter).gameObject.AddComponent<PolygonCollider2D>();
        }
        
        CreatePolygon2DColliderPoints((menuCommand.context as MeshFilter), (menuCommand.context as MeshFilter).gameObject.GetComponent<PolygonCollider2D>());

        Debug.Log("GenerateMap ended");
    }
    
    #region Helper
    private static void CreatePolygon2DColliderPoints(MeshFilter filter, PolygonCollider2D polyCollider)
    {
        var edges = BuildEdgesFromMesh(filter);
        var paths = BuildColliderPaths(edges);
        ApplyPathsToPolygonCollider(polyCollider, paths);
    }

    private static Dictionary<Edge2D, int> BuildEdgesFromMesh(MeshFilter filter)
    {
        var mesh = filter.sharedMesh;

        if (mesh == null)
            return null;

        var verts = mesh.vertices;
        var tris = mesh.triangles;
        var edges = new Dictionary<Edge2D, int>();

        for (int i = 0; i < tris.Length - 2; i += 3) {

            var faceVert1 = verts[tris[i]];
            var faceVert2 = verts[tris[i + 1]];
            var faceVert3 = verts[tris[i + 2]];

            Edge2D[] faceEdges;
            faceEdges = new Edge2D[] {
                new Edge2D{ a = faceVert1, b = faceVert2 },
                new Edge2D{ a = faceVert2, b = faceVert3 },
                new Edge2D{ a = faceVert3, b = faceVert1 },
            };

            foreach(var edge in faceEdges) {
                if (edges.ContainsKey(edge))
                    edges[edge]++;
                else
                    edges[edge] = 1;
            }
        }

        return edges;
    }

    private static List<Vector2[]> BuildColliderPaths(Dictionary<Edge2D, int> allEdges)
    {
        if (allEdges == null)
            return null;

        var outerEdges = GetOuterEdges(allEdges);

        var paths = new List<List<Edge2D>>();
        List<Edge2D> path = null;

        while (outerEdges.Count > 0) {
            if (path == null) {
                path = new List<Edge2D>();
                path.Add (outerEdges[0]);
                paths.Add (path);

                outerEdges.RemoveAt(0);
            }
            bool foundAtLeastOneEdge = false;
            int i = 0;
            while (i < outerEdges.Count) {
                var edge = outerEdges [i];
                bool removeEdgeFromOuter = false;

                if (edge.b == path[0].a) {
                    path.Insert (0, edge);
                    removeEdgeFromOuter = true;
                }
                else if (edge.a == path[path.Count - 1].b) {
                    path.Add(edge);
                    removeEdgeFromOuter = true;
                }

                if (removeEdgeFromOuter) {
                    foundAtLeastOneEdge = true;
                    outerEdges.RemoveAt(i);
                } else
                    i++;
            }
            //If we didn't find at least one edge, then the remaining outer edges must belong to a different path
            if (!foundAtLeastOneEdge)
                path = null;
        }
        var cleanedPaths = new List<Vector2[]>();
        foreach(var builtPath in paths) {
            var coords = new List<Vector2>();

            foreach(var edge in builtPath)
                coords.Add (edge.a);

            cleanedPaths.Add (CoordinatesCleaned(coords));
        }
        return cleanedPaths;
    }

    private static void ApplyPathsToPolygonCollider(PolygonCollider2D polyCollider, List<Vector2[]> paths)
    {
        if (paths == null)
            return;

        polyCollider.pathCount = paths.Count;
        for (int i = 0; i < paths.Count; i++) {
            var path = paths [i];
            polyCollider.SetPath(i, path);
        }
    }

    private static List<Edge2D> GetOuterEdges(Dictionary<Edge2D, int> allEdges)
    {
        var outerEdges = new List<Edge2D>();

        foreach(var edge in allEdges.Keys) {
            var numSharedFaces = allEdges[edge];
            if (numSharedFaces == 1)
                outerEdges.Add (edge);
        }

        return outerEdges;
    }

    private static bool CoordinatesFormLine(Vector2 a, Vector2 b, Vector2 c)
    {
        //If the area of a triangle created from three points is zero, they must be in a line.
        float area = a.x * ( b.y - c.y ) + b.x * ( c.y - a.y ) + c.x * ( a.y - b.y );
        return Mathf.Approximately(area, 0f);
    }

    private static Vector2[] CoordinatesCleaned(List<Vector2> coordinates)
    {
        List<Vector2> coordinatesCleaned = new List<Vector2> ();
        coordinatesCleaned.Add (coordinates [0]);

        var lastAddedIndex = 0;

        for (int i = 1; i < coordinates.Count; i++) {
            var coordinate = coordinates [i];

            Vector2 lastAddedCoordinate = coordinates [lastAddedIndex];
            Vector2 nextCoordinate = (i + 1 >= coordinates.Count) ? coordinates[0] : coordinates [i + 1];

            if (!CoordinatesFormLine(lastAddedCoordinate, coordinate, nextCoordinate)) {
                coordinatesCleaned.Add (coordinate);
                lastAddedIndex = i;
            }
        }
        return coordinatesCleaned.ToArray ();
    }

    #region Nested
    struct Edge2D {
        public Vector2 a;
        public Vector2 b;

        public override bool Equals (object obj)
        {
            if (obj is Edge2D) {
                var edge = (Edge2D)obj;
                //An edge is equal regardless of which order it's points are in
                return (edge.a == a && edge.b == b) || (edge.b == a && edge.a == b);
            }
            return false;

        }

        public override int GetHashCode ()
        {
            return a.GetHashCode() ^ b.GetHashCode();
        }

        public override string ToString ()
        {
            return string.Format ("["+a.x+","+a.y+"->"+b.x+","+b.y+"]");
        }
    }
    #endregion
    #endregion
}
