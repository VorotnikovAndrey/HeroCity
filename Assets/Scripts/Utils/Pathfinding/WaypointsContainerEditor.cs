using UnityEditor;
using UnityEngine;
using Utils.Pathfinding;

[CustomEditor(typeof(WaypointsContainer))]
public class WaypointsContainerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        WaypointsContainer waypointsContainer = (WaypointsContainer)target;

        ShowRefreshButton(waypointsContainer);

        GUILayout.Space(5);

        base.OnInspectorGUI();
    }

    private void ShowRefreshButton(WaypointsContainer waypointsContainer)
    {
        if (GUILayout.Button("Find all waypoints", GUILayout.Height(40)))
        {
            waypointsContainer.CacheWaypoints();
            EditorUtility.SetDirty(waypointsContainer);
        }
    }
}