using System.Text;
using UnityEditor;
using UnityEngine;
using Utils.Pathfinding;

[CustomEditor(typeof(MapWaypoint))]
public class MapWaypointEditor : UnityEditor.Editor
{
    private SerializedProperty _neighborsProp;
    private SerializedProperty _paramsProp;

    private int _entranceIndex = 0;

    private void OnEnable()
    {
        _neighborsProp = serializedObject.FindProperty("Neighbors");
        _paramsProp = serializedObject.FindProperty("Params");
    }
        
    public override void OnInspectorGUI()
    {
        MapWaypoint waypoint = (MapWaypoint)target;

        EditorGUILayout.HelpBox(GetHelpMessage(), MessageType.Info);
            
        DrawTypeContent(waypoint);
        DrawEntranceContent(waypoint);
            
        GUILayout.Space(5);
            
        EditorGUILayout.PropertyField(_neighborsProp);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_paramsProp);
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
            
        //base.OnInspectorGUI();
    }
        
    private string GetHelpMessage()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Continue Waypoint: CMD/CTRL + W");
        sb.AppendLine("Connect Waypoints: CMD/CTRL + SHIFT + W");
        sb.AppendLine("Disconnect Waypoints: SHIFT + W");
        sb.AppendLine("Divide pair with new waypoint: CMD/CTRL + SHIFT + D");
        return sb.ToString();
    }

    private void DrawTypeContent(MapWaypoint waypoint)
    {
        EditorGUILayout.BeginHorizontal();

        MapWaypointType newValue = (MapWaypointType) EditorGUILayout.EnumPopup(waypoint.WaypointType);
        if(newValue != waypoint.WaypointType)
        {
            waypoint.WaypointType = newValue;
            waypoint.TypeChanged();
        }

        if (GUILayout.Button("Update"))
        {
            waypoint.Validate();
        }
            
        EditorGUILayout.EndHorizontal();
    }

    private void DrawEntranceContent(MapWaypoint waypoint)
    {
        if (waypoint.buildingIds == null || waypoint.buildingIds.Count == 0) return;
            
        EditorGUILayout.BeginHorizontal();
            
        EditorGUILayout.LabelField("Set entrance to:");
        _entranceIndex = EditorGUILayout.Popup(_entranceIndex, waypoint.buildingIds.ToArray());
            
        if (GUILayout.Button("Set Entrance"))
        {
            waypoint.SetAsEntranceTo(waypoint.buildingIds[_entranceIndex]);
        }

        EditorGUILayout.EndHorizontal();
    }
}