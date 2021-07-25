using UnityEditor;
using UnityEngine;

public static class CameraModeSwitch
{
    [MenuItem("Camera/Orthographic")]
    static public void OrthographicCamera()
    {
        foreach (var cam in GameObject.FindObjectsOfType<Camera>())
            cam.transparencySortMode = TransparencySortMode.Orthographic;
    }
    [MenuItem("Camera/Perspective")]
    static public void PerspectiveCamera()
    {
        foreach (var cam in GameObject.FindObjectsOfType<Camera>())
            cam.transparencySortMode = TransparencySortMode.Default;
    }
}
