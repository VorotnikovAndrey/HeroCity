using System.IO;
using UnityEditor;
using UnityEngine;

public static class ToolsEditor
{
    private static string UserModelPath => Path.Combine(Application.persistentDataPath, _userModelFileName);
    private const string _userModelFileName = "userData.data";

    [MenuItem("Tools/User/Delete User")]
    public static void DeleteUser()
    {
        File.Delete(UserModelPath);
        UnityEngine.Debug.Log($"User deleted {UserModelPath.AddColorTag(Color.yellow)}".AddColorTag(Color.green));
    }

    public static string AddColorTag(this string message, Color color)
    {
        string result = $"<color=#{GetColorHexString(color)}>{message}</color>";
        return result;
    }

    private static string GetColorHexString(Color color)
    {
        string colorString = string.Empty;
        colorString += ((int)(color.r * 255)).ToString("X02");
        colorString += ((int)(color.g * 255)).ToString("X02");
        colorString += ((int)(color.b * 255)).ToString("X02");
        return colorString;
    }
}