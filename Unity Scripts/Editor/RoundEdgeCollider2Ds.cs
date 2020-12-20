using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class RoundEdgeCollider2Ds
{
    [MenuItem("Terrain Colliders/Round EdgeCollider2Ds")]
    static void RoundTerrainEdgeCollider2Ds()
    {
        EdgeCollider2D[] ec2dList = GameObject.FindObjectsOfType<EdgeCollider2D> ();
        foreach (EdgeCollider2D ec2d in ec2dList) {
            if (ec2d.gameObject.GetComponent<MeshRenderer> () != null) {
                var points = ec2d.points;
                for (int i = 0; i < points.Length; i++) {
                    points[i].x = Mathf.Round (points[i].x);
                    points[i].y = Mathf.Round (points[i].y);
                }
                ec2d.points = points;
            }
        }
    }
}
