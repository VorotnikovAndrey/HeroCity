using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Asyncoroutine;
using UnityEditor;
using UnityEngine;

public class ScreenShotWindow : EditorWindow
{
    private static string _directory = "/ScreenShots/";
    private static float _timeScale = 1;

    [MenuItem("Window/ScreenShot Window")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ScreenShotWindow), false, "ScreenShot Window");
    }

    private void OnGUI()
    {
        ShowTimeScaleBar();
        ShowNavigationButtons();
    }

    private void ShowNavigationButtons()
    {
        GUI.backgroundColor = Application.isPlaying ? Color.green : Color.yellow;

        if (!Application.isPlaying)
        {
            GUI.contentColor = Color.red;
            string text = "The ability to create screenshots, available only in the running game mode!";
            var areaStyle = new GUIStyle(GUI.skin.label);
            areaStyle.normal.textColor = Color.red;
            areaStyle.wordWrap = true;
            areaStyle.fontStyle = FontStyle.Bold;
            areaStyle.margin = new RectOffset(2, 0, 10, 10);
            areaStyle.CalcSize(new GUIContent(text));
            EditorGUILayout.LabelField(text, areaStyle);
            GUI.contentColor = Color.black;
        }
        else
        {
            EditorGUILayout.Space(10);
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Print Screen", GUILayout.ExpandWidth(true), GUILayout.Height(50)))
        {
            CreateScreenShot();
        }

        GUI.backgroundColor = Color.white;
        if (GUILayout.Button("Open Folder", GUILayout.ExpandWidth(true), GUILayout.Height(50)))
        {
            ShowExplorer();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void ShowTimeScaleBar()
    {
        EditorGUILayout.BeginVertical();

        string text = "Use the timeline to simulate a pause during which you can take a screenshot";
        var areaStyle = new GUIStyle(GUI.skin.label)
        {
            wordWrap = true, fontStyle = FontStyle.Bold, margin = new RectOffset(2, 0, 10, 10)
        };
        areaStyle.CalcSize(new GUIContent(text));
        EditorGUILayout.LabelField(text, areaStyle);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Min", GUILayout.Width(37), GUILayout.Height(25)))
        {
            _timeScale = 0;
        }

        _timeScale = EditorGUILayout.Slider(_timeScale, 0, 10, GUILayout.Height(20));

        if (GUILayout.Button("Default", GUILayout.Width(60), GUILayout.Height(25)))
        {
            _timeScale = 1;
        }

        Time.timeScale = _timeScale;

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private static async void CreateScreenShot()
    {
        await new WaitForEndOfFrame();

        var data = ScreenCapture.CaptureScreenshotAsTexture(4);
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, data.width, data.height), 0, 0);
        texture.Apply();

        string resolutionText = $"{Screen.width}x{Screen.height}";
        var bytes = texture.EncodeToPNG();

        DestroyImmediate(data);

        string path = Application.dataPath + _directory + $"/{"Default"}/{resolutionText}/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        File.WriteAllBytes($"{path}{"Default"}_{resolutionText}_{Guid.NewGuid()}.png", bytes);

        AssetDatabase.Refresh();
    }

    public void ShowExplorer()
    {
        string path = Application.dataPath + _directory;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string file = Directory.EnumerateFiles(path).FirstOrDefault();
        EditorUtility.RevealInFinder(!string.IsNullOrEmpty(file) ? Path.Combine(path, file) : path);
    }
}
