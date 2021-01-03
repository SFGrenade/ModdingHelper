using UnityEngine;
using System.Collections;
using UnityEditor;
using SFCore.MonoBehaviours;

[CustomEditor(typeof(PatchMusicRegions))]
[CanEditMultipleObjects]
public class PatchMusicRegionsEditor : Editor
{
    string[] _choices = new []
    {
        "Normal",
        "Normal Alt",
        "Normal Soft",
        "Normal Softer",
        "Normal Flange",
        "Normal Flangier",
        "Action",
        "Action and Sub",
        "Sub Area",
        "Silent",
        "Silent Flange",
        "Off",
        "Tension Only",
        "Normal - Gramaphone",
        "Action Only",
        "Main Only",
        "HK Decline 2",
        "HK Decline 3",
        "HK Decline 4",
        "HK Decline 5",
        "HK Decline 6"
    };

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        //int index = EditorGUILayout.Popup("Snapshot Name", serializedObject.FindProperty("SnapshotIndex").intValue, _choices);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("SnapshotIndex"), new GUIContent("Snapshot"));
        //serializedObject.FindProperty("SnapshotIndex").intValue = index;
        serializedObject.FindProperty("SnapshotName").stringValue = _choices[serializedObject.FindProperty("SnapshotIndex").intValue];

        EditorGUILayout.PropertyField(serializedObject.FindProperty("MusicRegionSet"), new GUIContent("Music Region Set"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Main"), new GUIContent("Main"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Action"), new GUIContent("Action"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Sub"), new GUIContent("Sub"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Tension"), new GUIContent("Tension"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MainAlt"), new GUIContent("Main Alt"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Extra"), new GUIContent("Extra"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Main2"), new GUIContent("Main 2"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Action2"), new GUIContent("Action 2"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Sub2"), new GUIContent("Sub 2"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Tension2"), new GUIContent("Tension 2"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MainAlt2"), new GUIContent("Main Alt 2"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Extra2"), new GUIContent("Extra 2"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Dirtmouth"), new GUIContent("Dirtmouth"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MinesDelay"), new GUIContent("Mines Delay"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EnterTrackEvent"), new GUIContent("Enter Track Event"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EnterTransitionTime"), new GUIContent("Enter Transition Time"));
        
        //Save all changes made on the inspector
        serializedObject.ApplyModifiedProperties();
    }
}
