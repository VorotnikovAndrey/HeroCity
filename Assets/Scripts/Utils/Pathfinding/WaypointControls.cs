using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using Utils.Pathfinding;

public static class WaypointControls
{
    [MenuItem("Tools/Waypoints/Continue Waypoint %w")]
    public static void ContinueWaypoint()
    {
        var selectedWp = Selection.activeGameObject?.GetComponent<MapWaypoint>();
        if (selectedWp == null)
            return;
        var parent = selectedWp.transform.parent;
        var newWp = CreateWaypoint(selectedWp.transform.position, parent);
        newWp.Neighbors.Add(selectedWp);
        newWp.Validate();
        newWp.WaypointType = selectedWp.WaypointType;
        newWp.name = selectedWp.name;
        selectedWp.Validate();

        var container = parent.GetComponent<WaypointsContainer>();
        if (container != null)
            container.CacheWaypoints();

        MakeSceneDirty();
    }

    [MenuItem("Tools/Waypoints/Connect Waypoints %#w")]
    static void ConnectWaypoints()
    {
        var selectedWps = Selection.gameObjects?.Select(o => o.GetComponent<MapWaypoint>()).ToArray();
        ConnectWaypointsArray(selectedWps);
    }

    [MenuItem("Tools/Waypoints/Disconnect Waypoints #w")]
    static void DisconnectWaypoints()
    {
        var selectedWps = Selection.gameObjects?.Select(o => o.GetComponent<MapWaypoint>()).ToArray();
        DisconnectWaypointsArray(selectedWps);
    }

    [MenuItem("Tools/Waypoints/Divide pair with new waypoint %#d")]
    static void DivideWithWaypoint()
    {
        var selectedWps = Selection.gameObjects?.Select(o => o.GetComponent<MapWaypoint>()).ToArray();
        DivideWaypointArray(selectedWps);
    }

    private static void MakeSceneDirty()
    {
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null)
            EditorSceneManager.MarkSceneDirty(prefabStage.scene);
    }

    public static MapWaypoint CreateWaypoint(Vector3 pos, Transform parent)
    {
        var newWp = new GameObject("Waypoint").AddComponent<MapWaypoint>();
        Undo.RegisterCreatedObjectUndo(newWp.gameObject, "Waypoint");
        newWp.gameObject.layer = 10;
        newWp.transform.parent = parent;
        newWp.transform.position = pos + Vector3.right * 0.2f;
        Selection.activeObject = newWp.gameObject;
        return newWp;
    }

    public static void ConnectWaypointsArray(MapWaypoint[] selectedWps)
    {
        if (selectedWps == null || selectedWps.Length < 2)
            return;

        foreach (var wp in selectedWps)
            Undo.RecordObject(wp.gameObject, "Waypoint connection");

        foreach (var wp1 in selectedWps)
        foreach (var wp2 in selectedWps)
            if (wp1 != wp2)
                wp1.Neighbors.Add(wp2);

        foreach (var wp in selectedWps)
            wp.Validate();

        MakeSceneDirty();
    }

    public static void DisconnectWaypointsArray(MapWaypoint[] selectedWps)
    {
        if (selectedWps == null || selectedWps.Length < 2)
            return;

        foreach (var wp in selectedWps)
            Undo.RecordObject(wp.gameObject, "Waypoint disconnection");

        foreach (var wp1 in selectedWps)
        foreach (var wp2 in selectedWps)
            if (wp1 != wp2)
                wp1.Neighbors.Remove(wp2);

        foreach (var wp in selectedWps)
            wp.Validate();

        MakeSceneDirty();
    }

    public static void DivideWaypointArray(MapWaypoint[] selectedWps)
    {
        if (selectedWps == null || selectedWps.Length != 2)
            return;

        foreach (var wp in selectedWps)
            Undo.RecordObject(wp.gameObject, "Divide pair with new waypoint");

        DisconnectWaypoints();

        var pos = selectedWps[0].Position + (selectedWps[1].Position - selectedWps[0].Position) / 2;
        var newWp = CreateWaypoint(pos, selectedWps[0].transform.parent);
        newWp.Neighbors.Add(selectedWps[0]);
        newWp.Neighbors.Add(selectedWps[1]);
        selectedWps[0].Validate();
        selectedWps[1].Validate();
        newWp.Validate();

        MakeSceneDirty();
    }
}