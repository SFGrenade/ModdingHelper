using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CameraLockArea))]
public class CameraLockAreaEditor : Editor 
{
    void OnSceneGUI()
    {
        var cla = target as CameraLockArea;
        var transform = cla.transform;
        
        var positions = new Vector3[5];
        positions[0] = new Vector3(cla.cameraXMin - 14.6f, cla.cameraYMin - 8.3f);
        positions[1] = new Vector3(cla.cameraXMax + 14.6f, cla.cameraYMin - 8.3f);
        positions[2] = new Vector3(cla.cameraXMax + 14.6f, cla.cameraYMax + 8.3f);
        positions[3] = new Vector3(cla.cameraXMin - 14.6f, cla.cameraYMax + 8.3f);
        positions[4] = new Vector3(cla.cameraXMin - 14.6f, cla.cameraYMin - 8.3f);
        Handles.DrawPolyLine(positions);
    }
}
