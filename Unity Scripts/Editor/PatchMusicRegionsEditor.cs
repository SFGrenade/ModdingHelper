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
        PatchMusicRegions pmr = (PatchMusicRegions)target;
        
        pmr.SnapshotIndex = EditorGUILayout.Popup("Snapshot Name", pmr.SnapshotIndex, _choices);
        pmr.SnapshotName = _choices[pmr.SnapshotIndex];
        //EditorUtility.SetDirty(target);

        pmr.MusicRegionSet = EditorGUILayout.TextField("Music Region Set", pmr.MusicRegionSet);

        pmr.Main = (AudioClip) EditorGUILayout.ObjectField("Main", pmr.Main, typeof(AudioClip), false);
        pmr.Action = (AudioClip) EditorGUILayout.ObjectField("Action", pmr.Action, typeof(AudioClip), false);
        pmr.Sub = (AudioClip) EditorGUILayout.ObjectField("Sub", pmr.Sub, typeof(AudioClip), false);
        pmr.Tension = (AudioClip) EditorGUILayout.ObjectField("Tension", pmr.Tension, typeof(AudioClip), false);
        pmr.MainAlt = (AudioClip) EditorGUILayout.ObjectField("Main Alt", pmr.MainAlt, typeof(AudioClip), false);
        pmr.Extra = (AudioClip) EditorGUILayout.ObjectField("Extra", pmr.Extra, typeof(AudioClip), false);
        pmr.Main2 = (AudioClip) EditorGUILayout.ObjectField("Main 2", pmr.Main2, typeof(AudioClip), false);
        pmr.Action2 = (AudioClip) EditorGUILayout.ObjectField("Action 2", pmr.Action2, typeof(AudioClip), false);
        pmr.Sub2 = (AudioClip) EditorGUILayout.ObjectField("Sub 2", pmr.Sub2, typeof(AudioClip), false);
        pmr.Tension2 = (AudioClip) EditorGUILayout.ObjectField("Tension 2", pmr.Tension2, typeof(AudioClip), false);
        pmr.MainAlt2 = (AudioClip) EditorGUILayout.ObjectField("Main Alt 2", pmr.MainAlt2, typeof(AudioClip), false);
        pmr.Extra2 = (AudioClip) EditorGUILayout.ObjectField("Extra 2", pmr.Extra2, typeof(AudioClip), false);
        pmr.Dirtmouth = EditorGUILayout.Toggle("Dirtmouth", pmr.Dirtmouth);
        pmr.MinesDelay = EditorGUILayout.Toggle("Mines Delay", pmr.MinesDelay);
        pmr.EnterTrackEvent = EditorGUILayout.TextField("Enter Track Event", pmr.EnterTrackEvent);
        pmr.EnterTransitionTime = EditorGUILayout.FloatField("Enter Transition Time", pmr.EnterTransitionTime);
    }
}
